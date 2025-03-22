namespace TAmeAtCoderLibrary;

using System;
using System.Collections.Generic; // IEnumerable<T1> を使用するために必要

/// <summary>
/// 自己平衡二分探索木です。挿入、削除、検索の各操作が常にO(log n)の時間計算量で行えます。
/// </summary>
/// <typeparam name="T1">木に格納する要素の型。IComparable<T1> インターフェースを実装している必要があります。</typeparam>
public class AvlTree<T1> where T1 : IComparable<T1>
{
    /// <summary>
    /// ルートノードを取得または設定します。
    /// </summary>
    public TreeNode RootNode { get; private set; }
    /// <summary>
    /// 木の最小キー値を取得します。木が空の場合は型のデフォルト値を返します。
    /// </summary>
    public T1 MinKey => RootNode == null ? default : RootNode.GetMin();
    /// <summary>
    /// 木の最大キー値を取得します。木が空の場合は型のデフォルト値を返します。
    /// </summary>
    public T1 MaxKey => RootNode == null ? default : RootNode.GetMax();

    /// <summary>
    /// 空の<see cref="AvlTree{T1}"/>クラスの新しいインスタンスを初期化します。
    /// </summary>
    public AvlTree() { }

    /// <summary>
    /// 指定したコレクションに含まれる要素を使用して、<see cref="AvlTree{T1}"/>クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="enum">初期化する要素を含むコレクション。</param>
    public AvlTree(IEnumerable<T1> @enum)
    {
        foreach (var item in @enum)
            Add(item);
    }

    /// <summary>
    /// 木に新しい要素を追加します。同じ要素が既に存在する場合は、何も行いません。
    /// </summary>
    /// <param name="item">追加する要素。</param>
    public void Add(T1 item)
    {
        if (Contains(item))
            return;

        if (RootNode == null) RootNode = new TreeNode(item, this);
        else RootNode.Add(item);
    }

    /// <summary>
    /// 木からすべての要素を削除します。
    /// </summary>
    public void Clear()
    {
        if (RootNode == null) return;
        RootNode.Clear();
        RootNode = null;
    }

    /// <summary>
    /// 指定した要素が木に含まれているかどうかを判断します。
    /// </summary>
    /// <param name="item">木内で検索する要素。</param>
    /// <returns>指定した要素が木に含まれている場合は<c>true</c>。それ以外の場合は<c>false</c>。</returns>
    public bool Contains(T1 item) => RootNode != null && RootNode.Contains(item);

    /// <summary>
    /// 指定した値よりも小さい最大の値を内部的に取得します。
    /// </summary>
    /// <param name="item">基準となる値。</param>
    /// <param name="min">現在の最小値。</param>
    /// <returns>指定した値よりも小さい最大の値を返します。</returns>
    private T1 _GetBelow(T1 item, T1 min) => RootNode == null ? min : RootNode.GetBelow(item, min);
    /// <summary>
    /// 指定した値よりも大きい最小の値を内部的に取得します。
    /// </summary>
    /// <param name="item">基準となる値。</param>
    /// <param name="max">現在の最大値。</param>
    /// <returns>指定した値よりも大きい最小の値を返します。</returns>
    private T1 _GetNext(T1 item, T1 max) => RootNode == null ? max : RootNode.GetAbove(item, max);
    /// <summary>
    /// 指定したキー以下の最大のキーを返します。
    /// </summary>
    /// <param name="item">基準となるキー。</param>
    /// <param name="minKey">許容される最小のキー。</param>
    /// <returns>指定したキー以下の最大のキー。そのようなキーが存在しない場合は <paramref name="minKey"/> を返します。</returns>
    public T1 GetBelow(T1 item, T1 minKey) => Contains(item) ? item : _GetBelow(item, minKey);
    /// <summary>
    /// 指定したキー以上の最小のキーを返します。
    /// </summary>
    /// <param name="item">基準となるキー。</param>
    /// <param name="maxKey">許容される最大のキー。</param>
    /// <returns>指定したキー以上の最小のキー。そのようなキーが存在しない場合は <paramref name="maxKey"/> を返します。</returns>
    public T1 GetNext(T1 item, T1 maxKey) => Contains(item) ? item : _GetNext(item, maxKey);

    /// <summary>
    /// 木に格納されている要素の数を取得します。
    /// </summary>
    public int Count { get { return RootNode == null ? 0 : RootNode.Count; } }

    /// <summary>
    /// 木から指定した要素を削除します。要素が見つからない場合は、何も行いません。
    /// </summary>
    /// <param name="item">削除する要素。</param>
    public void Remove(T1 item)
    {
        RootNode.Remove(item);
    }

    /// <summary>
    /// 木の要素を昇順で含むリストを返します。
    /// </summary>
    /// <returns>木の要素を昇順で並べた<see cref="List{T1}"/>。</returns>
    public List<T1> InOrder()
    {
        var queue = new List<T1>();
        RootNode.InOrder(queue);

        return queue;
    }

    /// <summary>
    /// 指定したインデックスにある要素を取得します。
    /// </summary>
    /// <param name="index">取得する要素のインデックス（0から始まる）。</param>
    /// <returns>指定したインデックスにある要素。</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> が範囲外の場合。</exception>
    /// <exception cref="InvalidOperationException">インデックスへの設定操作はサポートされていません。</exception>
    public T1 this[int index]
    {
        get
        {
            if (RootNode != null) return RootNode[index];
            else throw new ArgumentOutOfRangeException("index");
        }
        set { throw new InvalidOperationException("インデックスによる設定操作はサポートされていません。"); }
    }

    /// <summary>
    /// <see cref="AvlTree{T1}"/>クラスのノードを表します。
    /// </summary>
    public class TreeNode
    {
        /// <summary>
        /// <see cref="TreeNode"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="item">ノードに格納する値。</param>
        /// <param name="tree">このノードが属する<see cref="AvlTree{T1}"/>インスタンス。</param>
        public TreeNode(T1 item, AvlTree<T1> tree)
        {
            Value = item;
            Level = 1;
            Count = 1;
            Tree = tree;
        }
        /// <summary>
        /// このノードが属する<see cref="AvlTree{T1}"/>インスタンスを取得します。
        /// </summary>
        public AvlTree<T1> Tree { get; private set; }
        /// <summary>
        /// ノードの値を読み取り専用で取得します。
        /// </summary>
        public T1 Value { get; private set; }
        /// <summary>
        /// 親ノードを取得または設定します。
        /// </summary>
        public TreeNode Parent { get; private set; }
        /// <summary>
        /// 左の子ノードを取得または設定します。
        /// </summary>
        public TreeNode Left { get; private set; }
        /// <summary>
        /// 右の子ノードを取得または設定します。
        /// </summary>
        public TreeNode Right { get; private set; }
        /// <summary>
        /// このノードの高さ（レベル）を取得または設定します。
        /// </summary>
        int Level { get; set; }
        /// <summary>
        /// このノードをルートとする部分木のノード数を取得します。
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// このノードまたはその子孫に新しい要素を追加します。同じ要素が既に存在する場合は、何も行いません。
        /// </summary>
        /// <param name="item">追加する要素。</param>
        public void Add(T1 item)
        {
            var compare = item.CompareTo(Value);
            if (compare < 0)
                if (Left == null)
                    ((Left = new TreeNode(item, Tree)).Parent = this).Reconstruct(true);
                else Left.Add(item);
            else
                if (Right == null)
                ((Right = new TreeNode(item, Tree)).Parent = this).Reconstruct(true);
            else Right.Add(item);
        }

        /// <summary>
        /// このノードをルートとする部分木からすべてのノードを削除します。
        /// </summary>
        public void Clear()
        {
            Left?.Clear();
            Right?.Clear();
            Left = Right = null;
        }

        /// <summary>
        /// 指定した要素がこのノードまたはその子孫に含まれているかどうかを判断します。
        /// </summary>
        /// <param name="item">検索する要素。</param>
        /// <returns>指定した要素がこのノードまたはその子孫に含まれている場合は<c>true</c>。それ以外の場合は<c>false</c>。</returns>
        public bool Contains(T1 item)
        {
            var compare = item.CompareTo(Value);
            if (compare < 0)
                return Left != null && Left.Contains(item);
            else if (compare == 0)
                return true;
            else
                return Right != null && Right.Contains(item);
        }

        /// <summary>
        /// このノードをルートとする部分木の最小の値を取得します。
        /// </summary>
        /// <returns>部分木の最小の値。</returns>
        public T1 GetMin() => Left == null ? Value : Left.GetMin();
        /// <summary>
        /// このノードをルートとする部分木の最大の値を取得します。
        /// </summary>
        /// <returns>部分木の最大の値。</returns>
        public T1 GetMax() => Right == null ? Value : Right.GetMax();

        /// <summary>
        /// 指定した値よりも小さい最大の値をこの部分木内で検索します。
        /// </summary>
        /// <param name="item">基準となる値。</param>
        /// <param name="maxKey">現在見つかっている最大のキー。</param>
        /// <returns>指定した値よりも小さい最大のキー。存在しない場合は <paramref name="maxKey"/> を返します。</returns>
        public T1 GetBelow(T1 item, T1 maxKey)
        {
            var compare = item.CompareTo(Value);

            if (compare > 0 && Value.CompareTo(maxKey) >= 0)
                maxKey = Value;

            if (compare <= 0)
                return Left == null ? maxKey : Left.GetBelow(item, maxKey);
            else
                return Right == null ? maxKey : Right.GetBelow(item, maxKey);
        }

        /// <summary>
        /// 指定した値よりも大きい最小の値をこの部分木内で検索します。
        /// </summary>
        /// <param name="item">基準となる値。</param>
        /// <param name="minKey">現在見つかっている最小のキー。</param>
        /// <returns>指定した値よりも大きい最小のキー。存在しない場合は <paramref name="minKey"/> を返します。</returns>
        public T1 GetAbove(T1 item, T1 minKey)
        {
            var compare = item.CompareTo(Value);

            if (compare < 0 && Value.CompareTo(minKey) <= 0)
                minKey = Value;

            if (compare >= 0)
                return Right == null ? minKey : Right.GetAbove(item, minKey);
            else
                return Left == null ? minKey : Left.GetAbove(item, minKey);
        }

        /// <summary>
        /// このコレクションが読み取り専用かどうかを示す値を取得します。（常に<c>false</c>を返します）
        /// </summary>
        public bool IsReadOnly { get { return false; } }

        /// <summary>
        /// このノードまたはその子孫から指定した要素を削除します。要素が見つかった場合は<c>true</c>を返します。
        /// </summary>
        /// <param name="item">削除する要素。</param>
        /// <returns>要素が削除された場合は<c>true</c>。それ以外の場合は<c>false</c>。</returns>
        public bool Remove(T1 item)
        {
            var compare = item.CompareTo(Value);
            if (compare == 0)
            {
                // 子ノードがない場合
                if (Left == null && Right == null)
                    if (Parent != null)
                    {
                        if (Parent.Left == this) Parent.Left = null;
                        else Parent.Right = null;
                        Parent.Reconstruct(true);
                    }
                    else Tree.RootNode = null;
                // 子ノードが1つだけの場合
                else if (Left == null || Right == null)
                {
                    var child = Left == null ? Right : Left;
                    if (Parent != null)
                    {
                        if (Parent.Left == this) Parent.Left = child;
                        else Parent.Right = child;
                        (child.Parent = Parent).Reconstruct(true);
                    }
                    else (Tree.RootNode = child).Parent = null;
                }
                // 子ノードが2つある場合
                else
                {
                    // 左部分木の最大値と値を交換して削除
                    var replace = Left;
                    while (replace.Right != null) replace = replace.Right;
                    var temp = Value;
                    Value = replace.Value;
                    replace.Value = temp;
                    return replace.Remove(replace.Value);
                }
                Parent = Left = Right = null;
                return true;
            }
            else if (compare < 0)
                return Left == null ? false : Left.Remove(item);
            else
                return Right == null ? false : Right.Remove(item);
        }

        /// <summary>
        /// AVL木のバランスを再構築します。
        /// </summary>
        /// <param name="recursive">親ノードに対しても再帰的に再構築を行うかどうか。</param>
        void Reconstruct(bool recursive)
        {
            Count = 1;

            int leftLevel = 0, rightLevel = 0;
            if (Left != null)
            {
                leftLevel = Left.Level;
                Count += Left.Count;
            }
            if (Right != null)
            {
                rightLevel = Right.Level;
                Count += Right.Count;
            }

            // 左部分木が右部分木より2以上高い場合（左偏り）
            if (leftLevel - rightLevel > 1)
            {
                var leftLeft = Left.Left == null ? 0 : Left.Left.Level;
                var leftRight = Left.Right == null ? 0 : Left.Right.Level;
                if (leftLeft >= leftRight)
                {
                    // 右回転（RR回転）
                    Left.Elevate();
                    Reconstruct(true);
                }
                else
                {
                    // 左右回転（LR回転）
                    var pivot = Left.Right;
                    pivot.Elevate(); pivot.Elevate();
                    pivot.Left.Reconstruct(false);
                    pivot.Right.Reconstruct(true);
                }
            }
            // 右部分木が左部分木より2以上高い場合（右偏り）
            else if (rightLevel - leftLevel > 1)
            {
                var rightRight = Right.Right == null ? 0 : Right.Right.Level;
                var rightLeft = Right.Left == null ? 0 : Right.Left.Level;
                if (rightRight >= rightLeft)
                {
                    // 左回転（LL回転）
                    Right.Elevate();
                    Reconstruct(true);
                }
                else
                {
                    // 右左回転（RL回転）
                    var pivot = Right.Left;
                    pivot.Elevate(); pivot.Elevate();
                    pivot.Left.Reconstruct(false);
                    pivot.Right.Reconstruct(true);
                }
            }
            else
            {
                // 高さを更新
                Level = Math.Max(leftLevel, rightLevel) + 1;
                if (Parent != null && recursive)
                    Parent.Reconstruct(true);
            }
        }

        /// <summary>
        /// ノードを1レベル上昇させます（AVL木の回転操作の一部）。
        /// </summary>
        void Elevate()
        {
            var root = Parent;
            var parent = root.Parent;
            if ((Parent = parent) == null) Tree.RootNode = this;
            else
            {
                if (parent.Left == root) parent.Left = this;
                else parent.Right = this;
            }

            if (root.Left == this)
            {
                root.Left = Right;
                if (Right != null) Right.Parent = root;
                Right = root;
                root.Parent = this;
            }
            else
            {
                root.Right = Left;
                if (Left != null) Left.Parent = root;
                Left = root;
                root.Parent = this;
            }
        }

        /// <summary>
        /// 指定したインデックスにある要素を取得します。
        /// </summary>
        /// <param name="index">取得する要素のインデックス（0から始まる）。</param>
        /// <returns>指定したインデックスにある要素。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> が範囲外の場合。</exception>
        /// <exception cref="InvalidOperationException">インデックスへの設定操作はサポートされていません。</exception>
        public T1 this[int index]
        {
            get
            {
                if (Left != null)
                    if (index < Left.Count) return Left[index];
                    else index -= Left.Count;
                if (index-- == 0) return Value;
                if (Right != null)
                    if (index < Right.Count) return Right[index];
                throw new ArgumentOutOfRangeException("index");
            }
            set { throw new InvalidOperationException("インデックスによる設定操作はサポートされていません。"); }
        }

        /// <summary>
        /// このノードをルートとする部分木の要素を昇順で指定されたリストに追加します（中間順巡回）。
        /// </summary>
        /// <param name="queue">要素を追加するリスト。</param>
        public void InOrder(List<T1> queue)
        {
            Left?.InOrder(queue);
            queue.Add(Value);
            Right?.InOrder(queue);
        }
    }
}
