namespace TAmeAtCoderLibrary;

/// <summary>
/// 頂点が非負整数で識別され、辺が長整数の重みを持つことができる単純無向グラフを表します。
/// このグラフは自己ループを許可しません。
/// 頂点と辺の追加/削除、隣接頂点と辺の重みの取得、葉ノードの検出、サイクルの検出などの機能を提供します。
/// 重みが指定されない場合、デフォルトで 1 が設定されます。
/// </summary>
public class SimpleUndirectedGraph
{
    /// <summary>
    /// グラフの隣接リストを格納します。
    /// キー: 頂点ID。
    /// 値: キーが隣接頂点ID、値が辺の重みである辞書。
    /// </summary>
    private readonly Dictionary<int, Dictionary<int, long>> _adjacencyList = new();

    /// <summary>
    /// グラフ内の辺の数を格納します。
    /// </summary>
    private int _edgeCount = 0;

    /// <summary>
    /// 空の単純無向グラフを初期化します。
    /// </summary>
    public SimpleUndirectedGraph() { }

    /// <summary>
    /// 指定した最大ID値までの頂点と辺のリストを持つ単純無向グラフを初期化します。
    /// 1からmaxVertexId（含む）までのIDを持つ頂点が事前に追加されます。
    /// 辺の重みはデフォルトで1です。自己ループは無視されます。
    /// </summary>
    /// <param name="maxVertexId">事前に追加される頂点の最大ID（含む、1から開始）。非負でなければなりません。</param>
    /// <param name="edges">辺の配列。各辺は配列{vertexA, vertexB}で表されます。</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxVertexId"/>が負の場合にスローされます。</exception>
    /// <exception cref="ArgumentNullException"><paramref name="edges"/>がnullの場合にスローされます。</exception>
    /// <exception cref="ArgumentException"><paramref name="edges"/>内の任意の辺の配列がnullまたは正確に2つの頂点IDを含まない場合にスローされます。</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="edges"/>内の任意の頂点IDが負の場合にスローされます。</exception>
    public SimpleUndirectedGraph(int maxVertexId, int[][] edges)
    {
        if (maxVertexId < 0)
            throw new ArgumentOutOfRangeException(nameof(maxVertexId), "Maximum vertex ID cannot be negative.");

        // 1からmaxVertexIdまでの頂点を事前に追加
        for (int vertexId = 1; vertexId <= maxVertexId; vertexId++)
        {
            _adjacencyList.TryAdd(vertexId, new Dictionary<int, long>());
        }

        AddEdges(edges); // 提供された辺を追加 (デフォルト重み 1 で追加される)
    }

    /// <summary>
    /// 辺のリストから単純無向グラフを初期化します。
    /// 頂点は提供された辺に基づいて自動的に追加されます。辺の重みはデフォルトで1です。
    /// 自己ループは無視されます。
    /// </summary>
    /// <param name="edges">辺の配列。各辺は配列{vertexA, vertexB}で表されます。</param>
    /// <exception cref="ArgumentNullException"><paramref name="edges"/>がnullの場合にスローされます。</exception>
    /// <exception cref="ArgumentException"><paramref name="edges"/>内の任意の辺の配列がnullまたは正確に2つの頂点IDを含まない場合にスローされます。</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="edges"/>内の任意の頂点IDが負の場合にスローされます。</exception>
    public SimpleUndirectedGraph(int[][] edges)
    {
        AddEdges(edges); // デフォルト重み 1 で追加される
    }

    /// <summary>
    /// グラフ内の現在の頂点数を取得します。
    /// </summary>
    public int VertexCount => _adjacencyList.Count;

    /// <summary>
    /// グラフ内の現在の辺の数を取得します。
    /// 自己ループはカウントされません。各辺は1回だけカウントされます。
    /// </summary>
    public int EdgeCount => _edgeCount;

    /// <summary>
    /// グラフに頂点を追加します。
    /// </summary>
    /// <param name="vertex">追加する頂点のID。非負でなければなりません。</param>
    /// <exception cref="ArgumentOutOfRangeException">頂点IDが負の場合にスローされます。</exception>
    /// <exception cref="ArgumentException">同じIDを持つ頂点が既に存在する場合にスローされます。</exception>
    public void AddVertex(int vertex)
    {
        if (vertex < 0)
            throw new ArgumentOutOfRangeException(nameof(vertex), "Vertex ID cannot be negative.");
        if (_adjacencyList.ContainsKey(vertex))
            throw new ArgumentException($"Vertex {vertex} already exists.", nameof(vertex));

        _adjacencyList.Add(vertex, new Dictionary<int, long>());
    }

    /// <summary>
    /// デフォルトの重み1で複数の辺をグラフに追加します。
    /// 辺の頂点が存在しない場合は自動的に追加されます。
    /// 辺が既に存在する場合は無視されます（重みは更新されません）。
    /// 自己ループ（同じ頂点を結ぶ辺）は無視されます。
    /// </summary>
    /// <param name="edges">辺の配列。各辺は配列{vertexA, vertexB}で表されます。</param>
    /// <exception cref="ArgumentNullException"><paramref name="edges"/>がnullの場合にスローされます。</exception>
    /// <exception cref="ArgumentException">任意の辺の配列がnullまたは正確に2つの頂点IDを含まない場合にスローされます。</exception>
    /// <exception cref="ArgumentOutOfRangeException">任意の頂点IDが負の場合にスローされます。</exception>
    public void AddEdges(int[][] edges)
    {
        ArgumentNullException.ThrowIfNull(edges);

        foreach (var edge in edges)
        {
            if (edge == null || edge.Length != 2)
                throw new ArgumentException("Each edge array must contain exactly two vertex IDs.", nameof(edges));

            // AddEdge(int, int) を呼び出すことで、デフォルト重み 1 が使用される
            AddEdge(edge[0], edge[1]);
        }
    }

    /// <summary>
    /// 重み1の辺を2つの頂点間に追加します。
    /// 頂点が存在しない場合は自動的に追加されます。
    /// 辺が既に存在する場合、または自己ループになる場合、このメソッドは何もしません。
    /// </summary>
    /// <param name="vertexA">最初の頂点のID。非負でなければなりません。</param>
    /// <param name="vertexB">2番目の頂点のID。非負でなければなりません。</param>
    /// <exception cref="ArgumentOutOfRangeException">任意の頂点IDが負の場合にスローされます。</exception>
    public void AddEdge(int vertexA, int vertexB)
    {
        // デフォルトの重みを 1L に変更
        AddEdge(vertexA, vertexB, 1L);
    }

    /// <summary>
    /// 指定された重み付きの辺を2つの頂点間に追加します。
    /// 頂点が存在しない場合は自動的に追加されます。
    /// 辺が既に存在する場合、または自己ループになる場合、このメソッドは何もしません（重みを変更するには<see cref="AddOrUpdateEdge"/>を使用してください）。
    /// </summary>
    /// <param name="vertexA">最初の頂点のID。非負でなければなりません。</param>
    /// <param name="vertexB">2番目の頂点のID。非負でなければなりません。</param>
    /// <param name="weight">辺の重み。</param>
    /// <exception cref="ArgumentOutOfRangeException">任意の頂点IDが負の場合にスローされます。</exception>
    public void AddEdge(int vertexA, int vertexB, long weight)
    {
        // 自己ループは許可しない
        if (vertexA == vertexB) return;

        // 潜在的に追加する前に頂点が非負であることを確認
        if (vertexA < 0) throw new ArgumentOutOfRangeException(nameof(vertexA), "Vertex ID cannot be negative.");
        if (vertexB < 0) throw new ArgumentOutOfRangeException(nameof(vertexB), "Vertex ID cannot be negative.");

        // 頂点が存在しない場合は追加
        AddVertexIfNotExists(vertexA);
        AddVertexIfNotExists(vertexB);

        // TryAddは既存の辺の重みを上書きせず、キーが存在する場合の例外を回避します
        // また、辺が新たに追加されたかどうかを返します
        bool addedA = _adjacencyList[vertexA].TryAdd(vertexB, weight);
        bool addedB = _adjacencyList[vertexB].TryAdd(vertexA, weight);

        // 両方向の追加が成功した場合のみ（つまり、辺が新規の場合のみ）辺カウントを増やす
        if (addedA && addedB)
        {
            _edgeCount++;
        }
    }

    /// <summary>
    /// 重み付きの辺を2つの頂点間に追加するか、辺が既に存在する場合は重みを更新します。
    /// 頂点が存在しない場合は自動的に追加されます。
    /// 自己ループ（同じ頂点を結ぶ辺）は無視されます。
    /// </summary>
    /// <param name="vertexA">最初の頂点のID。非負でなければなりません。</param>
    /// <param name="vertexB">2番目の頂点のID。非負でなければなりません。</param>
    /// <param name="weight">辺の重み。</param>
    /// <exception cref="ArgumentOutOfRangeException">任意の頂点IDが負の場合にスローされます。</exception>
    public void AddOrUpdateEdge(int vertexA, int vertexB, long weight)
    {
        // 自己ループは許可しない
        if (vertexA == vertexB) return;

        if (vertexA < 0) throw new ArgumentOutOfRangeException(nameof(vertexA), "Vertex ID cannot be negative.");
        if (vertexB < 0) throw new ArgumentOutOfRangeException(nameof(vertexB), "Vertex ID cannot be negative.");

        AddVertexIfNotExists(vertexA);
        AddVertexIfNotExists(vertexB);

        // 辺が新たに追加されるか確認
        bool edgeExists = _adjacencyList[vertexA].ContainsKey(vertexB);

        // インデクサーを使用して重みを直接追加または更新
        _adjacencyList[vertexA][vertexB] = weight;
        _adjacencyList[vertexB][vertexA] = weight;

        // 辺が存在しなかった場合に辺カウントを増やす
        if (!edgeExists)
        {
            _edgeCount++;
        }
    }

    /// <summary>
    /// 頂点とそれに接続するすべての辺をグラフから削除します。
    /// </summary>
    /// <param name="vertex">削除する頂点のID。</param>
    /// <returns>頂点が見つかり削除された場合はtrue、それ以外の場合はfalse。</returns>
    public bool RemoveVertex(int vertex)
    {
        if (!_adjacencyList.TryGetValue(vertex, out var edgesToRemove))
            return false; // 頂点が存在しない

        int degree = edgesToRemove.Count;
        var neighbors = edgesToRemove.Keys.ToList();

        foreach (var neighbor in neighbors)
        {
            if (_adjacencyList.TryGetValue(neighbor, out var neighborEdges))
            {
                neighborEdges.Remove(vertex);
            }
        }

        _adjacencyList.Remove(vertex);
        _edgeCount -= degree;

        return true;
    }

    /// <summary>
    /// 2つの頂点間の辺を削除します。
    /// </summary>
    /// <param name="vertexA">最初の頂点のID。</param>
    /// <param name="vertexB">2番目の頂点のID。</param>
    /// <returns>辺が見つかり両方の隣接リストから削除された場合はtrue、それ以外の場合はfalse。</returns>
    public bool RemoveEdge(int vertexA, int vertexB)
    {
        if (vertexA == vertexB) return false;

        bool removedA = false;
        bool removedB = false;

        if (_adjacencyList.TryGetValue(vertexA, out var edgesA))
        {
            removedA = edgesA.Remove(vertexB);
        }
        if (_adjacencyList.TryGetValue(vertexB, out var edgesB))
        {
            removedB = edgesB.Remove(vertexA);
        }

        if (removedA && removedB)
        {
            _edgeCount--;
            return true;
        }

        return false;
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
    /// 2つの頂点間に辺が存在するかどうかを確認します。
    /// </summary>
    /// <param name="vertexA">最初の頂点のID。</param>
    /// <param name="vertexB">2番目の頂点のID。</param>
    /// <returns>頂点間に辺が存在する場合はtrue、それ以外の場合はfalse。</returns>
    public bool ContainsEdge(int vertexA, int vertexB)
    {
        if (vertexA == vertexB) return false;
        return _adjacencyList.TryGetValue(vertexA, out var edgesA) && edgesA.ContainsKey(vertexB);
    }

    /// <summary>
    /// 指定された頂点に隣接するすべての頂点のID（その隣接頂点）を取得します。
    /// </summary>
    /// <param name="vertex">隣接頂点を取得する頂点のID。</param>
    /// <returns>隣接頂点のIDを含む読み取り専用コレクション。頂点が存在しないか隣接頂点がない場合は空のコレクションを返します。</returns>
    public IReadOnlyCollection<int> GetNeighbors(int vertex)
    {
        if (_adjacencyList.TryGetValue(vertex, out var neighbors))
        {
            return neighbors.Keys;
        }
        return Array.Empty<int>();
    }

    /// <summary>
    /// 2つの指定された頂点間の辺の重みを取得します。
    /// </summary>
    /// <param name="vertexA">最初の頂点のID。</param>
    /// <param name="vertexB">2番目の頂点のID。</param>
    /// <returns>辺の重み。</returns>
    /// <exception cref="KeyNotFoundException">いずれかの頂点が存在しないか、それらの間の辺が存在しない場合にスローされます。</exception>
    public long GetEdgeWeight(int vertexA, int vertexB)
    {
        if (!_adjacencyList.TryGetValue(vertexA, out var edgesA))
            throw new KeyNotFoundException($"Vertex {vertexA} does not exist in the graph.");

        if (!edgesA.TryGetValue(vertexB, out var weight))
            throw new KeyNotFoundException($"Edge between vertex {vertexA} and vertex {vertexB} does not exist.");

        return weight;
    }

    /// <summary>
    /// 指定された頂点の次数（接続辺の数）を取得します。
    /// </summary>
    /// <param name="vertex">頂点のID。</param>
    /// <returns>頂点の次数。頂点が存在しない場合は0。</returns>
    public int GetDegree(int vertex)
    {
        if (_adjacencyList.TryGetValue(vertex, out var neighbors))
        {
            return neighbors.Count;
        }
        return 0;
    }


    /// <summary>
    /// グラフ内の葉（次数0または1の頂点）であるすべての頂点を取得します。
    /// 孤立頂点（次数0）も葉と見なされます。
    /// </summary>
    /// <returns>すべての葉頂点のIDを含むハッシュセット。</returns>
    public HashSet<int> GetLeaves()
    {
        var leaves = new HashSet<int>();
        foreach (var kvp in _adjacencyList)
        {
            if (kvp.Value.Count <= 1)
            {
                leaves.Add(kvp.Key);
            }
        }
        return leaves;
    }

    /// <summary>
    /// 幅優先探索（BFS）を使用してグラフにサイクルが含まれているかどうかを確認します。
    /// 各コンポーネントをチェックすることで非連結グラフを処理します。
    /// </summary>
    /// <returns>サイクルが検出された場合はtrue、それ以外の場合はfalse。</returns>
    public bool ContainsCycle()
    {
        var globallyVisited = new HashSet<int>();
        var parentMap = new Dictionary<int, int>();

        foreach (var startVertex in _adjacencyList.Keys)
        {
            if (globallyVisited.Contains(startVertex))
                continue;

            var queue = new Queue<int>();
            var currentComponentVisited = new HashSet<int>();

            queue.Enqueue(startVertex);
            currentComponentVisited.Add(startVertex);
            globallyVisited.Add(startVertex);
            parentMap[startVertex] = -1; // Use -1 or another value not used as a vertex ID

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();

                if (!_adjacencyList.TryGetValue(currentNode, out var neighbors)) continue;

                foreach (var neighborNode in neighbors.Keys)
                {
                    if (parentMap.TryGetValue(currentNode, out int parent) && neighborNode == parent)
                        continue;

                    if (currentComponentVisited.Contains(neighborNode))
                    {
                        return true; // Cycle detected
                    }

                    if (!globallyVisited.Contains(neighborNode))
                    {
                        globallyVisited.Add(neighborNode);
                        currentComponentVisited.Add(neighborNode);
                        parentMap[neighborNode] = currentNode;
                        queue.Enqueue(neighborNode);
                    }
                }
            }
        }
        return false; // No cycle found
    }

    /// <summary>
    /// グラフ内に現在存在するすべての頂点IDを取得します。
    /// </summary>
    /// <returns>すべての頂点IDを含む読み取り専用コレクション。</returns>
    public IReadOnlyCollection<int> GetVertices()
    {
        return _adjacencyList.Keys;
    }


    // === プライベートヘルパーメソッド ===

    /// <summary>
    /// 頂点がまだ存在しない場合にグラフに追加します。
    /// 内部ヘルパーメソッド。頂点IDが呼び出し側によって非負として検証されていることを前提とします。
    /// </summary>
    /// <param name="vertex">追加する頂点のID。</param>
    private void AddVertexIfNotExists(int vertex)
    {
        _adjacencyList.TryAdd(vertex, new Dictionary<int, long>());
    }
}