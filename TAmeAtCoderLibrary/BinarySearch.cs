namespace TAmeAtCoderLibrary;

/// <summary>
/// バイナリサーチを行うための静的クラス。
/// </summary>
public static class BinarySearch<T> where T : IComparable
{
    /// <summary>
    /// 値が合致する要素の中で一番小さいIndexを返す。
    /// 合致する要素が存在しない場合は、値未満の要素の中で一番大きいIndexを返す。
    /// </summary>
    /// <param name="array">検索対象のソート済み配列。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>合致する要素の中で一番小さいIndex。存在しない場合は値未満の要素の中で一番大きいIndex。どちらも存在しない場合は-1。</returns>
    public static int FindFirstOrOver(T[] array, T value)
    {
        var idx = FindFirst(array, value);
        return 0 <= idx ? idx : Over(array, value);
    }

    /// <summary>
    /// 値が合致する要素の中で一番小さいIndexを返す。
    /// 合致する要素が存在しない場合は、値より大きい要素の中で一番小さいIndexを返す。
    /// </summary>
    /// <param name="array">検索対象のソート済み配列。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>合致する要素の中で一番小さいIndex。存在しない場合は値より大きい要素の中で一番小さいIndex。どちらも存在しない場合は-1。</returns>
    public static int FindFirstOrUnder(T[] array, T value)
    {
        var idx = FindFirst(array, value);
        return 0 <= idx ? idx : Under(array, value);
    }

    /// <summary>
    /// 値が合致する要素の中で一番大きいIndexを返す。
    /// 合致する要素が存在しない場合は、値より大きい要素の中で一番小さいIndexを返す。
    /// </summary>
    /// <param name="array">検索対象のソート済み配列。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>合致する要素の中で一番大きいIndex。存在しない場合は値より大きい要素の中で一番小さいIndex。どちらも存在しない場合は-1。</returns>
    public static int FindLastOrUnder(T[] array, T value)
    {
        var idx = FindLast(array, value);
        return 0 <= idx ? idx : Under(array, value);
    }

    /// <summary>
    /// 値が合致する要素の中で一番大きいIndexを返す。
    /// 合致する値が存在しない場合は、値未満の要素の中で一番大きいIndexを返す。
    /// </summary>
    /// <param name="array">検索対象のソート済み配列。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>合致する要素の中で一番大きいIndex。存在しない場合は値未満の要素の中で一番大きいIndex。どちらも存在しない場合は-1。</returns>
    public static int FindLastOrOver(T[] array, T value)
    {
        var idx = FindLast(array, value);
        return 0 <= idx ? idx : Over(array, value);
    }

    /// <summary>
    /// 値が合致する要素の中で一番小さいIndexを返す。合致する要素が存在しない場合は-1を返す。
    /// </summary>
    /// <param name="array">検索対象のソート済み配列。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>合致する要素の中で一番小さいIndex。存在しない場合は-1。</returns>
    public static int FindFirst(T[] array, T value)
    {
        int left = 0, right = array.Length - 1;
        return right < left ? -1 : FindFirst(array, value, left, right);
    }

    /// <summary>
    ///  FindFirst の内部実装用メソッド
    /// </summary>
    /// <param name="array"></param>
    /// <param name="value"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    private static int FindFirst(T[] array, T value, int left, int right)
    {
        if (right < left) return -1;

        var middle = (left + right) / 2;
        var mValue = array[middle];

        if (left == right)
            return mValue.CompareTo(value) == 0 ? middle : -1;

        if (mValue.CompareTo(value) == 1) return FindFirst(array, value, left, middle - 1);
        else if (mValue.CompareTo(value) == -1) return FindFirst(array, value, middle + 1, right);
        else return FindFirst(array, value, left, middle);
    }

    /// <summary>
    /// 値が合致する要素の中で一番大きいIndexを返す。合致する要素が存在しない場合は-1を返す。
    /// </summary>
    /// <param name="array">検索対象のソート済み配列。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>合致する要素の中で一番大きいIndex。存在しない場合は-1。</returns>
    public static int FindLast(T[] array, T value)
    {
        int left = 0, right = array.Length - 1;
        return right < left ? -1 : FindLast(array, value, left, right);
    }

    /// <summary>
    /// FindLast の内部実装用メソッド
    /// </summary>
    /// <param name="array"></param>
    /// <param name="value"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    private static int FindLast(T[] array, T value, int left, int right)
    {
        if (right < left) return -1;

        var middle = (left + right) / 2 + ((left + right) % 2 == 1 ? 1 : 0);
        var mValue = array[middle];

        if (left == right)
            return mValue.CompareTo(value) == 0 ? middle : -1;

        if (mValue.CompareTo(value) == 1) return FindLast(array, value, left, middle - 1);
        else if (mValue.CompareTo(value) == -1) return FindLast(array, value, middle + 1, right);
        else return FindLast(array, value, middle, right);
    }

    /// <summary>
    /// 値未満の要素の中で一番大きいIndexを返す。値未満の要素が存在しない場合は-1を返す。
    /// </summary>
    /// <param name="array">検索対象のソート済み配列。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>値未満の要素の中で一番大きいIndex。存在しない場合は-1。</returns>
    public static int Over(T[] array, T value)
    {
        int left = 0, right = array.Length - 1;
        return right < left ? -1 : Over(array, value, left, right);
    }

    /// <summary>
    /// Over の内部実装用メソッド
    /// </summary>
    /// <param name="array"></param>
    /// <param name="value"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    private static int Over(T[] array, T value, int left, int right)
    {
        var middle = (left + right) / 2 + ((left + right) % 2 == 1 ? 1 : 0);
        var mValue = array[middle];

        if (left == right)
            return mValue.CompareTo(value) == -1 ? middle : -1;
        else
            return mValue.CompareTo(value) == -1 ? Over(array, value, middle, right) : Over(array, value, left, middle - 1);
    }

    /// <summary>
    /// 値より大きいの要素の中で一番小さいIndexを返す。値以上の要素が存在しない場合は-1を返す。
    /// </summary>
    /// <param name="array">検索対象のソート済み配列。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>値より大きい要素の中で一番小さいIndex。存在しない場合は-1。</returns>
    public static int Under(T[] array, T value)
    {
        int left = 0, right = array.Length - 1;
        return right < left ? -1 : Under(array, value, left, right);

    }

    /// <summary>
    /// Under の内部実装用メソッド
    /// </summary>
    /// <param name="array"></param>
    /// <param name="value"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    private static int Under(T[] array, T value, int left, int right)
    {
        var middle = (left + right) / 2;
        var mValue = array[middle];

        if (left == right)
            return mValue.CompareTo(value) == 1 ? middle : -1;
        else
            return mValue.CompareTo(value) == 1 ? Under(array, value, left, middle) : Under(array, value, middle + 1, right);
    }

    /// <summary>
    /// 値が合致する要素の中で一番小さいIndexを返す。
    /// 合致する要素が存在しない場合は、値未満の要素の中で一番大きいIndexを返す。
    /// </summary>
    /// <param name="array">検索対象のソート済みリスト。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>合致する要素の中で一番小さいIndex。存在しない場合は値未満の要素の中で一番大きいIndex。どちらも存在しない場合は-1。</returns>
    public static int FindFirstOrOver(List<T> array, T value)
    {
        var idx = FindFirst(array, value);
        return 0 <= idx ? idx : Over(array, value);
    }

    /// <summary>
    /// 値が合致する要素の中で一番小さいIndexを返す。
    /// 合致する要素が存在しない場合は、値より大きい要素の中で一番小さいIndexを返す。
    /// </summary>
    /// <param name="array">検索対象のソート済みリスト。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>合致する要素の中で一番小さいIndex。存在しない場合は値より大きい要素の中で一番小さいIndex。どちらも存在しない場合は-1。</returns>
    public static int FindFirstOrUnder(List<T> array, T value)
    {
        var idx = FindFirst(array, value);
        return 0 <= idx ? idx : Under(array, value);
    }

    /// <summary>
    /// 値が合致する要素の中で一番大きいIndexを返す。
    /// 合致する要素が存在しない場合は、値より大きい要素の中で一番小さいIndexを返す。
    /// </summary>
    /// <param name="array">検索対象のソート済みリスト。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>合致する要素の中で一番大きいIndex。存在しない場合は値より大きい要素の中で一番小さいIndex。どちらも存在しない場合は-1。</returns>
    public static int FindLastOrUnder(List<T> array, T value)
    {
        var idx = FindLast(array, value);
        return 0 <= idx ? idx : Under(array, value);
    }

    /// <summary>
    /// 値が合致する要素の中で一番大きいIndexを返す。
    /// 合致する値が存在しない場合は、値未満の要素の中で一番大きいIndexを返す。
    /// </summary>
    /// <param name="array">検索対象のソート済みリスト。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>合致する要素の中で一番大きいIndex。存在しない場合は値未満の要素の中で一番大きいIndex。どちらも存在しない場合は-1。</returns>
    public static int FindLastOrOver(List<T> array, T value)
    {
        var idx = FindLast(array, value);
        return 0 <= idx ? idx : Over(array, value);
    }

    /// <summary>
    /// 値が合致する要素の中で一番小さいIndexを返す。合致する要素が存在しない場合は-1を返す。
    /// </summary>
    /// <param name="array">検索対象のソート済みリスト。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>合致する要素の中で一番小さいIndex。存在しない場合は-1。</returns>
    public static int FindFirst(List<T> array, T value)
    {
        int left = 0, right = array.Count - 1;
        return right < left ? -1 : FindFirst(array, value, left, right);
    }

    /// <summary>
    /// FindFirst の内部実装用メソッド
    /// </summary>
    /// <param name="array"></param>
    /// <param name="value"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    private static int FindFirst(List<T> array, T value, int left, int right)
    {
        if (right < left) return -1;

        var middle = (left + right) / 2;
        var mValue = array[middle];

        if (left == right)
            return mValue.CompareTo(value) == 0 ? middle : -1;

        if (mValue.CompareTo(value) == 1) return FindFirst(array, value, left, middle - 1);
        else if (mValue.CompareTo(value) == -1) return FindFirst(array, value, middle + 1, right);
        else return FindFirst(array, value, left, middle);
    }

    /// <summary>
    /// 値が合致する要素の中で一番大きいIndexを返す。合致する要素が存在しない場合は-1を返す。
    /// </summary>
    /// <param name="array">検索対象のソート済みリスト。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>合致する要素の中で一番大きいIndex。存在しない場合は-1。</returns>
    public static int FindLast(List<T> array, T value)
    {
        int left = 0, right = array.Count - 1;
        return right < left ? -1 : FindLast(array, value, left, right);
    }

    /// <summary>
    /// FindLast の内部実装用メソッド
    /// </summary>
    /// <param name="array"></param>
    /// <param name="value"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    private static int FindLast(List<T> array, T value, int left, int right)
    {
        if (right < left) return -1;

        var middle = (left + right) / 2 + ((left + right) % 2 == 1 ? 1 : 0);
        var mValue = array[middle];

        if (left == right)
            return mValue.CompareTo(value) == 0 ? middle : -1;

        if (mValue.CompareTo(value) == 1) return FindLast(array, value, left, middle - 1);
        else if (mValue.CompareTo(value) == -1) return FindLast(array, value, middle + 1, right);
        else return FindLast(array, value, middle, right);
    }

    /// <summary>
    /// 値未満の要素の中で一番大きいIndexを返す。値未満の要素が存在しない場合は-1を返す。
    /// </summary>
    /// <param name="array">検索対象のソート済みリスト。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>値未満の要素の中で一番大きいIndex。存在しない場合は-1。</returns>
    public static int Over(List<T> array, T value)
    {
        int left = 0, right = array.Count - 1;
        return right < left ? -1 : Over(array, value, left, right);
    }

    /// <summary>
    /// Over の内部実装用メソッド
    /// </summary>
    /// <param name="array"></param>
    /// <param name="value"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    private static int Over(List<T> array, T value, int left, int right)
    {
        var middle = (left + right) / 2 + ((left + right) % 2 == 1 ? 1 : 0);
        var mValue = array[middle];

        if (left == right)
            return mValue.CompareTo(value) == -1 ? middle : -1;
        else
            return mValue.CompareTo(value) == -1 ? Over(array, value, middle, right) : Over(array, value, left, middle - 1);
    }

    /// <summary>
    /// ソート済みリスト内で、指定された値より大きい要素のうち、最も小さいインデックスを返します。
    /// 指定された値より大きい要素が存在しない場合は -1 を返します。
    /// </summary>
    /// <param name="array">検索対象のソート済みリスト。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>指定された値より大きい要素の中で一番小さいインデックス。そのような要素が存在しない場合は -1。</returns>
    public static int Under(List<T> array, T value)
    {
        int left = 0, right = array.Count - 1;
        return right < left ? -1 : Under(array, value, left, right);

    }

    /// <summary>
    /// Under メソッドの内部実装用。
    /// </summary>
    /// <param name="array">検索対象のソート済みリスト。</param>
    /// <param name="value">検索する値。</param>
    /// <param name="left">検索範囲の左端インデックス。</param>
    /// <param name="right">検索範囲の右端インデックス。</param>
    /// <returns>指定された値より大きい要素の中で一番小さいインデックス。そのような要素が存在しない場合は -1。</returns>
    private static int Under(List<T> array, T value, int left, int right)
    {
        var middle = (left + right) / 2;
        var mValue = array[middle];

        if (left == right)
            return mValue.CompareTo(value) == 1 ? middle : -1;
        else
            return mValue.CompareTo(value) == 1 ? Under(array, value, left, middle) : Under(array, value, middle + 1, right);
    }
}
