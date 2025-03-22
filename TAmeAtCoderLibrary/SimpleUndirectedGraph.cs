namespace TAmeAtCoderLibrary;

/// <summary>
/// 単純無向グラフを表現するクラス。
/// 頂点と辺を持ち、辺には重みを設定できます。
/// </summary>
public class SimpleUndirectedGraph
{
    /// <summary>
    /// グラフの辺を格納する辞書。
    /// キーは頂点ID、値はその頂点に隣接する頂点とその辺の重みを表す辞書。
    /// </summary>
    private readonly Dictionary<int, Dictionary<int, long>> _Edges = new();

    /// <summary>
    /// 空の単純無向グラフを初期化します。
    /// </summary>
    public SimpleUndirectedGraph()
    { }

    /// <summary>
    /// 最大頂点数と辺のリストを指定して、単純無向グラフを初期化します。
    /// </summary>
    /// <param name="maxVertice">グラフに含める頂点の最大数。1から始まるIDを想定。</param>
    /// <param name="edges">辺の情報を表す二次元配列。各要素は {頂点A, 頂点B} の形式。</param>
    public SimpleUndirectedGraph(int maxVertice, int[][] edges)
    {
        // 指定された最大頂点数まで頂点を追加
        for (int n = 1; n <= maxVertice; n++)
            AddVertice(n);

        // 辺を追加
        AddEdges(edges);
    }

    /// <summary>
    /// 辺のリストを指定して、単純無向グラフを初期化します。
    /// 辺の情報から必要な頂点が自動的に作成されます。
    /// </summary>
    /// <param name="edges">辺の情報を表す二次元配列。各要素は {頂点A, 頂点B} の形式。</param>
    public SimpleUndirectedGraph(int[][] edges)
    {
        AddEdges(edges);
    }

    /// <summary>
    /// グラフに頂点を追加します。
    /// </summary>
    /// <param name="vertice">追加する頂点のID。グラフ内にまだ存在しないIDを指定してください。</param>
    /// <exception cref="ArgumentException">既に同じIDの頂点が存在する場合にスローされます。</exception>
    public void AddVertice(int vertice)
    {
        if (_Edges.ContainsKey(vertice))
            throw new ArgumentException($"頂点 {vertice} は既に存在します。");
        _Edges.Add(vertice, new Dictionary<int, long>());
    }

    /// <summary>
    /// グラフに辺を一括で追加します。
    /// </summary>
    /// <param name="edges">辺の情報の配列。各要素は {頂点A, 頂点B} の形式の配列です。</param>
    /// <exception cref="ArgumentException">辺の要素数が2でない場合、または頂点IDが負の値である場合にスローされます。</exception>
    public void AddEdges(int[][] edges)
    {
        foreach (var edge in edges)
            AddEdge(edge[0], edge[1]);
    }

    /// <summary>
    /// グラフに辺を追加します。辺の重みは0として扱われます。
    /// </summary>
    /// <param name="verticeA">辺の一方の頂点のID。</param>
    /// <param name="verticeB">辺のもう一方の頂点のID。</param>
    /// <exception cref="ArgumentException">頂点IDが負の値である場合にスローされます。</exception>
    public void AddEdge(int verticeA, int verticeB)
    {
        AddEdge(verticeA, verticeB, 0L);
    }

    /// <summary>
    /// グラフに重み付きの辺を追加します。
    /// </summary>
    /// <param name="verticeA">辺の一方の頂点のID。</param>
    /// <param name="verticeB">辺のもう一方の頂点のID。</param>
    /// <param name="weight">辺の重み。</param>
    /// <exception cref="ArgumentException">頂点IDが負の値である場合にスローされます。</exception>
    public void AddEdge(int verticeA, int verticeB, long weight)
    {
        if (verticeA < 0 || verticeB < 0) throw new ArgumentException("頂点IDは0以上の値を指定してください");
        AddVerticeIfNotExists(verticeA);
        AddVerticeIfNotExists(verticeB);
        _Edges[verticeA].TryAdd(verticeB, weight);
        _Edges[verticeB].TryAdd(verticeA, weight);
    }

    /// <summary>
    /// グラフに重み付きの辺を追加します。既に辺が存在する場合は重みを更新します。
    /// </summary>
    /// <param name="verticeA">辺の一方の頂点のID。</param>
    /// <param name="verticeB">辺のもう一方の頂点のID。</param>
    /// <param name="weight">辺の重み。</param>
    /// <exception cref="ArgumentException">頂点IDが負の値である場合にスローされます。</exception>
    public void AddAndSetWeight(int verticeA, int verticeB, long weight)
    {
        AddEdge(verticeA, verticeB, weight);
        _Edges[verticeA][verticeB] = weight;
        _Edges[verticeB][verticeA] = weight;
    }

    /// <summary>
    /// グラフから頂点を削除します。
    /// </summary>
    /// <param name="vertice">削除する頂点のID。</param>
    /// <remarks>頂点に関連するすべての辺も削除されます。</remarks>
    /// <exception cref="KeyNotFoundException">指定された頂点が存在しない場合にスローされます。</exception>
    public void RemoveVertice(int vertice)
    {
        if (!_Edges.ContainsKey(vertice)) throw new KeyNotFoundException($"頂点{vertice}は存在しません");
        foreach (var next in GetVertices(vertice))
            RemoveEdge(vertice, next);

        _Edges.Remove(vertice);
    }

    /// <summary>
    /// グラフから辺を削除します。
    /// </summary>
    /// <param name="verticeA">辺の一方の頂点のID。</param>
    /// <param name="verticeB">辺のもう一方の頂点のID。</param>
    /// <exception cref="KeyNotFoundException">指定された頂点が存在しない場合にスローされます。</exception>
    public void RemoveEdge(int verticeA, int verticeB)
    {
        if (!_Edges.ContainsKey(verticeA) || !_Edges.ContainsKey(verticeB)) throw new KeyNotFoundException("頂点が存在しません");
        _Edges[verticeA].Remove(verticeB);
        _Edges[verticeB].Remove(verticeA);
    }

    public bool ContainsVertice(int vertice) => _Edges.ContainsKey(vertice);

    /// <summary>
    /// 指定された頂点に隣接する頂点のIDを配列で取得します。
    /// </summary>
    /// <param name="vertice">隣接頂点を調べたい頂点のID。</param>
    /// <returns>指定された頂点に隣接する頂点のIDの配列。隣接頂点が存在しない場合は空の配列を返します。</returns>
    /// <exception cref="KeyNotFoundException">指定された頂点が存在しない場合にスローされます。</exception>
    public Dictionary<int, long>.KeyCollection GetVertices(int vertice)
    {
        if (!_Edges.ContainsKey(vertice)) throw new KeyNotFoundException($"頂点{vertice}は存在しません");
        return _Edges[vertice].Keys;
    }

    /// <summary>
    /// 指定した辺の重みを取得します。
    /// </summary>
    /// <param name="verticeA">辺の一方の頂点のID。</param>
    /// <param name="verticeB">辺のもう一方の頂点のID。</param>
    /// <returns>頂点Aと頂点Bを結ぶ辺の重み。</returns>
    /// <exception cref="KeyNotFoundException">指定された頂点または辺が存在しない場合にスローされます。</exception>
    public long GetWeight(int verticeA, int verticeB)
    {
        if (!_Edges.ContainsKey(verticeA) || !_Edges.ContainsKey(verticeB)) throw new KeyNotFoundException("頂点が存在しません");
        if (!_Edges[verticeA].ContainsKey(verticeB)) throw new KeyNotFoundException("指定された辺が存在しません");
        return _Edges[verticeA][verticeB];
    }

    /// <summary>
    /// 次数が0または1（末端にある頂点=葉）の頂点を取得します。
    /// </summary>
    /// <returns>葉（次数が0または1の頂点）のIDのHashSet。</returns>
    /// <remarks>グラフが複数の連結成分を持つ場合、すべての連結成分の葉を返します。グラフに辺が全くない(頂点のみ)の場合は、全頂点が葉となります。</remarks>
    public HashSet<int> GetLeaves()
    {
        var leaves = new HashSet<int>();

        foreach (var kv in _Edges)
            if (kv.Value.Count <= 1)
                leaves.Add(kv.Key);

        return leaves;
    }

    /// <summary>
    /// グラフに閉路が存在するかどうかをチェックします。
    /// </summary>
    /// <returns>閉路が存在する場合はtrue、存在しない場合はfalseを返します。</returns>
    public bool ContainsLoop()
    {
        var keys = _Edges.Keys.ToArray();
        var set = new HashSet<int>();

        foreach (var s in keys)
        {
            if (set.Contains(s))
                continue;

            var queue = new Queue<(int Before, int Current)>();
            queue.Enqueue((-1, s));

            while (queue.TryDequeue(out var item))
            {
                var b = item.Before;
                var c = item.Current;

                if (set.Contains(c))
                    return true;

                set.Add(c);

                foreach (var n in GetVertices(c))
                    if (b != n)
                        queue.Enqueue((c, n));
            }
        }

        return false;
    }

    /// <summary>
    /// 頂点がまだグラフに存在しない場合のみ、頂点を追加する。
    /// </summary>
    /// <param name="vertice">確認する頂点のID</param>
    private void AddVerticeIfNotExists(int vertice)
    {
        if (!_Edges.ContainsKey(vertice))
        {
            _Edges.Add(vertice, new Dictionary<int, long>());
        }
    }
}
