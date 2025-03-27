namespace TAmeAtCoderLibrary;

/// <summary>
/// 双方向連結リストを実装したクラスです。要素は前後の要素への参照を持ちます。
/// </summary>
/// <typeparam name="T">リストに格納する要素の型</typeparam>
public class DoublyLinkedList<T>
{
    private readonly Dictionary<T, Node> _dic = new();

    /// <summary>
    /// 双方向連結リストの新しいインスタンスを初期化します。
    /// </summary>
    public DoublyLinkedList()
    { }

    /// <summary>
    /// 指定された値の前の要素を取得します。
    /// </summary>
    /// <param name="value">検索する値</param>
    /// <param name="previous">前の要素が格納される出力パラメータ</param>
    /// <returns>前の要素が存在する場合はtrue、存在しない場合はfalse</returns>
    public bool TryGetPrevious(T value, out T previous)
    {
        if (_dic[value].Previous == null)
        {
            previous = default;
            return false;
        }
        else
        {
            previous = _dic[value].Previous.Value;
            return true;
        }
    }

    /// <summary>
    /// 指定された値の次の要素を取得します。
    /// </summary>
    /// <param name="value">検索する値</param>
    /// <param name="next">次の要素が格納される出力パラメータ</param>
    /// <returns>次の要素が存在する場合はtrue、存在しない場合はfalse</returns>
    public bool TryGetNext(T value, out T next)
    {
        if (_dic[value].Next == null)
        {
            next = default;
            return false;
        }
        else
        {
            next = _dic[value].Next.Value;
            return true;
        }
    }

    /// <summary>
    /// リストに新しい要素を追加します。既に存在する場合は追加されません。
    /// </summary>
    /// <param name="value">追加する値</param>
    public void Add(T value)
    {
        if (!_dic.ContainsKey(value))
            _dic.Add(value, new Node(value));
    }

    /// <summary>
    /// 2つの要素を連結させ、前後の関係を設定します。
    /// </summary>
    /// <param name="previous">前の要素とする値</param>
    /// <param name="next">次の要素とする値</param>
    public void Connect(T previous, T next)
    {
        Add(previous);
        Add(next);

        _dic[previous].Next = _dic[next];
        _dic[next].Previous = _dic[previous];
    }

    /// <summary>
    /// 指定された値をリストから削除します。値が存在しない場合は何も行いません。
    /// 値の前後の要素が存在する場合、それらの要素は自動的に連結されます。
    /// </summary>
    /// <param name="value">削除する値</param>
    public void Remove(T value)
    {
        if (!_dic.ContainsKey(value))
            return;

        var reConnect = _dic[value].Previous != null && _dic[value].Next != null;
        var before = _dic[value].Previous != null ? _dic[value].Previous.Value : default;
        var after = _dic[value].Next != null ? _dic[value].Next.Value : default;

        DisconnectPrevious(value);
        DisconnectNext(value);

        if (reConnect)
            AddNext(before, after);

        _dic.Remove(value);
    }

    /// <summary>
    /// 指定された要素の前に新しい要素を追加します。
    /// 既に前の要素が存在する場合、それらの関係を適切に再構築します。
    /// </summary>
    /// <param name="current">現在の要素</param>
    /// <param name="pre">追加する前の要素</param>
    public void AddPrevious(T current, T pre)
    {
        Add(current);
        Add(pre);

        var reConnect = _dic[current].Previous != null;
        var prepre = _dic[current].Previous != null ? _dic[current].Previous.Value : default;

        DisconnectPrevious(current);

        _dic[current].Previous = _dic[pre];
        _dic[current].Previous.Next = _dic[current];

        if (reConnect)
            AddPrevious(pre, prepre);
    }

    /// <summary>
    /// 指定された要素の次に新しい要素を追加します。
    /// 既に次の要素が存在する場合、それらの関係を適切に再構築します。
    /// </summary>
    /// <param name="current">現在の要素</param>
    /// <param name="next">追加する次の要素</param>
    public void AddNext(T current, T next)
    {
        Add(current);
        Add(next);

        var reConnect = _dic[current].Next != null;
        var nextnext = _dic[current].Next != null ? _dic[current].Next.Value : default;

        DisconnectNext(current);

        _dic[current].Next = _dic[next];
        _dic[current].Next.Previous = _dic[current];

        if (reConnect)
            AddNext(next, nextnext);
    }

    /// <summary>
    /// 指定された要素と前の要素との接続を切ります。
    /// </summary>
    /// <param name="current">対象の要素</param>
    public void DisconnectPrevious(T current)
    {
        if (!_dic.TryGetValue(current, out var node) || node.Previous == null) return;

        node.Previous.Next = null;
        node.Previous = null;
    }

    /// <summary>
    /// 指定された要素と次の要素との接続を切ります。
    /// </summary>
    /// <param name="current">対象の要素</param>
    public void DisconnectNext(T current)
    {
        if (!_dic.TryGetValue(current, out var node) || node.Next == null) return;

        node.Next.Previous = null;
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

        if (!_dic.TryGetValue(_dic.Keys.First(), out var node))
            return queue;

        while (node.Previous != null)
            node = node.Previous;

        do
        {
            queue.Enqueue(node.Value);
            node = node.Next;
        } while (node != null);

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

        if (!_dic.TryGetValue(current, out var node))
            return queue;

        while (node.Previous != null)
            node = node.Previous;

        do
        {
            queue.Enqueue(node.Value);
            node = node.Next;
        } while (node != null);

        return queue;
    }

    /// <summary>
    /// 双方向リンクリストのノードを表す内部クラスです。
    /// 値と前後のノードへの参照を保持します。
    /// </summary>
    private class Node
    {
        internal T Value;
        internal Node Previous;
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
