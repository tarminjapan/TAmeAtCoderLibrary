namespace TAmeAtCoderLibrary;

/// <summary>
/// 単純有向グラフ
/// </summary>
public class SimpleDirectedGraph
{
    private readonly Dictionary<int, Dictionary<int, long>> Edges = new();

    /// <summary>
    /// グラフが持つ頂点の数を取得します。
    /// </summary>
    public int NumberOfVertices => Edges.Count;

    public SimpleDirectedGraph()
    { }

    /// <summary>
    /// 辺の配列から単純有向グラフを生成します。
    /// </summary>
    /// <param name="edges">辺の配列。各辺は {from, to} または {from, to, weight} の形式です。</param>
    public SimpleDirectedGraph(int[][] edges)
    {
        AddEdges(edges);
    }

    /// <summary>
    /// 連結リストに辺を一括追加する。
    /// </summary>
    /// <param name="edges">辺の配列。各辺は {from, to} または {from, to, weight} の形式です。</param>
    public void AddEdges(int[][] edges)
    {
        foreach (var edge in edges)
            if (edge.Length == 3)
                AddEdge(edge[0], edge[1], edge[2]);
            else
                AddEdge(edge[0], edge[1]);
    }

    /// <summary>
    /// 辺を追加します。重みは1として扱われます。
    /// </summary>
    /// <param name="from">辺の始点</param>
    /// <param name="to">辺の終点</param>
    public void AddEdge(int from, int to)
    {
        AddEdge(from, to, 1L);
    }

    /// <summary>
    /// 連結リストに辺を追加する。
    /// </summary>
    /// <param name="from">辺の始点</param>
    /// <param name="to">辺の終点</param>
    /// <param name="weight">辺の重み</param>
    public void AddEdge(int from, int to, long weight)
    {
        AddVertice(from);
        AddVertice(to);

        Edges[from][to] = weight;
    }

    /// <summary>
    /// グラフに頂点を追加する。
    /// </summary>
    /// <param name="vertice">追加する頂点</param>
    public void AddVertice(int vertice)
    {
        Edges.TryAdd(vertice, new Dictionary<int, long>());
    }

    /// <summary>
    /// 辺の重みを取得します。
    /// </summary>
    /// <param name="from">辺の始点</param>
    /// <param name="to">辺の終点</param>
    /// <returns>辺の重み</returns>
    /// <exception cref="KeyNotFoundException">指定された辺が存在しない場合</exception>
    public long GetWeight(int from, int to) => Edges[from][to];

    /// <summary>
    /// 辺の重みを設定します。
    /// </summary>
    /// <param name="from">辺の始点</param>
    /// <param name="to">辺の終点</param>
    /// <param name="weight">設定する重み</param>
    /// <exception cref="KeyNotFoundException">指定された辺が存在しない場合</exception>
    public void SetWeight(int from, int to, long weight)
    {
        Edges[from][to] = weight;
    }

    /// <summary>
    /// 辺が存在するか確認する。
    /// </summary>
    /// <param name="from">辺の始点</param>
    /// <param name="to">辺の終点</param>
    /// <returns>辺が存在するかどうか</returns>
    public bool ContainsEdge(int from, int to) => Edges.TryGetValue(from, out var dic) && dic.ContainsKey(to);

    /// <summary>
    /// 頂点が存在するか確認する。
    /// </summary>
    /// <param name="vertice">確認する頂点</param>
    /// <returns>頂点が存在するかどうか</returns>
    public bool ContainsVertice(int vertice) => Edges.ContainsKey(vertice);

    /// <summary>
    /// グラフから辺を削除する。
    /// </summary>
    /// <param name="from">辺の始点</param>
    /// <param name="to">辺の終点</param>
    public void RemoveEdge(int from, int to)
    {
        if (Edges.TryGetValue(from, out var set))
            set.Remove(to);
    }

    /// <summary>
    /// グラフから頂点を削除する。
    /// </summary>
    /// <param name="vertice">削除する頂点</param>
    public void RemoveVertice(int vertice)
    {
        Edges.Remove(vertice);
    }

    /// <summary>
    /// 頂点を取得する。
    /// </summary>
    /// <returns>グラフに含まれる頂点のキュー</returns>
    public Queue<int> GetVertices()
    {
        var queue = new Queue<int>();

        foreach (var kv in Edges)
            queue.Enqueue(kv.Key);

        return queue;
    }

    /// <summary>
    /// 逆方向のSDGを取得する。
    /// </summary>
    /// <returns>逆方向のグラフ</returns>
    public SimpleDirectedGraph GenerateReversed()
    {
        var sdg = new SimpleDirectedGraph();

        foreach (var kv in Edges)
        {
            sdg.AddVertice(kv.Key);

            foreach (var to in kv.Value)
            {
                sdg.AddVertice(to.Key);
                sdg.AddEdge(to.Key, kv.Key, GetWeight(to.Key, kv.Key));
            }
        }

        return sdg;
    }

    /// <summary>
    /// 入ってくる辺の数が0個の頂点を取得する。
    /// </summary>
    /// <returns>入次数が0の頂点のHashSet</returns>
    public HashSet<int> Terms()
    {
        var reversedGraph = GenerateReversed();

        return Terms(reversedGraph);
    }

    /// <summary>
    /// 入ってくる辺の数が0個の頂点を取得する。
    /// </summary>
    /// <param name="reversedGraph">逆グラフ</param>
    /// <returns>入次数が0の頂点のHashSet</returns>
    public static HashSet<int> Terms(SimpleDirectedGraph reversedGraph)
    {
        var terms = new HashSet<int>();
        foreach (var vertice in reversedGraph.GetVertices())
            if (reversedGraph.GetVertices(vertice).Length == 0)
                terms.Add(vertice);

        return terms;
    }

    /// <summary>
    /// 有向グラフの閉路を1つ検出する。
    /// 閉路が複数個存在する場合はランダムに1つ検出する。
    /// </summary>
    /// <param name="vertice">閉路の検出を開始する頂点</param>
    /// <returns>検出された閉路の頂点のキュー。閉路が存在しない場合は空のキュー。</returns>
    public Queue<int> CreateClosedPath(int vertice)
    {
        var queue = GetClosedPath(vertice, new LinkedList<int>(), new HashSet<int>(), new HashSet<int>());

        if (1 <= queue.Count)
            return queue;

        return new Queue<int>();
    }

    /// <summary>
    /// 有向グラフの閉路を1つ検出する。
    /// 閉路が複数個存在する場合はランダムに1つ検出する。
    /// </summary>
    /// <returns>検出された閉路の頂点のキュー。閉路が存在しない場合は空のキュー。</returns>
    public Queue<int> CreateClosedPath()
    {
        var falsed = new HashSet<int>();
        foreach (var vertice in GetVertices())
        {
            var queue = GetClosedPath(vertice, new LinkedList<int>(), new HashSet<int>(), falsed);

            if (1 <= queue.Count)
                return queue;
        }

        return new Queue<int>();
    }
    private Queue<int> GetClosedPath(int current, LinkedList<int> linkedList, HashSet<int> visited, HashSet<int> falsed)
    {
        if (falsed.Contains(current))
            return new Queue<int>();

        if (visited.Contains(current))
        {
            var item = linkedList.First;
            var queue = new Queue<int>();
            var tbool = false;

            while (item != null)
            {
                if (item.Value == current)
                    tbool = true;

                if (tbool)
                    queue.Enqueue(item.Value);

                item = item.Next;
            }

            return queue;
        }

        visited.Add(current);
        linkedList.AddLast(current);

        foreach (var next in GetVertices(current))
        {
            var queue = GetClosedPath(next, linkedList, visited, falsed);

            if (1 <= queue.Count)
                return queue;
        }

        linkedList.RemoveLast();
        visited.Remove(current);
        falsed.Add(current);

        return new Queue<int>();
    }

    /// <summary>
    /// トポロジカルソートの結果を取得する。
    /// この関数はSDGがDAG（有向非巡回グラフ）である場合のみ正常に動作する。
    /// 閉路が存在する場合はQueueの要素数 < 頂点の数となる。
    /// </summary>
    /// <returns>トポロジカルソートの結果のキュー。閉路が存在する場合は不完全な結果。</returns>
    public Queue<int> TopologicalSort()
    {
        // 入ってくる辺の数が0個の頂点を探索
        var reversedGraph = GenerateReversed();
        var terms = Terms(reversedGraph);

        // トポロジカルソートの実行
        var result = new Queue<int>();
        var visited = new HashSet<int>();
        var queue = new Queue<int>();
        foreach (var term in terms)
            queue.Enqueue(term);

        while (queue.TryDequeue(out var cur))
        {
            if (visited.Contains(cur)) continue;
            visited.Add(cur);

            result.Enqueue(cur);

            foreach (var next in GetVertices(cur))
            {
                reversedGraph.RemoveEdge(next, cur);

                if (1 <= reversedGraph.GetVertices(next).Length) continue;

                queue.Enqueue(next);
            }
        }

        return result;
    }

    /// <summary>
    /// トポロジカルソートの結果を取得する。
    /// ソート結果が複数存在する場合、辞書順で最小のものを出力する。
    /// この関数はSDGがDAG（有向非巡回グラフ）である場合のみ正常に動作する。
    /// 閉路が存在する場合はQueueの要素数 < 頂点の数となる。
    /// </summary>
    /// <returns>トポロジカルソートの結果のキュー。閉路が存在する場合は不完全な結果。</returns>
    public Queue<int> TopologicalSortOrderby()
    {
        // 入ってくる辺の数が0個の頂点を探索
        var reversedGraph = GenerateReversed();
        var terms = Terms(reversedGraph);

        // トポロジカルソートの実行
        var result = new Queue<int>();
        var visited = new HashSet<int>();
        var set = new SortedSet<int>();
        foreach (var term in terms)
            set.Add(term);

        while (0 < set.Count)
        {
            var cur = set.Min;
            set.Remove(cur);

            if (visited.Contains(cur)) continue;
            visited.Add(cur);

            result.Enqueue(cur);

            foreach (var next in GetVertices(cur))
            {
                reversedGraph.RemoveEdge(next, cur);

                if (1 <= reversedGraph.GetVertices(next).Length) continue;

                set.Add(next);
            }
        }

        return result;
    }

    /// <summary>
    /// ダイクストラ法で重みの最小値を求める
    /// 但し、経路が存在しない場合は、long（64ビット整数）の最大値を返す
    /// </summary>
    /// <param name="from">開始頂点</param>
    /// <param name="to">終了頂点</param>
    /// <returns>開始頂点から終了頂点までの最短距離。経路が存在しない場合はlong.MaxValueを返す</returns>
    public long Dijkstra(int from, int to)
    {
        if (!ContainsVertice(from)) throw new KeyNotFoundException("開始頂点が存在しません。");
        if (!ContainsVertice(to)) throw new KeyNotFoundException("終了頂点が存在しません。");

        var visited = new HashSet<int>();
        var costs = new Dictionary<int, long>();
        var pqueue = new PriorityQueue<int, long>();
        pqueue.Enqueue(from, 0L);

        while (pqueue.TryDequeue(out var current, out var ccost))
        {
            if (visited.Contains(current) || current == to)
                continue;

            visited.Add(current);

            foreach (var next in GetVertices(current))
            {
                var ncost = ccost + GetWeight(current, next);

                if (costs.TryGetValue(next, out var tcost) && tcost <= ncost)
                    continue;

                costs[next] = ncost;

                pqueue.Enqueue(next, ncost);
            }
        }

        return costs.TryGetValue(to, out var result) ? result : long.MaxValue;
    }

    /// <summary>
    /// 隣接する頂点を取得する。
    /// </summary>
    /// <param name="a">隣接する頂点を知りたい頂点</param>
    /// <returns>頂点aから出ている辺の終点の配列。辺が存在しない場合は空の配列。</returns>
    public int[] GetVertices(int a) => Edges.TryGetValue(a, out var value) ? value.Keys.ToArray() : Array.Empty<int>();
}
