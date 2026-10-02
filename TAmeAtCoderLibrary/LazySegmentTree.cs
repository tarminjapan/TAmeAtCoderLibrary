namespace TAmeAtCoderLibrary;

/// <summary>
/// 遅延評価セグメント木：指定された範囲に対する一括更新（加算・代入）と、
/// 範囲に対するクエリ（合計、最大、最小）を効率的に実行できるデータ構造を提供します。
/// 区間更新・区間クエリのいずれもO(log N)で行えます。
/// </summary>
/// <remarks>
/// <para>範囲の指定は <see cref="SegmentTree"/> と同じく両端を含む閉区間 [left, right] です。</para>
/// <para>
/// 使用例：
/// <code>
/// var seg = new LazySegmentTree(new long[] { 1, 2, 3, 4, 5 });
/// seg.Add(1, 3, 10);            // a[1]～a[3] に 10 を加算
/// seg.SetValue(0, 2, 7);        // a[0]～a[2] を 7 で上書き
/// long sum = seg.GetSum(0, 4);  // 区間合計
/// long max = seg.GetMax(0, 4);  // 区間最大値
/// </code>
/// </para>
/// </remarks>
public class LazySegmentTree
{
    private const int RootNode = 1;
    private const int DefaultMinIndex = 0; // 通常は0固定

    private readonly int _size;
    private readonly long[] _sum;        // ノードが担当する範囲の合計
    private readonly long[] _max;        // ノードが担当する範囲の最大値
    private readonly long[] _min;        // ノードが担当する範囲の最小値
    private readonly long[] _lazyAdd;    // 子へ未反映の加算値
    private readonly long[] _lazyAssign; // 子へ未反映の代入値
    private readonly bool[] _hasAssign;  // 子へ未反映の代入があるか

    /// <summary>
    /// セグメント木が管理する要素の数を取得します。
    /// </summary>
    public int Size => _size;

    /// <summary>
    /// セグメント木のインデックスの開始値を取得します（通常は0）。
    /// </summary>
    public static int MinIndex => DefaultMinIndex;

    /// <summary>
    /// 指定されたインデックスの値を取得・設定します。
    /// </summary>
    /// <param name="index">0から始まるインデックス。</param>
    public long this[int index]
    {
        get => GetValue(index);
        set => SetValue(index, value);
    }

    /// <summary>
    /// すべての要素を0で初期化した遅延評価セグメント木を構築します。
    /// </summary>
    /// <param name="size">セグメント木で管理する要素の総数。1以上の値を指定してください。</param>
    /// <exception cref="ArgumentOutOfRangeException">sizeが0以下の場合にスローされます。</exception>
    public LazySegmentTree(int size) : this(new long[ValidateSize(size)]) { }

    /// <summary>
    /// 指定された配列の内容で遅延評価セグメント木を構築します。
    /// </summary>
    /// <param name="values">初期値の配列。要素数は1以上である必要があります。</param>
    /// <exception cref="ArgumentNullException">valuesがnullの場合にスローされます。</exception>
    /// <exception cref="ArgumentOutOfRangeException">valuesが空の場合にスローされます。</exception>
    public LazySegmentTree(IReadOnlyList<long> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        _size = ValidateSize(values.Count);

        // 再帰的な2分割で必要となるノード数は 4N で十分。
        int nodeCount = 4 * _size;
        _sum = new long[nodeCount];
        _max = new long[nodeCount];
        _min = new long[nodeCount];
        _lazyAdd = new long[nodeCount];
        _lazyAssign = new long[nodeCount];
        _hasAssign = new bool[nodeCount];

        Build(RootNode, DefaultMinIndex, _size - 1, values);
    }

    /// <summary>
    /// 指定されたインデックスの値に加算します。
    /// </summary>
    /// <param name="index">値を加算するインデックス（0から始まる）。</param>
    /// <param name="value">加算する値。</param>
    /// <exception cref="ArgumentOutOfRangeException">indexが範囲外の場合にスローされます。</exception>
    public void Add(int index, long value)
    {
        ValidateIndex(index);
        AddRange(RootNode, DefaultMinIndex, _size - 1, index, index, value);
    }

    /// <summary>
    /// 指定された範囲 [left, right] (両端含む) のすべての要素に加算します。
    /// </summary>
    /// <param name="left">範囲の左端インデックス（含む）。</param>
    /// <param name="right">範囲の右端インデックス（含む）。</param>
    /// <param name="value">加算する値。</param>
    /// <exception cref="ArgumentOutOfRangeException">leftまたはrightが範囲外の場合にスローされます。</exception>
    /// <exception cref="ArgumentException">leftがrightより大きい場合にスローされます。</exception>
    public void Add(int left, int right, long value)
    {
        ValidateRange(left, right);
        AddRange(RootNode, DefaultMinIndex, _size - 1, left, right, value);
    }

    /// <summary>
    /// 指定されたインデックスの値を新しい値で設定（上書き）します。
    /// </summary>
    /// <param name="index">値を設定するインデックス（0から始まる）。</param>
    /// <param name="value">設定する新しい値。</param>
    /// <exception cref="ArgumentOutOfRangeException">indexが範囲外の場合にスローされます。</exception>
    public void SetValue(int index, long value)
    {
        ValidateIndex(index);
        AssignRange(RootNode, DefaultMinIndex, _size - 1, index, index, value);
    }

    /// <summary>
    /// 指定された範囲 [left, right] (両端含む) のすべての要素を新しい値で設定（上書き）します。
    /// </summary>
    /// <param name="left">範囲の左端インデックス（含む）。</param>
    /// <param name="right">範囲の右端インデックス（含む）。</param>
    /// <param name="value">設定する新しい値。</param>
    /// <exception cref="ArgumentOutOfRangeException">leftまたはrightが範囲外の場合にスローされます。</exception>
    /// <exception cref="ArgumentException">leftがrightより大きい場合にスローされます。</exception>
    public void SetValue(int left, int right, long value)
    {
        ValidateRange(left, right);
        AssignRange(RootNode, DefaultMinIndex, _size - 1, left, right, value);
    }

    /// <summary>
    /// 指定されたインデックスの値を取得します。
    /// </summary>
    /// <param name="index">値を取得するインデックス（0から始まる）。</param>
    /// <returns>指定されたインデックスの値。</returns>
    /// <exception cref="ArgumentOutOfRangeException">indexが範囲外の場合にスローされます。</exception>
    public long GetValue(int index)
    {
        ValidateIndex(index);
        return QuerySum(RootNode, DefaultMinIndex, _size - 1, index, index);
    }

    /// <summary>
    /// 指定された範囲 [left, right] (両端含む) の合計値を取得します。
    /// </summary>
    /// <param name="left">範囲の左端インデックス（含む）。</param>
    /// <param name="right">範囲の右端インデックス（含む）。</param>
    /// <returns>指定された範囲の合計値。</returns>
    /// <exception cref="ArgumentOutOfRangeException">leftまたはrightが範囲外の場合にスローされます。</exception>
    /// <exception cref="ArgumentException">leftがrightより大きい場合にスローされます。</exception>
    public long GetSum(int left, int right)
    {
        ValidateRange(left, right);
        return QuerySum(RootNode, DefaultMinIndex, _size - 1, left, right);
    }

    /// <summary>
    /// 指定された範囲 [left, right] (両端含む) の最大値を取得します。
    /// </summary>
    /// <param name="left">範囲の左端インデックス（含む）。</param>
    /// <param name="right">範囲の右端インデックス（含む）。</param>
    /// <returns>指定された範囲の最大値。</returns>
    /// <exception cref="ArgumentOutOfRangeException">leftまたはrightが範囲外の場合にスローされます。</exception>
    /// <exception cref="ArgumentException">leftがrightより大きい場合にスローされます。</exception>
    public long GetMax(int left, int right)
    {
        ValidateRange(left, right);
        return QueryMax(RootNode, DefaultMinIndex, _size - 1, left, right);
    }

    /// <summary>
    /// 指定された範囲 [left, right] (両端含む) の最小値を取得します。
    /// </summary>
    /// <param name="left">範囲の左端インデックス（含む）。</param>
    /// <param name="right">範囲の右端インデックス（含む）。</param>
    /// <returns>指定された範囲の最小値。</returns>
    /// <exception cref="ArgumentOutOfRangeException">leftまたはrightが範囲外の場合にスローされます。</exception>
    /// <exception cref="ArgumentException">leftがrightより大きい場合にスローされます。</exception>
    public long GetMin(int left, int right)
    {
        ValidateRange(left, right);
        return QueryMin(RootNode, DefaultMinIndex, _size - 1, left, right);
    }

    // --- 内部処理 ---

    /// <summary>
    /// 初期値の配列から木を構築する。
    /// </summary>
    private void Build(int node, int nodeLeft, int nodeRight, IReadOnlyList<long> values)
    {
        if (nodeLeft == nodeRight) // 葉ノード
        {
            _sum[node] = _max[node] = _min[node] = values[nodeLeft];
            return;
        }

        int mid = (nodeLeft + nodeRight) / 2;
        Build(node * 2, nodeLeft, mid, values);
        Build(node * 2 + 1, mid + 1, nodeRight, values);
        UpdateAggregates(node);
    }

    /// <summary>
    /// 子ノードの値に基づいて、このノードの集計値 (Sum, Max, Min) を更新する。
    /// </summary>
    private void UpdateAggregates(int node)
    {
        _sum[node] = _sum[node * 2] + _sum[node * 2 + 1];
        _max[node] = Math.Max(_max[node * 2], _max[node * 2 + 1]);
        _min[node] = Math.Min(_min[node * 2], _min[node * 2 + 1]);
    }

    /// <summary>
    /// 指定ノードが担当する範囲全体に加算を反映し、加算を遅延情報として記録する。
    /// </summary>
    private void ApplyAdd(int node, int count, long value)
    {
        _sum[node] += value * count;
        _max[node] += value;
        _min[node] += value;
        _lazyAdd[node] += value;
    }

    /// <summary>
    /// 指定ノードが担当する範囲全体に代入を反映し、代入を遅延情報として記録する。
    /// </summary>
    private void ApplyAssign(int node, int count, long value)
    {
        _sum[node] = value * count;
        _max[node] = value;
        _min[node] = value;
        _lazyAssign[node] = value;
        _hasAssign[node] = true;
        _lazyAdd[node] = 0L; // 代入によって、それ以前の加算は打ち消される
    }

    /// <summary>
    /// 指定ノードに溜まっている遅延情報を、2つの子ノードへ伝播させる。
    /// 「代入してから加算」の順序で反映する。
    /// </summary>
    private void Propagate(int node, int nodeLeft, int nodeRight)
    {
        int mid = (nodeLeft + nodeRight) / 2;
        int leftCount = mid - nodeLeft + 1;
        int rightCount = nodeRight - mid;

        if (_hasAssign[node])
        {
            ApplyAssign(node * 2, leftCount, _lazyAssign[node]);
            ApplyAssign(node * 2 + 1, rightCount, _lazyAssign[node]);
            _hasAssign[node] = false;
        }

        if (_lazyAdd[node] != 0L)
        {
            ApplyAdd(node * 2, leftCount, _lazyAdd[node]);
            ApplyAdd(node * 2 + 1, rightCount, _lazyAdd[node]);
            _lazyAdd[node] = 0L;
        }
    }

    /// <summary>
    /// 指定範囲 [queryLeft, queryRight] に加算する。
    /// </summary>
    private void AddRange(int node, int nodeLeft, int nodeRight, int queryLeft, int queryRight, long value)
    {
        // 1. クエリ範囲がノード範囲と全く交差しない場合
        if (queryRight < nodeLeft || nodeRight < queryLeft) return;

        // 2. クエリ範囲がノード範囲を完全に含む場合
        if (queryLeft <= nodeLeft && nodeRight <= queryRight)
        {
            ApplyAdd(node, nodeRight - nodeLeft + 1, value);
            return;
        }

        // 3. それ以外（子へ降りる前に遅延情報を伝播させる）
        Propagate(node, nodeLeft, nodeRight);
        int mid = (nodeLeft + nodeRight) / 2;
        AddRange(node * 2, nodeLeft, mid, queryLeft, queryRight, value);
        AddRange(node * 2 + 1, mid + 1, nodeRight, queryLeft, queryRight, value);
        UpdateAggregates(node);
    }

    /// <summary>
    /// 指定範囲 [queryLeft, queryRight] に代入する。
    /// </summary>
    private void AssignRange(int node, int nodeLeft, int nodeRight, int queryLeft, int queryRight, long value)
    {
        if (queryRight < nodeLeft || nodeRight < queryLeft) return;

        if (queryLeft <= nodeLeft && nodeRight <= queryRight)
        {
            ApplyAssign(node, nodeRight - nodeLeft + 1, value);
            return;
        }

        Propagate(node, nodeLeft, nodeRight);
        int mid = (nodeLeft + nodeRight) / 2;
        AssignRange(node * 2, nodeLeft, mid, queryLeft, queryRight, value);
        AssignRange(node * 2 + 1, mid + 1, nodeRight, queryLeft, queryRight, value);
        UpdateAggregates(node);
    }

    /// <summary>
    /// 指定範囲 [queryLeft, queryRight] の合計値を取得する。
    /// </summary>
    private long QuerySum(int node, int nodeLeft, int nodeRight, int queryLeft, int queryRight)
    {
        if (queryRight < nodeLeft || nodeRight < queryLeft) return 0L; // 合計の単位元
        if (queryLeft <= nodeLeft && nodeRight <= queryRight) return _sum[node];

        Propagate(node, nodeLeft, nodeRight);
        int mid = (nodeLeft + nodeRight) / 2;
        return QuerySum(node * 2, nodeLeft, mid, queryLeft, queryRight)
             + QuerySum(node * 2 + 1, mid + 1, nodeRight, queryLeft, queryRight);
    }

    /// <summary>
    /// 指定範囲 [queryLeft, queryRight] の最大値を取得する。
    /// </summary>
    private long QueryMax(int node, int nodeLeft, int nodeRight, int queryLeft, int queryRight)
    {
        if (queryRight < nodeLeft || nodeRight < queryLeft) return long.MinValue; // 最大値の単位元
        if (queryLeft <= nodeLeft && nodeRight <= queryRight) return _max[node];

        Propagate(node, nodeLeft, nodeRight);
        int mid = (nodeLeft + nodeRight) / 2;
        return Math.Max(QueryMax(node * 2, nodeLeft, mid, queryLeft, queryRight),
                        QueryMax(node * 2 + 1, mid + 1, nodeRight, queryLeft, queryRight));
    }

    /// <summary>
    /// 指定範囲 [queryLeft, queryRight] の最小値を取得する。
    /// </summary>
    private long QueryMin(int node, int nodeLeft, int nodeRight, int queryLeft, int queryRight)
    {
        if (queryRight < nodeLeft || nodeRight < queryLeft) return long.MaxValue; // 最小値の単位元
        if (queryLeft <= nodeLeft && nodeRight <= queryRight) return _min[node];

        Propagate(node, nodeLeft, nodeRight);
        int mid = (nodeLeft + nodeRight) / 2;
        return Math.Min(QueryMin(node * 2, nodeLeft, mid, queryLeft, queryRight),
                        QueryMin(node * 2 + 1, mid + 1, nodeRight, queryLeft, queryRight));
    }

    /// <summary>
    /// 要素数が有効か検証する。
    /// </summary>
    private static int ValidateSize(int size)
    {
        if (size <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(size), "Size must be positive.");
        }
        return size;
    }

    /// <summary>
    /// 一点クエリのインデックスが有効か検証する。
    /// </summary>
    private void ValidateIndex(int index)
    {
        if (index < 0 || index >= _size)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"Index must be between 0 and {_size - 1}.");
        }
    }

    /// <summary>
    /// 範囲クエリの引数 (left, right) が有効か検証する。
    /// </summary>
    private void ValidateRange(int left, int right)
    {
        if (left < 0 || left >= _size || right < 0 || right >= _size)
        {
            throw new ArgumentOutOfRangeException($"Arguments left ({left}) or right ({right}) must be within the range [0, {_size - 1}].");
        }
        if (left > right)
        {
            throw new ArgumentException($"Left index ({left}) cannot be greater than right index ({right}).", nameof(left));
        }
    }
}
