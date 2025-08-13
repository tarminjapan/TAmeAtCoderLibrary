namespace TAmeAtCoderLibrary;

/// <summary>
/// Binary Indexed Tree (Fenwick Tree) の実装。区間和の取得と一点更新を効率的に行うことができます。
/// </summary>
public class BinaryIndexedTree
{
    private readonly long[] _tree;
    private readonly int _size;
    private readonly long? _mod;

    /// <summary>
    /// <see cref="BinaryIndexedTree"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="size">要素の数。0からsize-1までのインデックスをサポートします。</param>
    public BinaryIndexedTree(int size)
    {
        if (size < 0) throw new ArgumentOutOfRangeException(nameof(size));
        _size = size;
        _tree = new long[size + 1];
        _mod = null;
    }

    /// <summary>
    /// 剰余演算を行う<see cref="BinaryIndexedTree"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="size">要素の数。0からsize-1までのインデックスをサポートします。</param>
    /// <param name="mod">剰余演算の法。</param>
    public BinaryIndexedTree(int size, long mod)
    {
        if (size < 0) throw new ArgumentOutOfRangeException(nameof(size));
        if (mod <= 0) throw new ArgumentOutOfRangeException(nameof(mod));
        _size = size;
        _tree = new long[size + 1];
        _mod = mod;
    }

    /// <summary>
    /// 指定したインデックスに値を追加します。
    /// </summary>
    /// <param name="index">0-basedのインデックス。</param>
    /// <param name="value">追加する値。</param>
    public void Add(int index, long value)
    {
        if (index < 0 || index >= _size) throw new ArgumentOutOfRangeException(nameof(index));
        index++; // 1-basedに変換
        while (index <= _size)
        {
            _tree[index] += value;
            if (_mod.HasValue)
            {
                _tree[index] %= _mod.Value;
                if (_tree[index] < 0) _tree[index] += _mod.Value;
            }
            index += index & -index;
        }
    }

    /// <summary>
    /// [0, index] の区間和を求めます。
    /// </summary>
    /// <param name="index">0-basedのインデックス。</param>
    /// <returns>[0, index]の合計値。</returns>
    public long Sum(int index)
    {
        if (index < 0 || index >= _size) throw new ArgumentOutOfRangeException(nameof(index));
        long sum = 0;
        index++; // 1-basedに変換
        while (index > 0)
        {
            sum += _tree[index];
            if (_mod.HasValue)
            {
                sum %= _mod.Value;
                if (sum < 0) sum += _mod.Value;
            }
            index -= index & -index;
        }
        return sum;
    }

    /// <summary>
    /// [from, to] の区間和を求めます。
    /// </summary>
    /// <param name="from">0-basedの開始インデックス。</param>
    /// <param name="to">0-basedの終了インデックス。</param>
    /// <returns>[from, to]の合計値。</returns>
    public long Sum(int from, int to)
    {
        if (from > to) throw new ArgumentException("'from' must be less than or equal to 'to'.");
        if (from < 0) throw new ArgumentOutOfRangeException(nameof(from));
        if (to >= _size) throw new ArgumentOutOfRangeException(nameof(to));

        long sumTo = Sum(to);
        long sumFrom = (from == 0) ? 0 : Sum(from - 1);

        long result = sumTo - sumFrom;

        if (_mod.HasValue)
        {
            result %= _mod.Value;
            if (result < 0) result += _mod.Value;
        }
        return result;
    }
}
