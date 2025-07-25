namespace TAmeAtCoderLibrary;

/// <summary>
/// Binary Indexed Tree (Fenwick Tree) の実装。区間和の取得と一点更新を効率的に行うことができます。
/// インデックスは0から始まります。
/// </summary>
public class BinaryIndexedTree
{
    private readonly List<long[]> _Layers = new List<long[]>();
    private readonly int _size;
    private long _Modulus = -1L;

    /// <summary>
    /// <see cref="BinaryIndexedTree"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="count">要素の個数。0からcount-1までのインデックスをサポートします。</param>
    public BinaryIndexedTree(int count)
    {
        if (count <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be positive.");
        }
        _size = count;
        InitializeLayers(_size);
    }

    /// <summary>
    /// 剰余演算用の除数を指定して、<see cref="BinaryIndexedTree"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="count">要素の個数。0からcount-1までのインデックスをサポートします。</param>
    /// <param name="divisor">剰余演算に使用する除数。0より大きな値である必要があります。</param>
    public BinaryIndexedTree(int count, long divisor) : this(count)
    {
        if (divisor <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(divisor), "Divisor must be greater than zero.");
        }
        _Modulus = divisor;
    }

    private void InitializeLayers(int size)
    {
        if (size <= 0) return;

        var currentSize = size;
        do
        {
            var half = (int)MathEx.Ceiling(currentSize, 2);
            _Layers.Add(new long[half]);
            currentSize = half;
        } while (2L <= currentSize);
    }

    private long ApplyModulo(long value)
    {
        if (_Modulus != -1)
        {
            value %= _Modulus;
            if (value < 0)
            {
                value += _Modulus;
            }
        }
        return value;
    }

    /// <summary>
    /// ツリー内の特定のキーに値を追加します。
    /// </summary>
    /// <param name="key">更新するキー（インデックス）。0 から count - 1 の範囲である必要があります。</param>
    /// <param name="value">キーに追加する値。</param>
    public void AddValue(int key, long value)
    {
        if (key < 0 || key >= _size)
        {
            throw new ArgumentOutOfRangeException(nameof(key), "Key is out of the valid range.");
        }
        key++; // 1-basedに変換

        for (int i = 0; i < _Layers.Count; i++)
        {
            var nextLevelKey = (int)MathEx.Ceiling(key, 2);
            if (key % 2 == 1)
            {
                _Layers[i][nextLevelKey - 1] = ApplyModulo(_Layers[i][nextLevelKey - 1] + value);
            }
            key = nextLevelKey;
        }
    }

    /// <summary>
    /// キー 0 から指定されたキーまでのすべての値の合計を取得します。
    /// </summary>
    /// <param name="key">範囲の上限（両端を含む）。</param>
    /// <returns>キー 0 から指定されたキーまでの値の合計。</returns>
    public long GetSum(int key)
    {
        if (key >= _size)
        {
            throw new ArgumentOutOfRangeException(nameof(key), "Key is out of the valid range.");
        }
        if (key < 0)
        {
            return 0L;
        }

        key++; // 1-basedに変換

        var sum = 0L;
        for (int i = 0; i < _Layers.Count; i++)
        {
            if (key % 2 == 1)
            {
                sum = ApplyModulo(sum + _Layers[i][(int)MathEx.Ceiling(key, 2) - 1]);
            }
            if (key == 1) break;
            key /= 2;
        }
        return sum;
    }

    /// <summary>
    /// 指定された範囲 [from, to] の合計を取得します。
    /// </summary>
    /// <param name="from">範囲の下限（両端を含む）。</param>
    /// <param name="to">範囲の上限（両端を含む）。</param>
    /// <returns>指定された範囲の値の合計。</returns>
    public long GetSum(int from, int to)
    {
        if (from > to)
        {
            throw new ArgumentException("from must be less than or equal to to.");
        }
        if (from < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(from), "from is out of the valid range.");
        }
        return ApplyModulo(GetSum(to) - GetSum(from - 1));
    }

    /// <summary>
    /// 特定のキーの値を取得します。
    /// </summary>
    /// <param name="key">値を取得するキー（インデックス）。</param>
    /// <returns>指定されたキーの値。</returns>
    public long GetValue(int key)
    {
        if (key < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(key), "Key is out of the valid range.");
        }
        return ApplyModulo(GetSum(key) - GetSum(key - 1));
    }
}
