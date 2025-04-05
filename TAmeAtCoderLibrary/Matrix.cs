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
    /// <exception cref="ArgumentException">高さまたは幅が0未満の場合にスローされます</exception>
    public static T[][] Create(int height, int width)
    {
        if (height < 0 || width < 0)
            throw new ArgumentException("高さと幅は0以上である必要があります");

        var matrix = new T[height][];

        for (int h = 0; h < height; h++)
            matrix[h] = new T[width];

        return matrix;
    }

    /// <summary>
    /// 指定された高さ、幅、およびデフォルト値を持つ2次元配列を作成します。
    /// </summary>
    /// <param name="height">行列の高さ（行数）</param>
    /// <param name="width">行列の幅（列数）</param>
    /// <param name="defaultValue">行列のすべての要素に設定するデフォルト値</param>
    /// <returns>デフォルト値で初期化された2次元配列</returns>
    /// <exception cref="ArgumentException">高さまたは幅が0未満の場合にスローされます</exception>
    public static T[][] Create(int height, int width, T defaultValue)
    {
        if (height < 0 || width < 0)
            throw new ArgumentException("高さと幅は0以上である必要があります");

        var matrix = new T[height][];

        for (int h = 0; h < height; h++)
        {
            matrix[h] = new T[width];

            for (int w = 0; w < width; w++)
                matrix[h][w] = defaultValue;
        }

        return matrix;
    }

    /// <summary>
    /// 指定された高さを持つ辞書を作成します。各キーに対して空のHashSetを割り当てます。
    /// </summary>
    /// <param name="height">作成する辞書のキー数</param>
    /// <returns>整数をキーとし、HashSet&lt;T&gt;を値とする辞書</returns>
    /// <exception cref="ArgumentException">高さが0未満の場合にスローされます</exception>
    public static Dictionary<int, HashSet<T>> CreateDictionary(int height)
    {
        if (height < 0)
            throw new ArgumentException("高さは0以上である必要があります");

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
    /// <exception cref="ArgumentNullException">行列がnullの場合にスローされます</exception>
    /// <exception cref="ArgumentException">行列が空または不規則な形状の場合にスローされます</exception>
    public static T[][] Clone(T[][] matrix)
    {
        if (matrix == null)
            throw new ArgumentNullException(nameof(matrix), "行列はnullであってはなりません");

        if (matrix.Length == 0)
            throw new ArgumentException("行列は少なくとも1行必要です", nameof(matrix));

        if (matrix[0] == null)
            throw new ArgumentException("行列の行はnullであってはなりません", nameof(matrix));

        int width = matrix[0].Length;
        var result = Create(matrix.Length, width);

        for (int h = 0; h < matrix.Length; h++)
        {
            if (matrix[h] == null)
                throw new ArgumentException($"行 {h} はnullです", nameof(matrix));

            if (matrix[h].Length != width)
                throw new ArgumentException("すべての行は同じ長さである必要があります", nameof(matrix));

            for (int w = 0; w < width; w++)
                result[h][w] = matrix[h][w];
        }

        return result;
    }

    /// <summary>
    /// 行列を転置します（行と列を入れ替えます）。
    /// </summary>
    /// <param name="matrix">転置する行列</param>
    /// <returns>転置された行列</returns>
    /// <exception cref="ArgumentNullException">行列がnullの場合にスローされます</exception>
    /// <exception cref="ArgumentException">行列が空または不規則な形状の場合にスローされます</exception>
    public static T[][] Transpose(T[][] matrix)
    {
        if (matrix == null)
            throw new ArgumentNullException(nameof(matrix), "行列はnullであってはなりません");

        if (matrix.Length == 0)
            throw new ArgumentException("行列は少なくとも1行必要です", nameof(matrix));

        if (matrix[0] == null)
            throw new ArgumentException("行列の行はnullであってはなりません", nameof(matrix));

        int height = matrix.Length;
        int width = matrix[0].Length;

        // 行列の形状を検証
        for (int h = 0; h < height; h++)
        {
            if (matrix[h] == null || matrix[h].Length != width)
                throw new ArgumentException("すべての行は同じ長さである必要があります", nameof(matrix));
        }

        var result = Create(width, height);

        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                result[w][h] = matrix[h][w];
            }
        }

        return result;
    }
}