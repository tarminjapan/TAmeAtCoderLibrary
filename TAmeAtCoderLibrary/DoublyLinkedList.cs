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
