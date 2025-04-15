#nullable enable
using System.Collections.ObjectModel;

namespace TAmeAtCoderLibrary;
/// <summary>
/// 頂点が非負整数で識別され、辺が長整数の重みを持つことができる単純有向グラフを表します。
/// このグラフは自己ループを許可しません。
/// 重みが指定されない場合、デフォルトで 1 が設定されます（TryAddEdge使用時）。
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
    /// 各辺はデフォルトの重み 1 で追加され、既に存在する辺は無視されます。自己ループも無視されます。
    /// </summary>
    /// <param name="edges">辺の配列。各要素は {from, to} または {from, to, weight} の形式です。weightが指定された場合はその値が使われます。</param>
    /// <exception cref="ArgumentNullException"><paramref name="edges"/>がnullの場合。</exception>
    /// <exception cref="ArgumentException"><paramref name="edges"/>内の配列形式が不正な場合。</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="edges"/>内に負の頂点IDが含まれる場合。</exception>
    public SimpleDirectedGraph(int[][] edges)
    {
        AddEdges(edges);
    }

    /// <summary>
    /// グラフに頂点を追加します。既に存在する場合は何もしません。
    /// </summary>
    /// <param name="vertex">追加する頂点のID。非負である必要があります。</param>
    /// <returns>頂点が新しく追加された場合は true、既に存在した場合は false。</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertex"/>が負の場合。</exception>
    public bool AddVertex(int vertex)
    {
        if (vertex < 0)
            throw new ArgumentOutOfRangeException(nameof(vertex), "Vertex ID cannot be negative.");

        if (_adjacencyList.TryAdd(vertex, new Dictionary<int, long>()))
        {
            _inDegrees.TryAdd(vertex, 0); // 新しい頂点の入次数は0
            return true;
        }
        return false;
    }

    /// <summary>
    /// グラフに複数の辺を追加します。
    /// 辺が存在しない場合は追加され、既に存在する場合は無視されます。自己ループも無視されます。
    /// 重みが指定されていない場合はデフォルトで 1 が使用されます。
    /// </summary>
    /// <param name="edges">辺の配列。各要素は {from, to} (重み1) または {from, to, weight} の形式です。</param>
    /// <exception cref="ArgumentNullException"><paramref name="edges"/>がnullの場合。</exception>
    /// <exception cref="ArgumentException">辺の配列の形式が不正な場合にスローされます。</exception>
    /// <exception cref="ArgumentOutOfRangeException">頂点IDが負の場合。</exception>
    public void AddEdges(int[][] edges)
    {
        ArgumentNullException.ThrowIfNull(edges);

        foreach (var edge in edges)
        {
            if (edge == null || edge.Length < 2 || edge.Length > 3)
            {
                throw new ArgumentException("Each edge array must be in the format {from, to} or {from, to, weight}.", nameof(edges));
            }

            int from = edge[0];
            int to = edge[1];
            long weight = (edge.Length == 3) ? edge[2] : 1L; // デフォルト重み 1L

            // TryAddEdge を使用して、存在しない場合のみ追加
            TryAddEdge(from, to, weight);
        }
    }

    /// <summary>
    /// グラフに重み 1 の辺を追加または更新します。
    /// 頂点が存在しない場合は自動的に追加されます。
    /// 自己ループは無視されます。
    /// </summary>
    /// <param name="from">辺の始点のID。非負である必要があります。</param>
    /// <param name="to">辺の終点のID。非負である必要があります。</param>
    /// <exception cref="ArgumentOutOfRangeException">頂点IDが負の場合。</exception>
    public void AddEdge(int from, int to)
    {
        AddOrUpdateEdge(from, to, 1L);
    }

    /// <summary>
    /// グラフに重み付きの辺を追加または更新します。
    /// 頂点が存在しない場合は自動的に追加されます。
    /// 自己ループは無視されます。
    /// </summary>
    /// <param name="from">辺の始点のID。非負である必要があります。</param>
    /// <param name="to">辺の終点のID。非負である必要があります。</param>
    /// <param name="weight">辺の重み。</param>
    /// <exception cref="ArgumentOutOfRangeException">頂点IDが負の場合。</exception>
    public void AddOrUpdateEdge(int from, int to, long weight)
    {
        // 自己ループは許可しない
        if (from == to) return;
        if (from < 0) throw new ArgumentOutOfRangeException(nameof(from), "Vertex ID cannot be negative.");
        if (to < 0) throw new ArgumentOutOfRangeException(nameof(to), "Vertex ID cannot be negative.");

        // 頂点が存在しない場合は追加
        AddVertex(from); // 内部で存在チェックするので TryAdd 不要
        AddVertex(to);   // 内部で存在チェックするので TryAdd 不要

        // 辺が既に存在したかを確認 (入次数と辺数更新のため)
        bool edgeExisted = _adjacencyList[from].ContainsKey(to);

        // 辺を追加または更新
        _adjacencyList[from][to] = weight;

        // 辺が新しく追加された場合のみ入次数と辺数を更新
        if (!edgeExisted)
        {
            _inDegrees[to] = _inDegrees.GetValueOrDefault(to, 0) + 1;
            EdgeCount++;
        }
    }

    /// <summary>
    /// グラフに重み付きの辺を追加しようと試みます。
    /// 頂点が存在しない場合は自動的に追加されます。
    /// 辺が既に存在する場合、または自己ループになる場合は、このメソッドは何もしません。
    /// </summary>
    /// <param name="from">辺の始点のID。非負である必要があります。</param>
    /// <param name="to">辺の終点のID。非負である必要があります。</param>
    /// <param name="weight">辺の重み。</param>
    /// <returns>辺が新しく追加された場合は true、それ以外の場合は false。</returns>
    /// <exception cref="ArgumentOutOfRangeException">頂点IDが負の場合。</exception>
    public bool TryAddEdge(int from, int to, long weight)
    {
        // 自己ループは許可しない
        if (from == to) return false;
        if (from < 0) throw new ArgumentOutOfRangeException(nameof(from), "Vertex ID cannot be negative.");
        if (to < 0) throw new ArgumentOutOfRangeException(nameof(to), "Vertex ID cannot be negative.");

        // 頂点が存在しない場合は追加
        AddVertex(from);
        AddVertex(to);

        // 辺を追加しようと試みる
        if (_adjacencyList[from].TryAdd(to, weight))
        {
            // 追加に成功した場合のみ入次数と辺数を更新
            _inDegrees[to] = _inDegrees.GetValueOrDefault(to, 0) + 1;
            EdgeCount++;
            return true;
        }
        return false;
    }


    /// <summary>
    /// 辺の重みを取得します。
    /// </summary>
    /// <param name="from">辺の始点のID。</param>
    /// <param name="to">辺の終点のID。</param>
    /// <returns>辺の重み。</returns>
    /// <exception cref="KeyNotFoundException">指定された頂点または辺が存在しない場合にスローされます。</exception>
    public long GetEdgeWeight(int from, int to)
    {
        if (!_adjacencyList.TryGetValue(from, out var neighbors))
            throw new KeyNotFoundException($"Vertex {from} does not exist in the graph.");
        if (neighbors.TryGetValue(to, out var weight))
        {
            return weight;
        }
        throw new KeyNotFoundException($"Edge ({from} -> {to}) does not exist.");
    }

    /// <summary>
    /// 辺の重みを安全に取得します。
    /// </summary>
    /// <param name="from">辺の始点のID。</param>
    /// <param name="to">辺の終点のID。</param>
    /// <param name="weight">辺が存在する場合、その重みが設定されます。</param>
    /// <returns>辺が存在する場合は true、存在しない場合は false。</returns>
    public bool TryGetEdgeWeight(int from, int to, out long weight)
    {
        weight = default;
        // 頂点の存在も暗黙的にチェックされる
        return _adjacencyList.TryGetValue(from, out var neighbors) && neighbors.TryGetValue(to, out weight);
    }

    /// <summary>
    /// 辺が存在するかどうかを確認します。
    /// </summary>
    /// <param name="from">辺の始点のID。</param>
    /// <param name="to">辺の終点のID。</param>
    /// <returns>辺が存在する場合は true、存在しない場合は false。</returns>
    public bool ContainsEdge(int from, int to)
    {
        if (from == to) return false; // 自己ループは存在しない
        return _adjacencyList.TryGetValue(from, out var neighbors) && neighbors.ContainsKey(to);
    }

    /// <summary>
    /// 頂点がグラフに存在するかどうかを確認します。
    /// </summary>
    /// <param name="vertex">確認する頂点のID。</param>
    /// <returns>頂点が存在する場合はtrue、それ以外の場合はfalse。</returns>
    public bool ContainsVertex(int vertex)
    {
        return _adjacencyList.ContainsKey(vertex);
    }

    /// <summary>
    /// グラフから辺を削除します。
    /// </summary>
    /// <param name="from">辺の始点のID。</param>
    /// <param name="to">辺の終点のID。</param>
    /// <returns>辺が削除された場合は true、辺が存在しなかった場合は false。</returns>
    public bool RemoveEdge(int from, int to)
    {
        // 自己ループ削除の試みは常にfalse
        if (from == to) return false;

        if (_adjacencyList.TryGetValue(from, out var neighbors))
        {
            if (neighbors.Remove(to))
            {
                // 辺が存在した場合のみ入次数と辺数を更新
                // _inDegreesにtoが存在しないことは通常ないはず
                _inDegrees[to]--;
                EdgeCount--;
                return true;
            }
        }
        return false;
    }

    // Note: 頂点の削除(RemoveVertex)は実装が複雑になるため除外 (依存辺の削除、入次数/出次数の更新など)
    // public bool RemoveVertex(int vertex) { ... }

    /// <summary>
    /// グラフに含まれるすべての頂点IDを取得します。
    /// 返されるコレクションは読み取り専用です。
    /// </summary>
    /// <returns>グラフの頂点IDの読み取り専用コレクション。</returns>
    public IReadOnlyCollection<int> GetVertices()
    {
        return _adjacencyList.Keys;
    }

    /// <summary>
    /// 指定された頂点から出ていく辺の終点（隣接頂点）を取得します。
    /// 返されるコレクションは読み取り専用です。
    /// </summary>
    /// <param name="vertex">始点となる頂点のID。</param>
    /// <returns>隣接する頂点のIDを含む読み取り専用コレクション。頂点が存在しない場合は空のコレクション。</returns>
    public IReadOnlyCollection<int> GetNeighbors(int vertex)
    {
        if (_adjacencyList.TryGetValue(vertex, out var neighbors))
        {
            return neighbors.Keys;
        }
        return Array.Empty<int>();
    }

    /// <summary>
    /// 指定された頂点から出ていく辺（隣接頂点とその重み）を取得します。
    /// 返される辞書は読み取り専用です。
    /// </summary>
    /// <param name="vertex">始点となる頂点のID。</param>
    /// <returns>隣接する頂点と重みのペアの読み取り専用辞書。頂点が存在しない場合は空の辞書。</returns>
    public IReadOnlyDictionary<int, long> GetOutgoingEdges(int vertex)
    {
        if (_adjacencyList.TryGetValue(vertex, out var neighbors))
        {
            return new ReadOnlyDictionary<int, long>(neighbors);
        }
        return new ReadOnlyDictionary<int, long>(new Dictionary<int, long>());
    }

    /// <summary>
    /// 指定された頂点の入次数（その頂点に入る辺の数）を取得します。
    /// </summary>
    /// <param name="vertex">入次数を調べる頂点のID。</param>
    /// <returns>頂点の入次数。頂点が存在しない場合は 0。</returns>
    public int GetInDegree(int vertex)
    {
        return _inDegrees.GetValueOrDefault(vertex, 0);
    }

    /// <summary>
    /// 指定された頂点の出次数（その頂点から出る辺の数）を取得します。
    /// </summary>
    /// <param name="vertex">出次数を調べる頂点のID。</param>
    /// <returns>頂点の出次数。頂点が存在しない場合は 0。</returns>
    public int GetOutDegree(int vertex)
    {
        if (_adjacencyList.TryGetValue(vertex, out var neighbors))
        {
            return neighbors.Count;
        }
        return 0;
    }

    /// <summary>
    /// グラフの向きを逆にした新しいグラフを生成します。
    /// </summary>
    /// <returns>辺の向きが逆転した新しい SimpleDirectedGraph インスタンス。</returns>
    public SimpleDirectedGraph GenerateReversed()
    {
        var reversedGraph = new SimpleDirectedGraph();
        // 全ての頂点をまず追加 (孤立点もコピーするため)
        foreach (var vertex in GetVertices())
        {
            reversedGraph.AddVertex(vertex);
        }
        // 辺を逆向きに追加 (TryAddEdgeを使う)
        foreach (var kvp in _adjacencyList)
        {
            int fromVertex = kvp.Key;
            foreach (var edge in kvp.Value)
            {
                int toVertex = edge.Key;
                long weight = edge.Value;
                reversedGraph.TryAddEdge(toVertex, fromVertex, weight); // AddOrUpdateEdge でも可
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
    public bool ContainsCycle()
    {
        return TryFindCycle(out _);
    }

    /// <summary>
    /// グラフ内の閉路を1つ検出し、その経路上の頂点を返します。
    /// 閉路が複数存在する場合、どの閉路が検出されるかは不定です。
    /// </summary>
    /// <param name="cyclePath">検出された閉路の頂点のリスト（順序付き）。閉路が存在しない場合は null。</param>
    /// <returns>閉路が検出された場合は true、存在しない場合は false。</returns>
    public bool TryFindCycle(out List<int>? cyclePath)
    {
        var visitState = new Dictionary<int, VisitState>();
        var recursionStack = new Stack<int>(); // 現在の探索パス

        foreach (int vertex in GetVertices())
        {
            if (!visitState.ContainsKey(vertex)) // Unvisitedと同じ扱い
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
    private bool TryFindCycleDfs(int currentVertex, Dictionary<int, VisitState> visitState, Stack<int> recursionStack, out List<int>? cyclePath)
    {
        visitState[currentVertex] = VisitState.Visiting;
        recursionStack.Push(currentVertex);

        // GetNeighbors を使用して隣接頂点を取得
        foreach (var neighbor in GetNeighbors(currentVertex))
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
    public bool TryTopologicalSort(out List<int>? sortedVertices)
    {
        return TryTopologicalSortInternal(false, out sortedVertices);
    }

    /// <summary>
    /// グラフを辞書順で最小となるようにトポロジカルソートします。
    /// グラフに閉路が存在する場合、ソート結果は不完全（全頂点を含まない）になります。
    /// </summary>
    /// <param name="sortedVertices">辞書順最小でトポロジカルソートされた頂点のリスト。閉路が存在する場合は null。</param>
    /// <returns>トポロジカルソートが成功した（閉路がなかった）場合は true、閉路が存在した場合は false。</returns>
    public bool TryTopologicalSortLexicographical(out List<int>? sortedVertices)
    {
        return TryTopologicalSortInternal(true, out sortedVertices);
    }

    // トポロジカルソートの内部実装
    private bool TryTopologicalSortInternal(bool lexicographical, out List<int>? sortedVertices)
    {
        sortedVertices = new List<int>();
        var currentInDegrees = new Dictionary<int, int>(_inDegrees); // 入次数のコピーを作成
        object zeroInDegreeQueue = InitializeZeroInDegreeQueue(lexicographical, currentInDegrees); // 型はobject

        while (GetQueueCount(zeroInDegreeQueue, lexicographical) > 0)
        {
            int currentVertex = DequeueFrom(zeroInDegreeQueue, lexicographical);
            sortedVertices.Add(currentVertex);

            // 隣接頂点の入次数を減らす (GetNeighborsを使用)
            foreach (var neighbor in GetNeighbors(currentVertex))
            {
                if (currentInDegrees.ContainsKey(neighbor)) // 念のため確認
                {
                    currentInDegrees[neighbor]--;
                    if (currentInDegrees[neighbor] == 0)
                    {
                        EnqueueTo(zeroInDegreeQueue, neighbor, lexicographical);
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
            sortedVertices = null;
            return false;
        }
    }

    // キュー/優先度付きキューの要素数を取得するヘルパー
    private int GetQueueCount(object queue, bool lexicographical)
    {
        return lexicographical
            ? ((PriorityQueue<int, int>)queue).Count
            : ((Queue<int>)queue).Count;
    }

    // トポロジカルソート用のキュー/優先度付きキュー初期化ヘルパー
    private object InitializeZeroInDegreeQueue(bool lexicographical, Dictionary<int, int> currentInDegrees)
    {
        // 入次数0の頂点を抽出
        var sources = GetVertices().Where(v => currentInDegrees.GetValueOrDefault(v, 0) == 0);

        if (lexicographical)
        {
            // 辞書順の場合はPriorityQueue (Min-Heap)
            var pq = new PriorityQueue<int, int>();
            // OrderByは不要 (PriorityQueueが順序を管理)
            foreach (var vertex in sources)
            {
                pq.Enqueue(vertex, vertex); // 優先度も頂点ID
            }
            return pq;
        }
        else
        {
            // 通常はQueue
            return new Queue<int>(sources);
        }
    }

    // トポロジカルソート用のキュー/優先度付きキューへの追加ヘルパー
    private void EnqueueTo(object queue, int vertex, bool lexicographical)
    {
        if (lexicographical)
            ((PriorityQueue<int, int>)queue).Enqueue(vertex, vertex);
        else
            ((Queue<int>)queue).Enqueue(vertex);
    }

    // トポロジカルソート用のキュー/優先度付きキューからの取り出しヘルパー
    private int DequeueFrom(object queue, bool lexicographical)
    {
        return lexicographical
            ? ((PriorityQueue<int, int>)queue).Dequeue()
            : ((Queue<int>)queue).Dequeue();
    }


    // --- ダイクストラ法 (Dijkstra's Algorithm) ---

    /// <summary>
    /// ダイクストラ法を用いて、指定された始点から他の全ての頂点への最短経路コストを計算します。
    /// 負の重みを持つ辺が存在する場合、正しい結果が得られない可能性があります。
    /// </summary>
    /// <param name="startVertex">始点となる頂点のID。非負である必要があります。</param>
    /// <returns>
    /// 各頂点への最短経路コストを格納した辞書。
    /// キーは頂点ID、値は始点からの最短コストです。
    /// 始点から到達不可能な頂点は辞書に含まれません。
    /// 始点自身のコストは 0 です。
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="startVertex"/>が負の場合。</exception>
    /// <exception cref="KeyNotFoundException">始点<paramref name="startVertex"/>がグラフに存在しない場合。</exception>
    public Dictionary<int, long> Dijkstra(int startVertex)
    {
        if (startVertex < 0)
            throw new ArgumentOutOfRangeException(nameof(startVertex), "Vertex ID cannot be negative.");
        if (!ContainsVertex(startVertex))
            throw new KeyNotFoundException($"Start vertex {startVertex} does not exist in the graph.");

        var costs = new Dictionary<int, long>(); // 始点からの確定した最短コスト
                                                 // 優先度付きキュー: {頂点, 始点からの暫定コスト} コストが小さい順に取り出す
        var priorityQueue = new PriorityQueue<int, long>();

        // 初期化
        costs[startVertex] = 0L;
        priorityQueue.Enqueue(startVertex, 0L);

        while (priorityQueue.TryDequeue(out int currentVertex, out long currentCost))
        {
            // キューから取り出したコストが、記録されている最短コストより大きい場合はスキップ
            // (より短い経路が既に見つかっている場合)
            if (costs.TryGetValue(currentVertex, out long recordedCost) && currentCost > recordedCost)
            {
                continue;
            }

            // 隣接頂点のコストを更新 (GetOutgoingEdgesを使用)
            foreach (var edge in GetOutgoingEdges(currentVertex))
            {
                int neighbor = edge.Key;
                long weight = edge.Value;

                // 負の重みチェック（ダイクストラ法の前提）
                // if (weight < 0) throw new InvalidOperationException("Dijkstra algorithm cannot handle negative weights.");

                long newCost = currentCost + weight;

                // オーバーフローチェック (任意、longの最大値を超えるような巨大なコストの場合)
                if (currentCost > 0 && weight > 0 && newCost < currentCost)
                {
                    newCost = long.MaxValue; // または他のオーバーフロー処理
                }


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
    /// <param name="startVertex">始点となる頂点のID。非負である必要があります。</param>
    /// <param name="endVertex">終点となる頂点のID。非負である必要があります。</param>
    /// <returns>始点から終点への最短経路コスト。到達不可能な場合は long.MaxValue。</returns>
    /// <exception cref="ArgumentOutOfRangeException">頂点IDが負の場合。</exception>
    /// <exception cref="KeyNotFoundException">始点または終点がグラフに存在しない場合。</exception>
    public long Dijkstra(int startVertex, int endVertex)
    {
        if (startVertex < 0)
            throw new ArgumentOutOfRangeException(nameof(startVertex), "Vertex ID cannot be negative.");
        if (endVertex < 0)
            throw new ArgumentOutOfRangeException(nameof(endVertex), "Vertex ID cannot be negative.");
        if (!ContainsVertex(startVertex))
            throw new KeyNotFoundException($"Start vertex {startVertex} does not exist in the graph.");
        if (!ContainsVertex(endVertex))
            throw new KeyNotFoundException($"End vertex {endVertex} does not exist in the graph.");

        // 最適化: 終点に到達した時点で探索を打ち切るダイクストラ法の実装も可能だが、
        // ここでは全頂点へのコスト計算後に結果を取得するシンプルな方法を採用。
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