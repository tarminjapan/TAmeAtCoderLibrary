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
    private readonly List<long[]> _Matrix = new List<long[]>();
    /// <summary>
    /// 剰余演算用の除数（モジュラス）。
    /// 0 以上の値が設定されている場合、すべての和と更新は、この除数で剰余をとった値になります。
    /// -1 の場合は、剰余演算は適用されません。
    /// </summary>
    private long _Divisor = -1L;

    /// <summary>
    /// <see cref="BinaryIndexedTree"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="maxKey">このツリーで使用できる最大キー（インデックス）。
    /// ツリーは 1 から maxKey（両端を含む）までのキーを収容できるようにサイズが設定されます。
    /// 注意: キー 0 も使用可能で、GetSum(0) または GetValue(0) を呼び出すと 0 を返します。
    /// </param>
    /// <remarks>
    /// ツリーの構造は、範囲を繰り返し 2 で割るという考え方に基づいて構築されています。
    /// _Matrix の各層は、キーのバイナリ表現によって定義された特定の範囲の和を格納します。
    /// </remarks>
    public BinaryIndexedTree(int maxKey)
    {
        var harf = Utilities.Common.Ceiling(maxKey, 2);
        _Matrix.Add(new long[harf]);
        maxKey = harf;

        do
        {
            harf = Utilities.Common.Ceiling(maxKey, 2);
            _Matrix.Add(new long[harf]);
            maxKey = harf;
        } while (2L <= maxKey);
    }

    /// <summary>
    /// 剰余演算用の除数を指定して、<see cref="BinaryIndexedTree"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="maxKey">このツリーで使用できる最大キー（インデックス）。</param>
    /// <param name="divisor">剰余演算に使用する除数。</param>
    /// <remarks>
    /// 特定の数で剰余演算を行う必要がある場合は、このコンストラクターを使用してください。
    /// 各更新と和の計算は、指定された除数で剰余をとった値になります。
    /// </remarks>
    public BinaryIndexedTree(int maxKey, long divisor)
    {
        var harf = Utilities.Common.Ceiling(maxKey, 2);
        _Matrix.Add(new long[harf]);
        maxKey = harf;

        do
        {
            harf = Utilities.Common.Ceiling(maxKey, 2);
            _Matrix.Add(new long[harf]);
            maxKey = harf;
        } while (2L <= maxKey);

        _Divisor = divisor;
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
    public void AddValue(int key, long value)
    {
        for (int i = 0; i < _Matrix.Count; i++)
        {
            var temp = Utilities.Common.Ceiling(key, 2);
            if (key % 2 == 1)
            {
                _Matrix[i][temp - 1] += value;

                if (_Divisor != -1)
                {
                    _Matrix[i][temp - 1] += _Divisor;
                    _Matrix[i][temp - 1] %= _Divisor;
                }
            }

            key = temp;
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
    public long GetSum(int key)
    {
        if (key == 0)
            return 0L;

        var value = 0L;

        for (int i = 0; i < _Matrix.Count; i++)
        {
            if (key % 2 == 1)
            {
                value += _Matrix[i][Utilities.Common.Ceiling(key, 2) - 1];

                if (_Divisor != -1)
                {
                    value += _Divisor;
                    value %= _Divisor;
                }
            }

            if (key == 1)
                break;

            key /= 2;
        }

        return value;
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
    public long GetValue(int key)
    {
        var value = GetSum(key) - GetSum(key - 1);

        if (_Divisor != -1)
        {
            value += _Divisor;
            value %= _Divisor;
        }

        return value;
    }
}
