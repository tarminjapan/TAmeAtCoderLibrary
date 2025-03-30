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
    /// 指定された値をリストから削除します。値が存在しない場合は何も行いません。
    /// 値の前後の要素が存在する場合、それらの要素は自動的に連結されます。
    /// </summary>
    /// <param name="value">削除する値</param>
    public void Remove(T value)
    {
        if (!_nodeMap.ContainsKey(value))
            return;

        var reConnect = _nodeMap[value].Before != null && _nodeMap[value].Next != null;
        var beforeVal = _nodeMap[value].Before != null ? _nodeMap[value].Before.Value : default;
        var afterVal = _nodeMap[value].Next != null ? _nodeMap[value].Next.Value : default;

        UnlinkBefore(value);
        UnlinkNext(value);

        if (reConnect)
            AddNext(beforeVal, afterVal);

        _nodeMap.Remove(value);
    }

    /// <summary>
    /// 指定された要素の前に新しい要素を追加します。
    /// 既に前の要素が存在する場合、それらの関係を適切に再構築します。
    /// </summary>
    /// <param name="current">現在の要素</param>
    /// <param name="before">追加する前の要素</param>
    /// <param name="reConnect">既存の接続を維持するかどうか（デフォルトはtrue）</param>
    public void AddBefore(T current, T before, bool reConnect = true)
    {
        Add(current);
        Add(before);

        var node = _nodeMap[current];
        var beforeNode = _nodeMap[before];

        if (reConnect && node.Before != null)
        {
            var beforeBefore = node.Before.Value;
            UnlinkBefore(current);

            // 新しい要素の前に元の前の要素を連結
            Link(beforeBefore, before);
        }
        else
        {
            UnlinkBefore(current);
        }

        // 新しい要素を現在の要素の前に連結
        beforeNode.Next = node;
        node.Before = beforeNode;
    }

    /// <summary>
    /// 指定された要素の次に新しい要素を追加します。
    /// 既に次の要素が存在する場合、それらの関係を適切に再構築します。
    /// </summary>
    /// <param name="current">現在の要素</param>
    /// <param name="next">追加する次の要素</param>
    /// <param name="reConnect">既存の接続を維持するかどうか（デフォルトはtrue）</param>
    public void AddNext(T current, T next, bool reConnect = true)
    {
        Add(current);
        Add(next);

        var node = _nodeMap[current];
        var nextNode = _nodeMap[next];

        if (reConnect && node.Next != null)
        {
            var afterNext = node.Next.Value;
            UnlinkNext(current);

            // 新しい要素の次に元の次の要素を連結
            Link(next, afterNext);
        }
        else
        {
            UnlinkNext(current);
        }

        // 新しい要素を現在の要素の次に連結
        node.Next = nextNode;
        nextNode.Before = node;
    }

    /// <summary>
    /// 指定された要素と前の要素との接続を切ります。
    /// </summary>
    /// <param name="current">対象の要素</param>
    public void UnlinkBefore(T current)
    {
        if (!_nodeMap.TryGetValue(current, out var node) || node.Before == null) return;

        node.Before.Next = null;
        node.Before = null;
    }

    /// <summary>
    /// 指定された要素と次の要素との接続を切ります。
    /// </summary>
    /// <param name="current">対象の要素</param>
    public void UnlinkNext(T current)
    {
        if (!_nodeMap.TryGetValue(current, out var node) || node.Next == null) return;

        node.Next.Before = null;
        node.Next = null;
    }

    /// <summary>
    /// リンクリスト全体をキューに変換します。
    /// リストの最初の要素から始まり、すべての要素を順序通りにキューに格納します。
    /// </summary>
    /// <returns>リンクリストの要素を順序通りに格納したキュー</returns>
    public Queue<T> ToQueue()
    {
        var queue = new Queue<T>();

        if (_nodeMap.Count == 0)
            return queue;

        var head = FindHeadNode();
        if (head == null)
            return queue;

        var current = head;
        do
        {
            queue.Enqueue(current.Value);
            current = current.Next;
        } while (current != null);

        return queue;
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

        if (!_nodeMap.TryGetValue(current, out var node))
            return queue;

        // 先頭ノードを探す
        var head = node;
        while (head.Before != null)
            head = head.Before;

        // 先頭からキューに詰める
        var currentNode = head;
        do
        {
            queue.Enqueue(currentNode.Value);
            currentNode = currentNode.Next;
        } while (currentNode != null);

        return queue;
    }

    /// <summary>
    /// リンクリストの先頭ノードを検索します。
    /// </summary>
    /// <returns>先頭ノード。リストが空の場合はnull</returns>
    private Node FindHeadNode()
    {
        if (_nodeMap.Count == 0)
            return null;

        var anyNode = _nodeMap.Values.First();
        var head = anyNode;

        // 先頭ノード（Beforeがnull）を見つける
        while (head.Before != null)
            head = head.Before;

        return head;
    }

    /// <summary>
    /// 双方向リンクリストのノードを表す内部クラスです。
    /// 値と前後のノードへの参照を保持します。
    /// </summary>
    private class Node
    {
        internal T Value;
        internal Node Before; // Previousから名称変更
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
