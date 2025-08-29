namespace TAmeAtCoderLibrary;

/// <summary>
/// デカルトの木を表します。
/// （解説）https://gemini.google.com/share/fae3dcb949ad
/// </summary>
/// <typeparam name="TKey">キーの型。比較可能である必要があります。</typeparam>
/// <typeparam name="TValue">値（優先度）の型。比較可能である必要があります。</typeparam>
public class CartesianTree<TKey, TValue> where TKey : IComparable<TKey> where TValue : IComparable<TValue>
{
    /// <summary>
    /// デカルトの木のノードを表します。
    /// </summary>
    public class Node
    {
        /// <summary>
        /// ノードのキーを取得します。
        /// </summary>
        public TKey Key { get; }

        /// <summary>
        /// ノードの値（優先度）を取得します。
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// ノードの親を取得または設定します。
        /// </summary>
        public Node Parent { get; internal set; }

        /// <summary>
        /// ノードの左の子を取得または設定します。
        /// </summary>
        public Node Left { get; internal set; }

        /// <summary>
        /// ノードの右の子を取得または設定します。
        /// </summary>
        public Node Right { get; internal set; }

        /// <summary>
        /// <see cref="Node"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="key">ノードのキー。</param>
        /// <param name="value">ノードの値（優先度）。</param>
        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    /// <summary>
    /// デカルトの木の根を取得します。
    /// </summary>
    public Node Root { get; private set; }

    /// <summary>
    /// キーと値のペアのコレクションから木を構築することにより、<see cref="CartesianTree{TKey, TValue}"/> クラスの新しいインスタンスを初期化します。
    /// 構築は、ソートのために O(n log n) で行われますが、入力がキーで事前にソートされている場合は O(n) です。
    /// </summary>
    /// <param name="items">木を構築するためのキーと値のペアのコレクション。</param>
    public CartesianTree(IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
        // 木を効率的に構築するために、項目をキーでソートします。
        var sortedItems = items.OrderBy(kvp => kvp.Key).ToArray();
        if (sortedItems.Length == 0)
        {
            return;
        }

        // 最初の項目がルートになります。
        Root = new Node(sortedItems[0].Key, sortedItems[0].Value);
        Node lastNode = Root;

        // 残りの項目を処理します。
        for (int i = 1; i < sortedItems.Length; i++)
        {
            var (key, value) = (sortedItems[i].Key, sortedItems[i].Value);
            var node = new Node(key, value);

            // 新しいノードの正しい位置を見つけるために、木の右側の幹を上にたどります。
            // 新しいノードの値よりも値（優先度）が小さいノードを探しています。
            while (lastNode.Parent != null && lastNode.Value.CompareTo(value) > 0)
            {
                lastNode = lastNode.Parent;
            }

            // 新しいノードが現在の lastNode よりも高い優先度（小さい値）を持つ場合、
            // それは、私たちがたどってきた部分木の新しいルートになります。
            if (lastNode.Value.CompareTo(value) > 0)
            {
                node.Left = lastNode;
                lastNode.Parent = node;
                Root = node;
            }
            else
            {
                // それ以外の場合、新しいノードは lastNode の右の子になります。
                // lastNode の以前の右の子は、新しいノードの左の子になります。
                node.Left = lastNode.Right;
                if (lastNode.Right != null)
                {
                    lastNode.Right.Parent = node;
                }
                lastNode.Right = node;
                node.Parent = lastNode;
            }
            lastNode = node;
        }
    }
}
