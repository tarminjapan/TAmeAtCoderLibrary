#nullable enable 
using System.Diagnostics.CodeAnalysis;

namespace TJAtCoderLibs;

/// <summary>
/// Union-Find Tree (Disjoint Set Union) データ構造。
/// </summary>
/// <typeparam name="T">要素の型。null 非許容である必要があります。</typeparam>
/// <remarks>
/// 互いに素な集合（要素のグループ）を効率的に管理します。連結成分の追跡などに有用です。
/// この実装では、経路圧縮 (Path Compression) とサイズによる結合 (Union by Size) を用いて最適化されています。
/// スレッドセーフではありません。
/// </remarks>
public class UnionFindTree<T> where T : notnull
{
    // 要素IDをキーとし、その要素の親とサイズ情報を持つノードを値とする辞書
    private readonly Dictionary<T, Node> _nodes;
    // 要素の比較に使用する EqualityComparer
    private readonly IEqualityComparer<T> _comparer;

    /// <summary>
    /// UnionFindTree クラスの新しいインスタンスを初期化します。
    /// デフォルトの EqualityComparer を使用します。
    /// </summary>
    public UnionFindTree() : this(EqualityComparer<T>.Default) { }

    /// <summary>
    /// 指定された EqualityComparer を使用して、UnionFindTree クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="comparer">要素の比較に使用する EqualityComparer。</param>
    /// <exception cref="ArgumentNullException">comparer が null の場合にスローされます。</exception>
    public UnionFindTree(IEqualityComparer<T> comparer)
    {
        _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        _nodes = new Dictionary<T, Node>(_comparer);
    }

    /// <summary>
    /// Union-Find ツリー内のノードを表します。
    /// </summary>
    private class Node
    {
        /// <summary>このノードの親要素。自身を指している場合、このノードは根です。</summary>
        public T Parent { get; set; }
        /// <summary>このノードを根とする木のサイズ（要素数）。根ノードでのみ有効な値です。</summary>
        public int Size { get; set; }

        public Node(T id)
        {
            Parent = id; // 初期状態では自身が親 (根)
            Size = 1;
        }
    }

    /// <summary>
    /// 新しい要素を、それ自身のみを含む新しい集合（新しい根）として追加します。
    /// </summary>
    /// <param name="id">追加する要素の ID。</param>
    /// <returns>要素が正常に追加された場合は true。要素が既に存在していた場合は false。</returns>
    /// <exception cref="ArgumentNullException">id が null の場合にスローされます。</exception>
    public bool Add(T id)
    {
        if (id == null) throw new ArgumentNullException(nameof(id));
        // C# 8.0以降の Dictionary.TryAdd を使うとより簡潔
        // return _nodes.TryAdd(id, new Node(id));
        if (_nodes.ContainsKey(id))
        {
            return false;
        }
        _nodes.Add(id, new Node(id));
        return true;
    }

    /// <summary>
    /// 指定された要素が Union-Find ツリーに存在するかどうかを確認します。
    /// </summary>
    /// <param name="id">確認する要素の ID。</param>
    /// <returns>要素が存在する場合は true、それ以外の場合は false。</returns>
    /// <exception cref="ArgumentNullException">id が null の場合にスローされます。</exception>
    public bool Contains(T id)
    {
        if (id == null) throw new ArgumentNullException(nameof(id));
        return _nodes.ContainsKey(id);
    }

    /// <summary>
    /// 指定された要素が含まれる集合の代表元（根）を検索します。
    /// 検索中に経路圧縮を行い、木の高さを低く保ちます。
    /// </summary>
    /// <param name="id">検索する要素の ID。</param>
    /// <returns>指定された要素が含まれる集合の根の ID。</returns>
    /// <exception cref="ArgumentNullException">id が null の場合にスローされます。</exception>
    /// <exception cref="KeyNotFoundException">指定された要素がツリー内に見つからない場合にスローされます。</exception>
    public T FindRoot(T id)
    {
        if (id == null) throw new ArgumentNullException(nameof(id));
        // TryGetValue を使って node を non-nullable にする
        if (!_nodes.TryGetValue(id, out Node? node) || node == null) // node の null チェックも追加 (理論上不要だが念のため)
        {
            throw new KeyNotFoundException($"要素 '{id}' は Union-Find ツリー内に存在しません。");
        }

        // 根を見つける (経路圧縮なしの最初の探索)
        T root = id;
        // ループ内で _nodes[root] をキャッシュしてアクセスを減らす
        Node currentRootNode = _nodes[root];
        while (!_comparer.Equals(currentRootNode.Parent, root))
        {
            root = currentRootNode.Parent;
            currentRootNode = _nodes[root];
        }

        // 経路圧縮 (見つけた根に直接つなぎ直す)
        T currentId = id;
        while (!_comparer.Equals(_nodes[currentId].Parent, root))
        {
            T nextId = _nodes[currentId].Parent;
            _nodes[currentId].Parent = root;
            currentId = nextId;
        }

        return root;
    }

    /// <summary>
    /// 指定された要素が含まれる集合の代表元（根）を検索します。要素が存在しない場合は false を返します。
    /// </summary>
    /// <param name="id">検索する要素の ID。</param>
    /// <param name="root">要素が存在する場合、その集合の根の ID が格納されます。</param>
    /// <returns>要素が存在し、根が見つかった場合は true。それ以外の場合は false。</returns>
    /// <exception cref="ArgumentNullException">id が null の場合にスローされます。</exception>
    public bool TryFindRoot(T id, [MaybeNullWhen(false)] out T root)
    {
        if (id == null) throw new ArgumentNullException(nameof(id));
        if (!_nodes.ContainsKey(id))
        {
            root = default; // T が struct の場合は default(T), class の場合は null
            return false;
        }
        // 要素が存在する場合、FindRoot は例外をスローしない
        root = FindRoot(id);
        return true;
    }


    /// <summary>
    /// 指定された 2 つの要素が含まれる集合を統合します。
    /// サイズによる結合 (Union by Size) を行い、小さい方の木を大きい方の木に接続します。
    /// </summary>
    /// <param name="id1">統合する最初の要素の ID。</param>
    /// <param name="id2">統合する 2 番目の要素の ID。</param>
    /// <returns>集合が実際に統合された場合（元々異なる集合だった場合）は true。既に同じ集合に属していた場合は false。</returns>
    /// <exception cref="ArgumentNullException">id1 または id2 が null の場合にスローされます。</exception>
    /// <exception cref="KeyNotFoundException">指定された要素のいずれかがツリー内に見つからない場合にスローされます。</exception>
    public bool Union(T id1, T id2)
    {
        if (id1 == null) throw new ArgumentNullException(nameof(id1));
        if (id2 == null) throw new ArgumentNullException(nameof(id2));

        T root1 = FindRoot(id1); // FindRoot が存在しない場合の KeyNotFoundException を処理
        T root2 = FindRoot(id2);

        if (_comparer.Equals(root1, root2))
        {
            return false; // 既に同じ集合に属している
        }

        Node node1 = _nodes[root1];
        Node node2 = _nodes[root2];

        // サイズによる結合: サイズが小さい方の木を大きい方の木の根に接続する
        if (node1.Size < node2.Size)
        {
            node1.Parent = root2;
            node2.Size += node1.Size;
        }
        else
        {
            node2.Parent = root1;
            node1.Size += node2.Size;
        }

        return true;
    }

    /// <summary>
    /// 指定された要素が含まれる集合のサイズ（要素数）を取得します。
    /// </summary>
    /// <param name="id">サイズを取得する要素の ID。</param>
    /// <returns>指定された要素が含まれる集合のサイズ。</returns>
    /// <exception cref="ArgumentNullException">id が null の場合にスローされます。</exception>
    /// <exception cref="KeyNotFoundException">指定された要素がツリー内に見つからない場合にスローされます。</exception>
    public int GetSetSize(T id)
    {
        if (id == null) throw new ArgumentNullException(nameof(id));
        // FindRoot が存在チェックと null チェックを行う
        T root = FindRoot(id);
        return _nodes[root].Size;
    }

    /// <summary>
    /// 指定された 2 つの要素が同じ集合に属しているかどうかを確認します。
    /// </summary>
    /// <param name="id1">確認する最初の要素の ID。</param>
    /// <param name="id2">確認する 2 番目の要素の ID。</param>
    /// <returns>2 つの要素が同じ集合に属している場合は true。それ以外の場合は false。</returns>
    /// <exception cref="ArgumentNullException">id1 または id2 が null の場合にスローされます。</exception>
    public bool AreConnected(T id1, T id2)
    {
        if (id1 == null) throw new ArgumentNullException(nameof(id1));
        if (id2 == null) throw new ArgumentNullException(nameof(id2));

        // 最適化: 同じ要素なら存在確認のみ
        if (_comparer.Equals(id1, id2))
        {
            return Contains(id1); // 存在すれば自分自身とは接続されている
        }

        // TryFindRoot を使って、要素が存在しない場合に false を返す
        // [MaybeNullWhen(false)] のおかげで、true が返れば root1, root2 は null でないことが保証される
        if (TryFindRoot(id1, out T? root1) && TryFindRoot(id2, out T? root2))
        {
            // 両方の要素が存在する場合のみ、根を比較
            return _comparer.Equals(root1, root2);
        }

        // どちらか一方、または両方の要素が存在しない場合は接続されていない
        return false;
    }

    /// <summary>
    /// Union-Find ツリー内の集合の数（根となっている要素の数）を取得します。
    /// </summary>
    /// <returns>現在の集合（根）の数。</returns>
    public int CountSets()
    {
        // 最も直接的な方法は、全てのノードを調べて、自身が親であるノードを数えること。
        // この方法は経路圧縮をトリガーしないため、副作用が少ない。
        int count = 0;
        foreach (var kvp in _nodes)
        {
            // kvp.Key が要素ID, kvp.Value が Node インスタンス
            if (_comparer.Equals(kvp.Value.Parent, kvp.Key))
            {
                count++;
            }
        }
        return count;
    }
}