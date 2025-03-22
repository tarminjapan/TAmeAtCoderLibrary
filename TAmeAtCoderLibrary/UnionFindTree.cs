namespace TAmeAtCoderLibrary;

/// <summary>
/// Union-Find Tree (素集合データ構造)
/// </summary>
/// <typeparam name="T">要素の型。IComparableインターフェースを実装している必要があります。</typeparam>
/// <remarks>
/// 連結成分の管理、要素のグループ分けに使用します。
/// </remarks>
public class UnionFindTree<T> where T : IComparable
{
    private readonly Dictionary<T, UnionFindLeaf> Leafs;

    /// <summary>
    /// UnionFindTreeの新しいインスタンスを初期化します。
    /// </summary>
    public UnionFindTree()
    {
        Leafs = new Dictionary<T, UnionFindLeaf>();
    }

    /// <summary>
    /// 新しい根を追加します。
    /// </summary>
    /// <param name="id">根として追加する要素のID。</param>
    /// <remarks>
    /// まだ存在しないIDを持つ新しい根をUnion-Find木に追加します。
    /// </remarks>
    public void AddRoot(T id)
    {
        Leafs.Add(id, new UnionFindLeaf(id));
    }

    /// <summary>
    /// 新しい葉（子要素）を追加します。
    /// </summary>
    /// <param name="id">葉として追加する要素のID。</param>
    /// <param name="parentId">親要素のID。このIDは既にUnion-Find木に存在している必要があります。</param>
    /// <remarks>
    /// 指定された親IDを持つ新しい葉をUnion-Find木に追加します。
    /// 親IDが存在しない場合は、新しい根として親IDが追加されます。
    /// </remarks>
    public void AddLeaf(T id, T parentId)
    {
        if (!Leafs.ContainsKey(parentId)) Leafs.Add(parentId, new UnionFindLeaf(parentId));
        if (!Leafs.ContainsKey(id)) Leafs.Add(id, new UnionFindLeaf(id));
        Leafs[id].ParentID = parentId;
    }

    /// <summary>
    /// 指定された要素の根を検索します。
    /// </summary>
    /// <param name="id">根を検索する要素のID。</param>
    /// <returns>指定された要素が属する木の根のID。</returns>
    /// <remarks>
    /// 経路圧縮を用いて、検索効率を高めます。
    /// </remarks>
    public T GetRoot(T id)
    {
        if (Leafs[id].ParentID.CompareTo(Leafs[id].ID) == 0)
            return Leafs[id].ID;
        else
        {
            var rootID = GetRoot(Leafs[id].ParentID);
            Leafs[id].ParentID = rootID;

            return rootID;
        }
    }

    /// <summary>
    /// 指定された要素の親要素を取得します。
    /// </summary>
    /// <param name="id">親要素を取得する要素のID。</param>
    /// <returns>指定された要素の親要素のID。</returns>
    public T GetParent(T id) => Leafs[id].ParentID;

    /// <summary>
    /// 2つの木を結合します。
    /// </summary>
    /// <param name="id">結合する木の要素のID。</param>
    /// <param name="parentID">結合先の木の根のID。</param>
    /// <remarks>
    /// 指定された2つの要素が属する木を結合します。
    /// </remarks>
    public void Union(T id, T parentID)
    {
        var rootID1 = GetRoot(id);
        var rootID2 = GetRoot(parentID);

        if (rootID1.CompareTo(rootID2) != 0)
            Leafs[rootID1].ParentID = rootID2;
    }

    /// <summary>
    /// 要素の根を設定します。
    /// </summary>
    /// <param name="id">根を設定する要素のID。</param>
    /// <param name="rootID">設定する根のID。</param>
    /// <remarks>
    /// 指定された要素の根を指定された根IDに設定します。
    /// </remarks>
    public void SetRoot(T id, T rootID)
    {
        var tRootID = GetRoot(id);

        Leafs[tRootID].ParentID = rootID;
        Leafs[rootID].ParentID = rootID;
    }

    private class UnionFindLeaf
    {
        public T ParentID;
        public T ID;

        public UnionFindLeaf(T id)
        {
            ParentID = id;
            ID = id;
        }
    }
}