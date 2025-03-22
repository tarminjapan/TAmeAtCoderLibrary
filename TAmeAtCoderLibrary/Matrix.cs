namespace TAmeAtCoderLibrary;

/// <summary>
/// 行列操作を提供する汎用クラスです。2次元配列の作成や操作をサポートします。
/// </summary>
/// <typeparam name="T">行列の要素の型</typeparam>
public static class Matrix<T>
{
    /// <summary>
    /// 指定された高さと幅を持つ2次元配列を作成します。
    /// </summary>
    /// <param name="height">行列の高さ（行数）</param>
    /// <param name="width">行列の幅（列数）</param>
    /// <returns>初期化された2次元配列</returns>
    public static T[][] Create(int height, int width)
    {
        var matrix = new T[height][];

        for (int i = 0; i < height; i++)
            matrix[i] = new T[width];

        return matrix;
    }

    /// <summary>
    /// 指定された高さ、幅、およびデフォルト値を持つ2次元配列を作成します。
    /// </summary>
    /// <param name="height">行列の高さ（行数）</param>
    /// <param name="width">行列の幅（列数）</param>
    /// <param name="defalutValue">行列のすべての要素に設定するデフォルト値</param>
    /// <returns>デフォルト値で初期化された2次元配列</returns>
    public static T[][] Create(int height, int width, T defalutValue)
    {
        var matrix = new T[height][];

        for (int i = 0; i < height; i++)
        {
            matrix[i] = new T[width];

            for (int w = 0; w < width; w++)
                matrix[i][w] = defalutValue;
        }

        return matrix;
    }

    /// <summary>
    /// 指定された高さを持つ辞書を作成します。各キーに対して空のHashSetを割り当てます。
    /// </summary>
    /// <param name="height">作成する辞書のキー数</param>
    /// <returns>整数をキーとし、HashSet&lt;T&gt;を値とする辞書</returns>
    public static Dictionary<int, HashSet<T>> CreateDictionary(int height)
    {
        var dic = new Dictionary<int, HashSet<T>>();

        for (int h = 0; h < height; h++)
            dic.Add(h, new HashSet<T>());

        return dic;
    }

    /// <summary>
    /// 指定された行列の深いコピー（クローン）を作成します。
    /// </summary>
    /// <param name="matrix">クローンする元の行列</param>
    /// <returns>元の行列のコピー</returns>
    public static T[][] Clone(T[][] matrix)
    {
        var result = Create(matrix.Length, matrix[0].Length);

        for (int h = 0; h < matrix.Length; h++)
            for (int w = 0; w < matrix[0].Length; w++)
                result[h][w] = matrix[h][w];

        return result;
    }
}