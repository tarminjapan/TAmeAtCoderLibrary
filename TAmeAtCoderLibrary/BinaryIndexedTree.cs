namespace TAmeAtCoderLibrary;

/// <summary>
/// Binary Indexed Tree (Fenwick Tree) の実装。区間和の取得と一点更新を効率的に行うことができます。
/// この実装では、階層的な方法で更新とクエリを処理するために、行列のような構造を使用しています。
/// </summary>
public class BinaryIndexedTree
{
    /// <summary>
    /// Binary Indexed Tree を表す内部データ構造。
    /// List の各要素はツリーの層を表し、各層は long 型の配列です。
    /// </summary>
    private readonly List<long[]> _Layers = new List<long[]>();
    /// <summary>
    /// 剰余演算用の除数（モジュラス）。
    /// 0 以上の値が設定されている場合、すべての和と更新は、この除数で剰余をとった値になります。
    /// -1 の場合は、剰余演算は適用されません。
    /// </summary>
    private long _Modulus = -1L;

    /// <summary>
    /// <see cref="BinaryIndexedTree"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="maxKey">このツリーで使用できる最大キー（インデックス）。
    /// ツリーは 1 から maxKey（両端を含む）までのキーを収容できるようにサイズが設定されます。
    /// 注意: キー 0 も使用可能で、GetSum(0) または GetValue(0) を呼び出すと 0 を返します。
    /// </param>
    /// <remarks>
    /// ツリーの構造は、範囲を繰り返し 2 で割るという考え方に基づいて構築されています。
    /// _Layers の各層は、キーのバイナリ表現によって定義された特定の範囲の和を格納します。
    /// </remarks>
    public BinaryIndexedTree(int maxKey)
    {
        InitializeLayers(maxKey);
    }

    /// <summary>
    /// 剰余演算用の除数を指定して、<see cref="BinaryIndexedTree"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="maxKey">このツリーで使用できる最大キー（インデックス）。</param>
    /// <param name="divisor">剰余演算に使用する除数。0より大きな値である必要があります。</param>
    /// <remarks>
    /// 特定の数で剰余演算を行う必要がある場合は、このコンストラクターを使用してください。
    /// 各更新と和の計算は、指定された除数で剰余をとった値になります。
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">divisorが0以下の場合にスローされます。</exception>
    public BinaryIndexedTree(int maxKey, long divisor)
    {
        if (divisor <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(divisor), "Divisor must be greater than zero.");
        }

        InitializeLayers(maxKey);
        _Modulus = divisor;
    }

    /// <summary>
    /// Binary Indexed Treeの層構造を初期化します。
    /// </summary>
    /// <param name="maxKey">最大キー値</param>
    private void InitializeLayers(int maxKey)
    {
        if (maxKey < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxKey), "Maximum key must be non-negative.");
        }

        var half = Utilities.Common.CeilingDivide(maxKey, 2);
        _Layers.Add(new long[half]);
        maxKey = half;

        do
        {
            half = Utilities.Common.CeilingDivide(maxKey, 2);
            _Layers.Add(new long[half]);
            maxKey = half;
        } while (2L <= maxKey);
    }

    /// <summary>
    /// 値にモジュロ演算を適用します（必要な場合）。
    /// </summary>
    /// <param name="value">モジュロ演算を適用する値</param>
    /// <returns>モジュロ演算後の値</returns>
    private long ApplyModulo(long value)
    {
        if (_Modulus != -1)
        {
            // 負の値の場合にのみ、除数を加えて正の数にする
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
    /// <param name="key">更新するキー（インデックス）。1 から maxKey（両端を含む）の範囲である必要があります。</param>
    /// <param name="value">キーに追加する値。</param>
    /// <remarks>
    /// このメソッドは、指定されたキーを含むツリー内のすべての関連する範囲を更新します。
    /// 除数が設定されている場合、更新は除数で剰余をとった値になります。
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">keyが0以下の場合、または配列の範囲を超える場合にスローされます。</exception>
    public void AddValue(int key, long value)
    {
        if (key <= 0 || key >= _Layers[0].Length * 2)
        {
            throw new ArgumentOutOfRangeException(nameof(key), "Key is out of the valid range.");
        }

        for (int i = 0; i < _Layers.Count; i++)
        {
            var nextLevelKey = Utilities.Common.CeilingDivide(key, 2);
            if (key % 2 == 1)
            {
                _Layers[i][nextLevelKey - 1] += value;
                _Layers[i][nextLevelKey - 1] = ApplyModulo(_Layers[i][nextLevelKey - 1]);
            }

            key = nextLevelKey;
        }
    }

    /// <summary>
    /// キー 1 から指定されたキーまでのすべての値の合計を取得します。
    /// </summary>
    /// <param name="key">範囲の上限（両端を含む）。</param>
    /// <returns>キー 1 から指定されたキーまでの値の合計。</returns>
    /// <remarks>
    /// このメソッドは、Binary Indexed Tree のプロパティを使用して、範囲の和を効率的に計算します。
    /// 除数が設定されている場合、和は除数で剰余をとった値になります。
    /// GetSum(0) は常に 0 を返します。
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">keyが0未満の場合、または配列の範囲を超える場合にスローされます。</exception>
    public long GetSum(int key)
    {
        if (key < 0 || key >= _Layers[0].Length * 2)
        {
            throw new ArgumentOutOfRangeException(nameof(key), "Key is out of the valid range.");
        }

        if (key == 0)
            return 0L;

        var sum = 0L;

        for (int i = 0; i < _Layers.Count; i++)
        {
            if (key % 2 == 1)
            {
                sum += _Layers[i][Utilities.Common.CeilingDivide(key, 2) - 1];
                sum = ApplyModulo(sum);
            }

            if (key == 1)
                break;

            key /= 2;
        }

        return sum;
    }

    /// <summary>
    /// 特定のキーの値を取得します。
    /// </summary>
    /// <param name="key">値を取得するキー（インデックス）。</param>
    /// <returns>指定されたキーの値。</returns>
    /// <remarks>
    /// このメソッドは、現在のキーまでの和から前のキーまでの和を減算することで、指定されたキーの値を計算します。
    /// 除数が設定されている場合、結果は除数で剰余をとった値になります。
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">keyが0以下の場合、または配列の範囲を超える場合にスローされます。</exception>
    public long GetValue(int key)
    {
        if (key <= 0 || key >= _Layers[0].Length * 2)
        {
            throw new ArgumentOutOfRangeException(nameof(key), "Key is out of the valid range.");
        }

        var value = GetSum(key) - GetSum(key - 1);
        return ApplyModulo(value);
    }
}
