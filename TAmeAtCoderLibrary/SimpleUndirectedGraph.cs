namespace TAmeAtCoderLibrary;

/// <summary>
/// 頂点が非負整数で識別され、辺が長整数の重みを持つことができる単純無向グラフを表します。
/// 頂点と辺の追加/削除、隣接頂点と辺の重みの取得、葉ノードの検出、サイクルの検出などの機能を提供します。
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
    /// 空の単純無向グラフを初期化します。
    /// </summary>
    public SimpleUndirectedGraph() { }

    /// <summary>
    /// 指定した最大ID値までの頂点と辺のリストを持つ単純無向グラフを初期化します。
    /// 1からmaxVertexId（含む）までのIDを持つ頂点が事前に追加されます。
    /// 辺の重みはデフォルトで0です。
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
            // 安全のためにAddVertexIfNotExistsを使用、ただしAddVertexも使用可能です。
            // AddVertexは後でID 0が試みられた場合に例外をスローする可能性があり、それが望ましい場合もあります。
            // 頂点IDが一意であるべきと仮定して、より厳格な制御のためにAddVertexを使用します。
            if (!_adjacencyList.ContainsKey(vertexId)) // 内部でAddVertexが呼び出された場合に例外を回避
                _adjacencyList.Add(vertexId, new Dictionary<int, long>());
        }

        AddEdges(edges); // 提供された辺を追加
    }

    /// <summary>
    /// 辺のリストから単純無向グラフを初期化します。
    /// 頂点は提供された辺に基づいて自動的に追加されます。辺の重みはデフォルトで0です。
    /// </summary>
    /// <param name="edges">辺の配列。各辺は配列{vertexA, vertexB}で表されます。</param>
    /// <exception cref="ArgumentNullException"><paramref name="edges"/>がnullの場合にスローされます。</exception>
    /// <exception cref="ArgumentException"><paramref name="edges"/>内の任意の辺の配列がnullまたは正確に2つの頂点IDを含まない場合にスローされます。</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="edges"/>内の任意の頂点IDが負の場合にスローされます。</exception>
    public SimpleUndirectedGraph(int[][] edges)
    {
        AddEdges(edges);
    }

    /// <summary>
    /// グラフ内の現在の頂点数を取得します。
    /// </summary>
    public int VertexCount => _adjacencyList.Count;

    /// <summary>
    /// グラフ内の現在の辺の数を取得します。
    /// 注意: 各辺は1回だけカウントされます（例: 辺A-Bは1つの辺であり、2つではありません）。
    /// </summary>
    public int EdgeCount
    {
        get
        {
            // 次数の合計を2で割ると、無向グラフの辺の数が得られます。
            // 次数が大きい場合のオーバーフローを防ぐためにlongを使用します。
            long degreeSum = 0;
            foreach (var edges in _adjacencyList.Values)
            {
                degreeSum += edges.Count;
            }
            // 各辺は2つの頂点の次数に寄与するため、2で割ります。
            return (int)(degreeSum / 2);
        }
    }

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
    /// デフォルトの重み0で複数の辺をグラフに追加します。
    /// 辺の頂点が存在しない場合は自動的に追加されます。
    /// 辺が既に存在する場合は無視されます（重みは更新されません）。
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

            // 頂点チェックと作成を処理するAddEdgeを使用
            AddEdge(edge[0], edge[1]); // デフォルトの重みは0
        }
    }

    /// <summary>
    /// 重み0の辺を2つの頂点間に追加します。
    /// 頂点が存在しない場合は自動的に追加されます。
    /// 辺が既に存在する場合、このメソッドは何もしません。
    /// </summary>
    /// <param name="vertexA">最初の頂点のID。非負でなければなりません。</param>
    /// <param name="vertexB">2番目の頂点のID。非負でなければなりません。</param>
    /// <exception cref="ArgumentOutOfRangeException">任意の頂点IDが負の場合にスローされます。</exception>
    public void AddEdge(int vertexA, int vertexB)
    {
        AddEdge(vertexA, vertexB, 0L);
    }

    /// <summary>
    /// 重み付きの辺を2つの頂点間に追加します。
    /// 頂点が存在しない場合は自動的に追加されます。
    /// 辺が既に存在する場合、このメソッドは何もしません（重みを変更するには<see cref="AddOrUpdateEdge"/>を使用してください）。
    /// </summary>
    /// <param name="vertexA">最初の頂点のID。非負でなければなりません。</param>
    /// <param name="vertexB">2番目の頂点のID。非負でなければなりません。</param>
    /// <param name="weight">辺の重み。</param>
    /// <exception cref="ArgumentOutOfRangeException">任意の頂点IDが負の場合にスローされます。</exception>
    public void AddEdge(int vertexA, int vertexB, long weight)
    {
        // 潜在的に追加する前に頂点が非負であることを確認
        if (vertexA < 0) throw new ArgumentOutOfRangeException(nameof(vertexA), "Vertex ID cannot be negative.");
        if (vertexB < 0) throw new ArgumentOutOfRangeException(nameof(vertexB), "Vertex ID cannot be negative.");

        // 頂点が存在しない場合は追加
        AddVertexIfNotExists(vertexA);
        AddVertexIfNotExists(vertexB);

        // TryAddは既存の辺の重みを上書きせず、キーが存在する場合の例外を回避します
        _adjacencyList[vertexA].TryAdd(vertexB, weight);
        _adjacencyList[vertexB].TryAdd(vertexA, weight);
    }

    /// <summary>
    /// 重み付きの辺を2つの頂点間に追加するか、辺が既に存在する場合は重みを更新します。
    /// 頂点が存在しない場合は自動的に追加されます。
    /// </summary>
    /// <param name="vertexA">最初の頂点のID。非負でなければなりません。</param>
    /// <param name="vertexB">2番目の頂点のID。非負でなければなりません。</param>
    /// <param name="weight">辺の重み。</param>
    /// <exception cref="ArgumentOutOfRangeException">任意の頂点IDが負の場合にスローされます。</exception>
    public void AddOrUpdateEdge(int vertexA, int vertexB, long weight)
    {
        if (vertexA < 0) throw new ArgumentOutOfRangeException(nameof(vertexA), "Vertex ID cannot be negative.");
        if (vertexB < 0) throw new ArgumentOutOfRangeException(nameof(vertexB), "Vertex ID cannot be negative.");

        AddVertexIfNotExists(vertexA);
        AddVertexIfNotExists(vertexB);

        // インデクサーを使用して重みを直接追加または更新
        _adjacencyList[vertexA][vertexB] = weight;
        _adjacencyList[vertexB][vertexA] = weight;
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

        // 隣接リストを変更するため、隣接頂点IDのコピーを作成して反復処理
        var neighbors = edgesToRemove.Keys.ToList();

        // 隣接頂点の隣接リストから接続辺を削除
        foreach (var neighbor in neighbors)
        {
            if (_adjacencyList.TryGetValue(neighbor, out var neighborEdges))
            {
                neighborEdges.Remove(vertex);
            }
        }

        // メイン辞書から頂点自体を削除
        _adjacencyList.Remove(vertex);
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
        bool removedA = false;
        bool removedB = false;

        // Aのリストから辺Bを削除しようとする
        if (_adjacencyList.TryGetValue(vertexA, out var edgesA))
        {
            removedA = edgesA.Remove(vertexB);
        }
        // Bのリストから辺Aを削除しようとする
        if (_adjacencyList.TryGetValue(vertexB, out var edgesB))
        {
            removedB = edgesB.Remove(vertexA);
        }

        // 辺が存在し、両側から削除された場合にのみtrueを返す
        // （これは辺が常に双方向であるという一貫したグラフの状態を前提としています）
        return removedA && removedB;
        // 代替: return removedA || removedB; // 少なくとも一方からの削除がカウントされる場合
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
        // 効率的な確認: vertexAが存在し、vertexBへの辺があることを確認します。
        // 無向性と対称的な追加/削除ロジックのため、vertexBのリストを確認する必要はありません。
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
            // .KeysはIReadOnlyCollection<TKey>を実装するKeyCollection<TKey>を返します
            return neighbors.Keys;
        }
        // 例外をスローする代わりに、頂点が見つからない場合は空のリストを返す
        return Array.Empty<int>();
        // 代替: 頂点が存在しなければならない場合はKeyNotFoundExceptionをスロー
        // throw new KeyNotFoundException($"Vertex {vertex} does not exist in the graph.");
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
            // より具体的なエラーのために、vertexBが存在するかもチェック（ただし、辺のチェックはそれが存在するはずであることを意味します）
            if (!_adjacencyList.ContainsKey(vertexB))
                throw new KeyNotFoundException($"Vertex {vertexB} does not exist in the graph.");
            else
                throw new KeyNotFoundException($"Edge between vertex {vertexA} and {vertexB} does not exist.");


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
        return 0; // 頂点が存在しない場合は0を返す
                  // 代替: 頂点が存在しなければならない場合はKeyNotFoundExceptionをスロー
                  // throw new KeyNotFoundException($"Vertex {vertex} does not exist in the graph.");
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
            // 内部辞書のエントリ数が次数です
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
        // すべてのBFSトラバーサル（非連結コンポーネント用）で訪問した頂点を追跡
        var globallyVisited = new HashSet<int>();
        // 即座に戻るのを避けるために、現在のBFSツリーの各ノードの親を追跡
        var parentMap = new Dictionary<int, int>();

        // 非連結グラフをカバーするためにすべての潜在的な開始頂点を反復処理
        foreach (var startVertex in _adjacencyList.Keys)
        {
            if (globallyVisited.Contains(startVertex))
                continue; // このコンポーネントは既に探索済み

            var queue = new Queue<int>();
            // 現在のBFSトラバーサル内で訪問した頂点を追跡
            var currentComponentVisited = new HashSet<int>();

            queue.Enqueue(startVertex);
            currentComponentVisited.Add(startVertex);
            globallyVisited.Add(startVertex);
            parentMap[startVertex] = -1; // 開始ノードをダミーの親でマーク

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();

                // 隣接ノードを取得（キーが存在しない可能性は低いが考慮）
                if (!_adjacencyList.TryGetValue(currentNode, out var neighbors)) continue;

                foreach (var neighborNode in neighbors.Keys)
                {
                    // 隣接ノードが直接の親ならスキップ
                    if (parentMap.TryGetValue(currentNode, out int parent) && neighborNode == parent)
                        continue;

                    // 隣接ノードが現在のコンポーネントのBFSで既に訪問されている場合...
                    if (currentComponentVisited.Contains(neighborNode))
                    {
                        // ...そしてそれが直接の親でない場合、バックエッジが見つかりました -> サイクル検出！
                        return true;
                    }

                    // 隣接ノードがグローバルにまだ訪問されていない場合、それを探索
                    if (!globallyVisited.Contains(neighborNode))
                    {
                        globallyVisited.Add(neighborNode);
                        currentComponentVisited.Add(neighborNode);
                        parentMap[neighborNode] = currentNode; // 親を記録
                        queue.Enqueue(neighborNode);
                    }
                    // 隣接ノードがグローバルに訪問済みだがcurrentComponentVisitedにない場合、
                    // それは以前に探索されたコンポーネントに属します（ここにはサイクルはありません）。
                }
            }
            // サイクルを見つけずにこのコンポーネントの探索を終了
        }

        return false; // どのコンポーネントにもサイクルは見つかりませんでした
    }

    /// <summary>
    /// グラフ内に現在存在するすべての頂点IDを取得します。
    /// </summary>
    /// <returns>すべての頂点IDを含む読み取り専用コレクション。</returns>
    public IReadOnlyCollection<int> GetVertices()
    {
        // .Keysはライブビューを提供しますが、これは通常は問題ありません。ToList().AsReadOnly()はスナップショットを作成します。
        // 要件に応じて適切なものを選択してください。スナップショットが不要な場合はKeysの方が効率的です。
        return _adjacencyList.Keys;
        // 代替: return _adjacencyList.Keys.ToList().AsReadOnly();
    }


    // === プライベートヘルパーメソッド ===

    /// <summary>
    /// 頂点がまだ存在しない場合にグラフに追加します。
    /// 内部ヘルパーメソッド。頂点IDが呼び出し側によって非負として検証されていることを前提とします。
    /// </summary>
    /// <param name="vertex">追加する頂点のID。</param>
    private void AddVertexIfNotExists(int vertex)
    {
        // 内部の一貫性のためのアサーション（パブリックメソッドによってチェックされているはず）
        // System.Diagnostics.Debug.Assert(vertex >= 0, "Vertex ID must be non-negative.");

        // 簡潔さのためにTryAddを使用、キーが既に存在する場合は何もしません。
        _adjacencyList.TryAdd(vertex, new Dictionary<int, long>());
        // 以下と同等:
        // if (!_adjacencyList.ContainsKey(vertex))
        // {
        //     _adjacencyList.Add(vertex, new Dictionary<int, long>());
        // }
    }
}