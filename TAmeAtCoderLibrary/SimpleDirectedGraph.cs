#nullable enable
using System.Collections.ObjectModel;

namespace TAmeAtCoderLibrary;

/// <summary>
/// 重み付き単純有向グラフを表します。
/// 頂点はint型、辺の重みはlong型で表現されます。
/// </summary>
public class SimpleDirectedGraph
{
    // 隣接リスト表現: {頂点 -> {隣接頂点 -> 重み}}
    private readonly Dictionary<int, Dictionary<int, long>> _adjacencyList = new();
    // 各頂点の入次数を保持する辞書
    private readonly Dictionary<int, int> _inDegrees = new();

    /// <summary>
    /// グラフが持つ頂点の数を取得します。
    /// 計算量: O(1)
    /// </summary>
    public int VertexCount => _adjacencyList.Count;

    /// <summary>
    /// グラフが持つ辺の数を取得します。
    /// 計算量: O(1) (内部でカウントを保持)
    /// </summary>
    public int EdgeCount { get; private set; } = 0;

    /// <summary>
    /// 新しい空の単純有向グラフを初期化します。
    /// </summary>
    public SimpleDirectedGraph() { }

    /// <summary>
    /// 辺の配列から単純有向グラフを生成します。
    /// </summary>
    /// <param name="edges">辺の配列。各要素は {from, to} (重み1) または {from, to, weight} の形式です。</param>
    /// <exception cref="ArgumentNullException">edgesがnullの場合。</exception>
    /// <exception cref="ArgumentException">辺の配列の形式が不正な場合にスローされます。</exception>
    public SimpleDirectedGraph(int[][] edges)
    {
        AddEdges(edges);
    }

    /// <summary>
    /// グラフに頂点を追加します。既に存在する場合は何もしません。
    /// </summary>
    /// <param name="vertex">追加する頂点。</param>
    /// <returns>頂点が新しく追加された場合は true、既に存在した場合は false。</returns>
    public bool AddVertex(int vertex)
    {
        if (_adjacencyList.TryAdd(vertex, new Dictionary<int, long>()))
        {
            _inDegrees.TryAdd(vertex, 0); // 新しい頂点の入次数は0
            return true;
        }
        return false;
    }

    /// <summary>
    /// グラフに複数の辺を追加します。
    /// </summary>
    /// <param name="edges">辺の配列。各要素は {from, to} (重み1) または {from, to, weight} の形式です。</param>
    /// <exception cref="ArgumentNullException">edgesがnullの場合。</exception>
    /// <exception cref="ArgumentException">辺の配列の形式が不正な場合にスローされます。</exception>
    public void AddEdges(int[][] edges)
    {
        if (edges == null) throw new ArgumentNullException(nameof(edges));

        foreach (var edge in edges)
        {
            if (edge == null || edge.Length < 2 || edge.Length > 3)
            {
                throw new ArgumentException("各辺の配列は {from, to} または {from, to, weight} の形式である必要があります。", nameof(edges));
            }

            int from = edge[0];
            int to = edge[1];
            long weight = (edge.Length == 3) ? edge[2] : 1L;
            AddEdge(from, to, weight);
        }
    }

    /// <summary>
    /// グラフに重み付きの辺を追加します。
    /// 頂点が存在しない場合は自動的に追加されます。
    /// 既に辺が存在する場合は重みが上書きされます。
    /// </summary>
    /// <param name="from">辺の始点。</param>
    /// <param name="to">辺の終点。</param>
    /// <param name="weight">辺の重み。</param>
    public void AddEdge(int from, int to, long weight)
    {
        // 頂点が存在しない場合は追加
        AddVertex(from);
        AddVertex(to);

        // 辺を追加または更新
        bool edgeExisted = _adjacencyList[from].ContainsKey(to);
        _adjacencyList[from][to] = weight;

        // 辺が新しく追加された場合のみ入次数と辺数を更新
        if (!edgeExisted)
        {
            _inDegrees[to] = _inDegrees.GetValueOrDefault(to, 0) + 1;
            EdgeCount++;
        }
    }

    /// <summary>
    /// 辺の重みを取得します。
    /// </summary>
    /// <param name="from">辺の始点。</param>
    /// <param name="to">辺の終点。</param>
    /// <returns>辺の重み。</returns>
    /// <exception cref="KeyNotFoundException">指定された辺が存在しない場合にスローされます。</exception>
    public long GetWeight(int from, int to)
    {
        if (_adjacencyList.TryGetValue(from, out var neighbors) && neighbors.TryGetValue(to, out var weight))
        {
            return weight;
        }
        throw new KeyNotFoundException($"辺 ({from} -> {to}) は存在しません。");
    }

    /// <summary>
    /// 辺の重みを安全に取得します。
    /// </summary>
    /// <param name="from">辺の始点。</param>
    /// <param name="to">辺の終点。</param>
    /// <param name="weight">辺が存在する場合、その重みが設定されます。</param>
    /// <returns>辺が存在する場合は true、存在しない場合は false。</returns>
    public bool TryGetWeight(int from, int to, out long weight)
    {
        weight = default;
        return _adjacencyList.TryGetValue(from, out var neighbors) && neighbors.TryGetValue(to, out weight);
    }


    /// <summary>
    /// 辺が存在するかどうかを確認します。
    /// </summary>
    /// <param name="from">辺の始点。</param>
    /// <param name="to">辺の終点。</param>
    /// <returns>辺が存在する場合は true、存在しない場合は false。</returns>
    public bool ContainsEdge(int from, int to)
    {
        return _adjacencyList.TryGetValue(from, out var neighbors) && neighbors.ContainsKey(to);
    }

    /// <summary>
    /// 頂点が存在するかどうかを確認します。
    /// </summary>
    /// <param name="vertex">確認する頂点。</param>
    /// <returns>頂点が存在する場合は true、存在しない場合は false。</returns>
    public bool ContainsVertex(int vertex)
    {
        return _adjacencyList.ContainsKey(vertex);
    }

    /// <summary>
    /// グラフから辺を削除します。
    /// </summary>
    /// <param name="from">辺の始点。</param>
    /// <param name="to">辺の終点。</param>
    /// <returns>辺が削除された場合は true、辺が存在しなかった場合は false。</returns>
    public bool RemoveEdge(int from, int to)
    {
        if (_adjacencyList.TryGetValue(from, out var neighbors))
        {
            if (neighbors.Remove(to))
            {
                // 辺が存在した場合のみ入次数と辺数を更新
                _inDegrees[to]--; // 存在しないキーアクセスは通常エラーだが、辺があればtoも存在するはず
                EdgeCount--;
                return true;
            }
        }
        return false;
    }

    // Note: 頂点の削除は実装が複雑になるため除外 (必要なら追加実装)
    // public bool RemoveVertex(int vertex) { ... }

    /// <summary>
    /// グラフに含まれるすべての頂点を取得します。
    /// 返されるコレクションは読み取り専用です。
    /// </summary>
    /// <returns>グラフの頂点の読み取り専用コレクション。</returns>
    public IReadOnlyCollection<int> GetAllVertices()
    {
        // Keys は直接変更できないため、そのまま返しても安全。
        return _adjacencyList.Keys;
    }

    /// <summary>
    /// 指定された頂点から出ていく辺の終点（隣接頂点）を取得します。
    /// 返されるコレクションは読み取り専用です。
    /// </summary>
    /// <param name="sourceVertex">始点となる頂点。</param>
    /// <returns>隣接する頂点の読み取り専用コレクション。頂点が存在しない場合は空のコレクション。</returns>
    public IReadOnlyCollection<int> GetAdjacentVertices(int sourceVertex)
    {
        if (_adjacencyList.TryGetValue(sourceVertex, out var neighbors))
        {
            // Keys は直接変更できないため、そのまま返しても安全。
            return neighbors.Keys;
        }
        // .NET Core 2.0以降では Array.Empty<T>() が推奨されることが多い
        return Array.Empty<int>();
    }

    /// <summary>
    /// 指定された頂点から出ていく辺（隣接頂点とその重み）を取得します。
    /// 返される辞書は読み取り専用です。
    /// </summary>
    /// <param name="sourceVertex">始点となる頂点。</param>
    /// <returns>隣接する頂点と重みのペアの読み取り専用辞書。頂点が存在しない場合は空の辞書。</returns>
    public IReadOnlyDictionary<int, long> GetOutgoingEdges(int sourceVertex)
    {
        if (_adjacencyList.TryGetValue(sourceVertex, out var neighbors))
        {
            // Dictionaryをそのまま返すと外部で変更できてしまうため、ReadOnlyDictionaryでラップ
            return new ReadOnlyDictionary<int, long>(neighbors);
        }
        // 空のReadOnlyDictionaryを返す
        return new ReadOnlyDictionary<int, long>(new Dictionary<int, long>());
    }

    /// <summary>
    /// 指定された頂点の入次数（その頂点に入る辺の数）を取得します。
    /// </summary>
    /// <param name="vertex">入次数を調べる頂点。</param>
    /// <returns>頂点の入次数。頂点が存在しない場合は 0。</returns>
    public int GetInDegree(int vertex)
    {
        // 存在しないキーでも 0 を返す
        return _inDegrees.GetValueOrDefault(vertex, 0);
    }

    /// <summary>
    /// グラフの向きを逆にした新しいグラフを生成します。
    /// </summary>
    /// <returns>辺の向きが逆転した新しい SimpleDirectedGraph インスタンス。</returns>
    public SimpleDirectedGraph GenerateReversed()
    {
        var reversedGraph = new SimpleDirectedGraph();
        foreach (var vertex in GetAllVertices())
        {
            reversedGraph.AddVertex(vertex); // 全ての頂点を追加
        }

        foreach (var kvp in _adjacencyList) // KeyValuePair<int, Dictionary<int, long>>
        {
            int fromVertex = kvp.Key;
            foreach (var edge in kvp.Value) // KeyValuePair<int, long>
            {
                int toVertex = edge.Key;
                long weight = edge.Value;
                // 辺の向きを逆にして追加
                reversedGraph.AddEdge(toVertex, fromVertex, weight);
            }
        }
        return reversedGraph;
    }

    // --- 閉路検出 (Cycle Detection) ---

    private enum VisitState { Unvisited, Visiting, Visited }

    /// <summary>
    /// グラフ内に閉路（サイクル）が存在するかどうかを検出します。
    /// </summary>
    /// <returns>閉路が存在する場合は true、存在しない場合は false。</returns>
    public bool HasCycle()
    {
        return TryFindCycle(out _);
    }

    /// <summary>
    /// グラフ内の閉路を1つ検出し、その経路上の頂点を返します。
    /// 閉路が複数存在する場合、どの閉路が検出されるかは不定です。
    /// </summary>
    /// <param name="cyclePath">検出された閉路の頂点のリスト（順序付き）。閉路が存在しない場合は null。</param>
    /// <returns>閉路が検出された場合は true、存在しない場合は false。</returns>
    public bool TryFindCycle(out List<int>? cyclePath) // CS8632 修正: #nullable enable があればOK
    {
        var visitState = new Dictionary<int, VisitState>();
        var recursionStack = new Stack<int>(); // 現在の探索パス

        foreach (int vertex in GetAllVertices())
        {
            // まだ訪問状態が記録されていない頂点から探索開始
            if (!visitState.ContainsKey(vertex)) // Unvisited と同じ扱い
            {
                if (TryFindCycleDfs(vertex, visitState, recursionStack, out cyclePath))
                {
                    return true;
                }
            }
        }

        cyclePath = null;
        return false;
    }

    // 閉路検出のDFSヘルパー
    private bool TryFindCycleDfs(int currentVertex, Dictionary<int, VisitState> visitState, Stack<int> recursionStack, out List<int>? cyclePath) // CS8632 修正
    {
        visitState[currentVertex] = VisitState.Visiting;
        recursionStack.Push(currentVertex);

        // 隣接頂点をループで処理する前に、頂点が存在するか確認（GetAllVerticesで取得した後、辺が削除された場合などへの対策）
        if (_adjacencyList.TryGetValue(currentVertex, out var neighbors))
        {
            foreach (var neighbor in neighbors.Keys) // GetAdjacentVertices(currentVertex) の中身を展開
            {
                VisitState neighborState = visitState.GetValueOrDefault(neighbor, VisitState.Unvisited);

                if (neighborState == VisitState.Unvisited)
                {
                    if (TryFindCycleDfs(neighbor, visitState, recursionStack, out cyclePath))
                    {
                        return true; // 再帰呼び出しで閉路が見つかった
                    }
                }
                else if (neighborState == VisitState.Visiting)
                {
                    // 訪問中の頂点に戻ってきた -> 閉路発見
                    cyclePath = new List<int>();
                    // スタックから閉路部分を抽出
                    foreach (int node in recursionStack.Reverse()) // StackはLIFOなのでReverseが必要
                    {
                        cyclePath.Add(node);
                        if (node == neighbor) break; // 閉路の開始点まで追加したら終了
                    }
                    cyclePath.Reverse(); // 始点から終点の順序にする
                    return true;
                }
                // neighborState == VisitState.Visited の場合はスキップ
            }
        }

        // この頂点からの探索が完了（閉路は見つからなかった）
        visitState[currentVertex] = VisitState.Visited;
        recursionStack.Pop();
        cyclePath = null;
        return false;
    }


    // --- トポロジカルソート (Topological Sort) ---

    /// <summary>
    /// グラフをトポロジカルソートします（Kahn's algorithm）。
    /// グラフに閉路が存在する場合、ソート結果は不完全（全頂点を含まない）になります。
    /// </summary>
    /// <param name="sortedVertices">トポロジカルソートされた頂点のリスト。閉路が存在する場合は null。</param>
    /// <returns>トポロジカルソートが成功した（閉路がなかった）場合は true、閉路が存在した場合は false。</returns>
    public bool TryTopologicalSort(out List<int>? sortedVertices) // CS8632 修正
    {
        return TryTopologicalSortInternal(false, out sortedVertices);
    }

    /// <summary>
    /// グラフを辞書順で最小となるようにトポロジカルソートします。
    /// グラフに閉路が存在する場合、ソート結果は不完全（全頂点を含まない）になります。
    /// </summary>
    /// <param name="sortedVertices">辞書順最小でトポロジカルソートされた頂点のリスト。閉路が存在する場合は null。</param>
    /// <returns>トポロジカルソートが成功した（閉路がなかった）場合は true、閉路が存在した場合は false。</returns>
    public bool TryTopologicalSortLexicographical(out List<int>? sortedVertices) // CS8632 修正
    {
        return TryTopologicalSortInternal(true, out sortedVertices);
    }

    /// <summary>
    /// トポロジカルソートの内部実装。
    /// </summary>
    private bool TryTopologicalSortInternal(bool lexicographical, out List<int>? sortedVertices) // CS8632 修正
    {
        sortedVertices = new List<int>();
        var currentInDegrees = new Dictionary<int, int>(_inDegrees); // 入次数のコピーを作成して操作
        object zeroInDegreeQueue = InitializeZeroInDegreeQueue(lexicographical, currentInDegrees); // 型はobject

        // CS0019 修正: 型に応じてCountプロパティにアクセスし、ループ条件とする
        while (GetQueueCount(zeroInDegreeQueue, lexicographical) > 0)
        {
            int currentVertex = DequeueFrom(zeroInDegreeQueue, lexicographical);
            sortedVertices.Add(currentVertex);

            // 隣接頂点の入次数を減らす
            if (_adjacencyList.TryGetValue(currentVertex, out var neighbors))
            {
                foreach (var neighbor in neighbors.Keys)
                {
                    // currentInDegreesにneighborが存在しないケースは基本的にないはずだが念のため
                    if (currentInDegrees.ContainsKey(neighbor))
                    {
                        currentInDegrees[neighbor]--;
                        if (currentInDegrees[neighbor] == 0)
                        {
                            EnqueueTo(zeroInDegreeQueue, neighbor, lexicographical);
                        }
                    }
                }
            }
        }

        // 全ての頂点がソートリストに含まれていれば成功（閉路なし）
        if (sortedVertices.Count == VertexCount)
        {
            return true;
        }
        else
        {
            // 閉路が存在するため、ソートは不完全
            sortedVertices = null; // Null許容型なので代入可能
            return false;
        }
    }

    // キュー/優先度付きキューの要素数を取得するヘルパー (CS0019 対策)
    private int GetQueueCount(object queue, bool lexicographical)
    {
        if (lexicographical)
        {
            // PriorityQueue<TElement, TPriority> にキャスト
            return ((PriorityQueue<int, int>)queue).Count;
        }
        else
        {
            // Queue<T> にキャスト
            return ((Queue<int>)queue).Count;
        }
    }


    // トポロジカルソート用のキュー/優先度付きキュー初期化ヘルパー
    private object InitializeZeroInDegreeQueue(bool lexicographical, Dictionary<int, int> currentInDegrees)
    {
        var sources = GetAllVertices().Where(v => currentInDegrees.GetValueOrDefault(v, 0) == 0);
        if (lexicographical)
        {
            // 辞書順の場合はPriorityQueue (Min-Heapとして使用)
            var pq = new PriorityQueue<int, int>(); // C# 6.0 以降
            foreach (var vertex in sources.OrderBy(v => v)) // 念のためOrderByを追加
            {
                pq.Enqueue(vertex, vertex); // 優先度も頂点IDそのもの
            }
            return pq;
        }
        else
        {
            // 通常はQueueで良い
            return new Queue<int>(sources); // QueueのコンストラクタはIEnumerableを受け取れる
        }
    }

    // トポロジカルソート用のキュー/優先度付きキューへの追加ヘルパー
    private void EnqueueTo(object queue, int vertex, bool lexicographical)
    {
        if (lexicographical)
        {
            ((PriorityQueue<int, int>)queue).Enqueue(vertex, vertex);
        }
        else
        {
            ((Queue<int>)queue).Enqueue(vertex);
        }
    }

    // トポロジカルソート用のキュー/優先度付きキューからの取り出しヘルパー
    private int DequeueFrom(object queue, bool lexicographical)
    {
        if (lexicographical)
        {
            return ((PriorityQueue<int, int>)queue).Dequeue();
        }
        else
        {
            return ((Queue<int>)queue).Dequeue();
        }
    }


    // --- ダイクストラ法 (Dijkstra's Algorithm) ---

    /// <summary>
    /// ダイクストラ法を用いて、指定された始点から他の全ての頂点への最短経路コストを計算します。
    /// 負の重みを持つ辺が存在する場合、正しい結果が得られない可能性があります。
    /// </summary>
    /// <param name="startVertex">始点となる頂点。</param>
    /// <returns>
    /// 各頂点への最短経路コストを格納した辞書。
    /// 始点から到達不可能な頂点は含まれません。
    /// 始点自身のコストは 0 です。
    /// </returns>
    /// <exception cref="KeyNotFoundException">始点が存在しない場合にスローされます。</exception>
    public Dictionary<int, long> Dijkstra(int startVertex)
    {
        if (!ContainsVertex(startVertex))
        {
            throw new KeyNotFoundException($"始点 {startVertex} はグラフに存在しません。");
        }

        var costs = new Dictionary<int, long>(); // 始点からの最短コスト
                                                 // 優先度付きキュー: {頂点, 始点からの暫定コスト} コストが小さい順に取り出す
        var priorityQueue = new PriorityQueue<int, long>(); // C# 6.0 以降

        // 初期化
        costs[startVertex] = 0L;
        priorityQueue.Enqueue(startVertex, 0L);

        while (priorityQueue.TryDequeue(out int currentVertex, out long currentCost))
        {
            // キューから取り出したコストが、記録されている最短コストより大きい場合はスキップ
            if (costs.TryGetValue(currentVertex, out long recordedCost) && currentCost > recordedCost)
            {
                continue;
            }

            // 隣接頂点のコストを更新 (GetOutgoingEdgesを使用)
            foreach (var edge in GetOutgoingEdges(currentVertex))
            {
                int neighbor = edge.Key;
                long weight = edge.Value;

                // 負の重みチェック (任意)
                // if (weight < 0) throw new InvalidOperationException("Dijkstra cannot handle negative weights.");

                long newCost = currentCost + weight;

                // オーバーフローチェック (任意、非常に大きなコストの場合)
                // if (newCost < currentCost) { /* Handle overflow, e.g., treat as infinity */ newCost = long.MaxValue; }


                // 既に記録されているコストより小さい場合、またはまだコストが記録されていない場合
                if (!costs.TryGetValue(neighbor, out long neighborCost) || newCost < neighborCost)
                {
                    costs[neighbor] = newCost;
                    priorityQueue.Enqueue(neighbor, newCost);
                }
            }
        }

        return costs;
    }

    /// <summary>
    /// ダイクストラ法を用いて、始点から終点への最短経路コストを計算します。
    /// 負の重みを持つ辺が存在する場合、正しい結果が得られない可能性があります。
    /// </summary>
    /// <param name="startVertex">始点となる頂点。</param>
    /// <param name="endVertex">終点となる頂点。</param>
    /// <returns>始点から終点への最短経路コスト。到達不可能な場合は long.MaxValue。</returns>
    /// <exception cref="KeyNotFoundException">始点または終点が存在しない場合にスローされます。</exception>
    public long Dijkstra(int startVertex, int endVertex)
    {
        if (!ContainsVertex(startVertex))
        {
            throw new KeyNotFoundException($"始点 {startVertex} はグラフに存在しません。");
        }
        if (!ContainsVertex(endVertex))
        {
            throw new KeyNotFoundException($"終点 {endVertex} はグラフに存在しません。");
        }

        // 最適化: 終点に到達した時点で探索を打ち切る実装も可能。
        // ここでは、全頂点へのコスト計算後に結果を取得する。
        var allCosts = Dijkstra(startVertex);

        if (allCosts.TryGetValue(endVertex, out long cost))
        {
            return cost;
        }
        else
        {
            // 終点に到達できなかった場合
            return long.MaxValue;
        }
    }
}