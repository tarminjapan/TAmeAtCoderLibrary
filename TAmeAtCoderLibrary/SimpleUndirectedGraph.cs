#nullable enable

namespace TAmeAtCoderLibrary;

/// <summary>
/// 頂点が非負整数で識別され、辺が長整数の重みを持つことができる単純無向グラフを表します。
/// このグラフは自己ループを許可しません。
/// このクラスは SimpleDirectedGraph を継承し、無向グラフのセマンティクスを提供します。
/// 重みが指定されない場合、デフォルトで 1 が設定されます。
/// </summary>
public class SimpleUndirectedGraph : SimpleDirectedGraph
{
    /// <summary>
    /// 空の単純無向グラフを初期化します。
    /// </summary>
    public SimpleUndirectedGraph() : base() { }

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
    public SimpleUndirectedGraph(int maxVertexId, int[][] edges) : base()
    {
        if (maxVertexId < 0)
            throw new ArgumentOutOfRangeException(nameof(maxVertexId), "Maximum vertex ID cannot be negative.");

        // 1からmaxVertexIdまでの頂点を事前に追加
        for (int vertexId = 1; vertexId <= maxVertexId; vertexId++)
        {
            base.AddVertex(vertexId);
        }

        // 提供された辺を追加
        this.AddEdges(edges);
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
    public SimpleUndirectedGraph(int[][] edges) : base()
    {
        this.AddEdges(edges);
    }

    /// <summary>
    /// グラフ内の現在の辺の数を取得します。
    /// 自己ループはカウントされません。各辺は1回だけカウントされます。
    /// </summary>
    public new int EdgeCount => base.EdgeCount / 2; // 基底クラスの有向辺カウントの半分

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
    public new void AddEdges(int[][] edges) // 基底クラスにも同名のメソッドがあるため new で隠蔽
    {
        ArgumentNullException.ThrowIfNull(edges);

        foreach (var edge in edges)
        {
            if (edge == null || edge.Length < 2)
                throw new ArgumentException("Each edge array must contain at least two vertex IDs.", nameof(edges));
            if (edge.Length > 3)
                throw new ArgumentException("Undirected edge array should be {vertexA, vertexB} or {vertexA, vertexB, weight}.", nameof(edges));

            int vertexA = edge[0];
            int vertexB = edge[1];
            long weight = (edge.Length == 3) ? edge[2] : 1L;

            this.AddEdge(vertexA, vertexB, weight);
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
    public new void AddEdge(int vertexA, int vertexB) // 基底クラスにも同名のメソッドがあるため new で隠蔽
    {
        this.AddEdge(vertexA, vertexB, 1L);
    }

    /// <summary>
    /// 指定された重み付きの辺を2つの頂点間に追加します。
    /// 頂点が存在しない場合は自動的に追加されます。
    /// 辺が既に存在する場合、または自己ループになる場合、このメソッドは何もしません。
    /// 重みを更新したい場合は <see cref="AddOrUpdateEdge"/> を使用してください。
    /// </summary>
    /// <param name="vertexA">最初の頂点のID。非負でなければなりません。</param>
    /// <param name="vertexB">2番目の頂点のID。非負でなければなりません。</param>
    /// <param name="weight">辺の重み。</param>
    /// <exception cref="ArgumentOutOfRangeException">任意の頂点IDが負の場合にスローされます。</exception>
    public void AddEdge(int vertexA, int vertexB, long weight) // 基底クラスにこのシグネチャはないため new は不要
    {
        if (vertexA == vertexB) return; // 自己ループは許可しない

        // 基底クラスの TryAddEdge は頂点/辺の存在チェックと追加を行う。
        // 無向グラフなので、両方向の辺を追加。
        // EdgeCount は基底クラスで管理され、成功時にインクリメントされる。
        base.TryAddEdge(vertexA, vertexB, weight);
        base.TryAddEdge(vertexB, vertexA, weight);
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
    public new void AddOrUpdateEdge(int vertexA, int vertexB, long weight) // 基底クラスにも同名のメソッドがあるため new で隠蔽
    {
        if (vertexA == vertexB) return; // 自己ループは許可しない

        // 基底クラスの AddOrUpdateEdge は頂点の追加と辺の追加/更新を行う。
        // 無向グラフなので、両方向の辺を追加/更新。
        // EdgeCount は基底クラスで管理され、新しい辺が追加された場合にインクリメントされる。
        base.AddOrUpdateEdge(vertexA, vertexB, weight);
        base.AddOrUpdateEdge(vertexB, vertexA, weight);
    }

    /// <summary>
    /// 頂点とそれに接続するすべての辺をグラフから削除します。
    /// 注意: この操作は頂点に接続する辺を削除しますが、基底クラスの制約により、
    /// 頂点自体（キー）は隣接リストに残る可能性があります（辺を持たない状態で）。
    /// そのため、<see cref="SimpleDirectedGraph.VertexCount"/> は減少しません。
    /// </summary>
    /// <param name="vertex">削除する頂点のID。</param>
    /// <returns>頂点が見つかり、その辺が削除された場合はtrue、頂点が存在しなかった場合はfalse。</returns>
    public bool RemoveVertex(int vertex)
    {
        // 基底クラスの _adjacencyList が private なので直接キーを削除できない。
        // 接続されている辺をすべて削除することで対応する。
        if (!base.ContainsVertex(vertex))
            return false;

        // ToList() でコピーを作成し、ループ中の変更問題を回避
        var neighbors = base.GetNeighbors(vertex).ToList();

        // 接続されている辺を両方向から削除
        foreach (var neighbor in neighbors)
        {
            this.RemoveEdge(vertex, neighbor); // 内部で基底クラスの RemoveEdge を両方向呼び出す
        }

        // 頂点キーは基底クラスに残るが、次数は 0 になる。
        // VertexCount は基底クラス管理のため、この操作では減らない。
        return true;
    }

    /// <summary>
    /// 2つの頂点間の辺を削除します。
    /// </summary>
    /// <param name="vertexA">最初の頂点のID。</param>
    /// <param name="vertexB">2番目の頂点のID。</param>
    /// <returns>辺が見つかり両方向から削除された場合はtrue、それ以外の場合はfalse。</returns>
    public new bool RemoveEdge(int vertexA, int vertexB) // 基底クラスにも同名のメソッドがあるため new で隠蔽
    {
        if (vertexA == vertexB) return false;

        // 基底クラスの RemoveEdge を両方向で呼び出す。
        // EdgeCount は基底クラスで管理され、成功時にデクリメントされる。
        bool removedA = base.RemoveEdge(vertexA, vertexB);
        bool removedB = base.RemoveEdge(vertexB, vertexA);

        // 無向グラフの辺が存在した場合、両方の削除が成功するはず。
        return removedA && removedB;
    }

    /// <summary>
    /// 指定された頂点の次数（接続辺の数）を取得します。
    /// </summary>
    /// <param name="vertex">頂点のID。</param>
    /// <returns>頂点の次数。頂点が存在しない場合は0。</returns>
    public int GetDegree(int vertex)
    {
        // 無向グラフの次数は、基底クラスの出次数と同じ (辺の追加/削除を両方向で行うため)。
        return base.GetOutDegree(vertex);
    }
    /// <summary>
    /// グラフ内の葉（次数0または1の頂点）であるすべての頂点を取得します。
    /// 孤立頂点（次数0）も葉と見なされます。
    /// </summary>
    /// <returns>すべての葉頂点のIDを含むハッシュセット。</returns>
    public HashSet<int> GetLeaves()
    {
        var leaves = new HashSet<int>();
        foreach (var vertex in base.GetVertices())
        {
            if (this.GetDegree(vertex) <= 1)
            {
                leaves.Add(vertex);
            }
        }
        return leaves;
    }

    /// <summary>
    /// 幅優先探索（BFS）を使用してグラフにサイクルが含まれているかどうかを確認します。
    /// 各コンポーネントをチェックすることで非連結グラフを処理します。
    /// これは無向グラフ用のサイクル検出アルゴリズムです。
    /// </summary>
    /// <returns>サイクルが検出された場合はtrue、それ以外の場合はfalse。</returns>
    public new bool ContainsCycle() // 基底クラスの有向グラフ用サイクル検出とは異なるため new で隠蔽
    {
        var globallyVisited = new HashSet<int>();
        var parentMap = new Dictionary<int, int>(); // BFSでの親を記録

        foreach (var startVertex in base.GetVertices())
        {
            if (globallyVisited.Contains(startVertex))
                continue; // この連結成分は既に探索済み

            var queue = new Queue<int>();

            queue.Enqueue(startVertex);
            globallyVisited.Add(startVertex);
            parentMap[startVertex] = -1; // 開始ノードの親は存在しない

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();

                foreach (var neighborNode in base.GetNeighbors(currentNode))
                {
                    // BFSで来た道を直接戻るのはサイクルではない
                    if (parentMap.TryGetValue(currentNode, out int parent) && neighborNode == parent)
                        continue;

                    // 訪問済みで親ノードでない隣接ノードはサイクルを意味する (無向グラフ)
                    if (globallyVisited.Contains(neighborNode))
                    {
                        return true;
                    }

                    // 未訪問の隣接ノードを訪問
                    globallyVisited.Add(neighborNode);
                    parentMap[neighborNode] = currentNode;
                    queue.Enqueue(neighborNode);
                }
            }
        }
        return false; // サイクルは見つからなかった
    }
}