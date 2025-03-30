namespace TAmeAtCoderLibrary;

/// <summary>
/// 双方向連結リストを実装したクラスです。要素は前後の要素への参照を持ちます。
/// </summary>
/// <typeparam name="T">リストに格納する要素の型</typeparam>
public class DoublyLinkedList<T>
{
    private readonly Dictionary<T, Node> _nodeMap = new();

    /// <summary>
    /// 双方向連結リストの新しいインスタンスを初期化します。
    /// </summary>
    public DoublyLinkedList()
    { }

    /// <summary>
    /// 指定された値の前の要素を取得します。
    /// </summary>
    /// <param name="value">検索する値</param>
    /// <param name="before">前の要素が格納される出力パラメータ</param>
    /// <returns>前の要素が存在する場合はtrue、存在しない場合はfalse</returns>
    public bool TryGetBefore(T value, out T before)
    {
        if (!_nodeMap.TryGetValue(value, out var node) || node.Before == null)
        {
            before = default;
            return false;
        }

        before = node.Before.Value;
        return true;
    }

    /// <summary>
    /// 指定された値の次の要素を取得します。
    /// </summary>
    /// <param name="value">検索する値</param>
    /// <param name="next">次の要素が格納される出力パラメータ</param>
    /// <returns>次の要素が存在する場合はtrue、存在しない場合はfalse</returns>
    public bool TryGetNext(T value, out T next)
    {
        if (!_nodeMap.TryGetValue(value, out var node) || node.Next == null)
        {
            next = default;
            return false;
        }

        next = node.Next.Value;
        return true;
    }

    /// <summary>
    /// リストに新しい要素を追加します。既に存在する場合は追加されません。
    /// </summary>
    /// <param name="value">追加する値</param>
    public void Add(T value)
    {
        if (!_nodeMap.ContainsKey(value))
            _nodeMap.Add(value, new Node(value));
    }

    /// <summary>
    /// 2つの要素を連結させ、前後の関係を設定します。
    /// 既に他の要素と接続されている場合は、その接続が上書きされます。
    /// </summary>
    /// <param name="before">前の要素とする値</param>
    /// <param name="next">次の要素とする値</param>
    public void Link(T before, T next)
    {
        Add(before);
        Add(next);

        _nodeMap[before].Next = _nodeMap[next];
        _nodeMap[next].Before = _nodeMap[before];
    }

    /// <summary>
    /// 指定された要素と前の要素との接続を切ります。
    /// </summary>
    /// <param name="current">対象の要素</param>
    public void UnlinkBefore(T current)
    {
        if (!_nodeMap.TryGetValue(current, out var currentNode) || currentNode.Before == null)
            return;

        currentNode.Before.Next = null;
        currentNode.Before = null;
    }

    /// <summary>
    /// 指定された要素と次の要素との接続を切ります。
    /// </summary>
    /// <param name="current">対象の要素</param>
    public void UnlinkNext(T current)
    {
        if (!_nodeMap.TryGetValue(current, out var currentNode) || currentNode.Next == null)
            return;

        currentNode.Next.Before = null;
        currentNode.Next = null;
    }

    /// <summary>
    /// 指定された要素の前に新しい要素を追加します。
    /// </summary>
    /// <param name="current">現在の要素</param>
    /// <param name="newBefore">前に追加する新しい要素</param>
    /// <param name="reConnect">元の前の要素との接続を維持するかどうか</param>
    public void AddBefore(T current, T newBefore, bool reConnect = false)
    {
        Add(current);
        Add(newBefore);

        // 現在のノードと新しい前ノードを取得
        Node currentNode = _nodeMap[current];
        Node newBeforeNode = _nodeMap[newBefore];

        // 現在のノードの元の前ノードを保存（再接続用）
        Node originalBefore = currentNode.Before;

        // 新しい接続を設定
        currentNode.Before = newBeforeNode;
        newBeforeNode.Next = currentNode;

        // 元の前の要素との接続を維持する場合
        if (reConnect && originalBefore != null)
        {
            newBeforeNode.Before = originalBefore;
            originalBefore.Next = newBeforeNode;
        }
    }

    /// <summary>
    /// 指定された要素の次に新しい要素を追加します。
    /// </summary>
    /// <param name="current">現在の要素</param>
    /// <param name="newNext">次に追加する新しい要素</param>
    /// <param name="reConnect">元の次の要素との接続を維持するかどうか</param>
    public void AddAfter(T current, T newNext, bool reConnect = false)
    {
        Add(current);
        Add(newNext);

        // 現在のノードと新しい次ノードを取得
        Node currentNode = _nodeMap[current];
        Node newNextNode = _nodeMap[newNext];

        // 現在のノードの元の次ノードを保存（再接続用）
        Node originalNext = currentNode.Next;

        // 新しい接続を設定
        currentNode.Next = newNextNode;
        newNextNode.Before = currentNode;

        // 元の次の要素との接続を維持する場合
        if (reConnect && originalNext != null)
        {
            newNextNode.Next = originalNext;
            originalNext.Before = newNextNode;
        }
    }

    /// <summary>
    /// 指定した要素から始まるリンクリストをキューに変換します。
    /// 指定した要素から最も前の要素まで遡り、そこから順にキューに格納します。
    /// </summary>
    /// <param name="current">開始要素</param>
    /// <returns>リンクリストの要素を順序通りに格納したキュー</returns>
    public Queue<T> ToQueue(T current)
    {
        var queue = new Queue<T>();

        if (!_nodeMap.TryGetValue(current, out var startNode))
            return queue;

        // 先頭ノードを見つける
        var headNode = FindHeadNode(startNode);

        // 先頭から順番にキューに追加
        var node = headNode;
        while (node != null)
        {
            queue.Enqueue(node.Value);
            node = node.Next;
        }

        return queue;
    }

    /// <summary>
    /// リンクリストの最初の要素からキューに変換します。
    /// </summary>
    /// <returns>リンクリストの要素を順序通りに格納したキュー</returns>
    public Queue<T> ToQueue()
    {
        var queue = new Queue<T>();

        if (_nodeMap.Count == 0)
            return queue;

        // 任意のノードを取得し、そこから先頭を探す
        if (!_nodeMap.TryGetValue(_nodeMap.Keys.First(), out var anyNode))
            return queue;

        // 先頭ノードを見つける
        var headNode = FindHeadNode(anyNode);

        // 先頭から順番にキューに追加
        var node = headNode;
        while (node != null)
        {
            queue.Enqueue(node.Value);
            node = node.Next;
        }

        return queue;
    }

    /// <summary>
    /// 指定されたノードから前方向に辿り、リストの先頭ノードを見つけます。
    /// </summary>
    /// <param name="node">開始ノード</param>
    /// <returns>リストの先頭ノード</returns>
    private Node FindHeadNode(Node node)
    {
        while (node.Before != null)
            node = node.Before;

        return node;
    }

    /// <summary>
    /// 双方向リンクリストのノードを表す内部クラスです。
    /// 値と前後のノードへの参照を保持します。
    /// </summary>
    private class Node
    {
        internal T Value;
        internal Node Before;
        internal Node Next;

        /// <summary>
        /// 指定された値を持つ新しいノードを初期化します。
        /// </summary>
        /// <param name="value">ノードが保持する値</param>
        internal Node(T value)
        {
            this.Value = value;
        }
    }
}
