namespace TAmeAtCoderLibrary;

/// <summary>
/// ソート済みリストに対するバイナリサーチ関連機能を提供します。
/// リストは昇順にソートされている必要があります。
/// </summary>
/// <typeparam name="T">比較可能な要素の型。</typeparam>
public static class BinarySearchUtils<T> where T : IComparable<T>
{
    /// <summary>
    /// 指定された値以上の最初の要素のインデックスを検索します (Lower Bound)。
    /// </summary>
    /// <param name="list">検索対象のソート済みリスト。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>
    /// value 以上の値を持つ最初の要素のインデックス。
    /// そのような要素が存在しない場合 (全ての要素が value 未満の場合) は list.Count を返します。
    /// </returns>
    /// <exception cref="ArgumentNullException">list が null の場合にスローされます。</exception>
    public static int LowerBound(IReadOnlyList<T> list, T value)
    {
        if (list == null) throw new ArgumentNullException(nameof(list));

        int left = 0;
        int count = list.Count;
        while (left < count)
        {
            int middle = left + (count - left) / 2;
            if (list[middle].CompareTo(value) < 0)
            {
                left = middle + 1;
            }
            else
            {
                count = middle;
            }
        }
        return left;
    }

    /// <summary>
    /// 指定された値より大きい最初の要素のインデックスを検索します (Upper Bound)。
    /// </summary>
    /// <param name="list">検索対象のソート済みリスト。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>
    /// value より大きい値を持つ最初の要素のインデックス。
    /// そのような要素が存在しない場合 (全ての要素が value 以下の場合) は list.Count を返します。
    /// </returns>
    /// <exception cref="ArgumentNullException">list が null の場合にスローされます。</exception>
    public static int UpperBound(IReadOnlyList<T> list, T value)
    {
        if (list == null) throw new ArgumentNullException(nameof(list));

        int left = 0;
        int count = list.Count;
        while (left < count)
        {
            int middle = left + (count - left) / 2;
            if (list[middle].CompareTo(value) <= 0)
            {
                left = middle + 1;
            }
            else
            {
                count = middle;
            }
        }
        return left;
    }

    /// <summary>
    /// 値が合致する最初の要素のインデックスを返します。
    /// </summary>
    /// <param name="list">検索対象のソート済みリスト。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>合致する最初の要素のインデックス。存在しない場合は -1。</returns>
    /// <exception cref="ArgumentNullException">list が null の場合にスローされます。</exception>
    public static int FindFirst(IReadOnlyList<T> list, T value)
    {
        int lb = LowerBound(list, value);
        if (lb < list.Count && list[lb].CompareTo(value) == 0)
        {
            return lb;
        }
        return -1;
    }

    /// <summary>
    /// 値が合致する最後の要素のインデックスを返します。
    /// </summary>
    /// <param name="list">検索対象のソート済みリスト。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>合致する最後の要素のインデックス。存在しない場合は -1。</returns>
    /// <exception cref="ArgumentNullException">list が null の場合にスローされます。</exception>
    public static int FindLast(IReadOnlyList<T> list, T value)
    {
        int ub = UpperBound(list, value);
        if (ub > 0 && list[ub - 1].CompareTo(value) == 0)
        {
            return ub - 1;
        }
        return -1;
    }

    /// <summary>
    /// 値未満の最大の要素のインデックスを返します。
    /// </summary>
    /// <param name="list">検索対象のソート済みリスト。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>値未満の最大の要素のインデックス。存在しない場合は -1。</returns>
    /// <exception cref="ArgumentNullException">list が null の場合にスローされます。</exception>
    public static int FindLargestLessThan(IReadOnlyList<T> list, T value)
    {
        int lb = LowerBound(list, value);
        return lb > 0 ? lb - 1 : -1;
    }

    /// <summary>
    /// 値より大きい最小の要素のインデックスを返します。
    /// </summary>
    /// <param name="list">検索対象のソート済みリスト。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>値より大きい最小の要素のインデックス。存在しない場合は -1。</returns>
    /// <exception cref="ArgumentNullException">list が null の場合にスローされます。</exception>
    public static int FindSmallestGreaterThan(IReadOnlyList<T> list, T value)
    {
        int ub = UpperBound(list, value);
        return ub < list.Count ? ub : -1;
    }

    /// <summary>
    /// 値が合致する最初の要素、または値未満の最大の要素のインデックスを返します。
    /// </summary>
    /// <param name="list">検索対象のソート済みリスト。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>条件に合致する要素のインデックス。存在しない場合は -1。</returns>
    /// <exception cref="ArgumentNullException">list が null の場合にスローされます。</exception>
    public static int FindFirstOrLargestLessThan(IReadOnlyList<T> list, T value)
    {
        int lb = LowerBound(list, value);
        if (lb < list.Count && list[lb].CompareTo(value) == 0)
        {
            return lb;
        }
        return lb > 0 ? lb - 1 : -1;
    }

    /// <summary>
    /// 値が合致する最初の要素、または値より大きい最小の要素のインデックスを返します。
    /// </summary>
    /// <param name="list">検索対象のソート済みリスト。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>条件に合致する要素のインデックス。存在しない場合は -1。</returns>
    /// <exception cref="ArgumentNullException">list が null の場合にスローされます。</exception>
    public static int FindFirstOrSmallestGreaterThan(IReadOnlyList<T> list, T value)
    {
        int lb = LowerBound(list, value);
        if (lb < list.Count && list[lb].CompareTo(value) == 0)
        {
            return lb;
        }
        // FindSmallestGreaterThan (UpperBound の結果) を返す
        int ub = UpperBound(list, value);
        return ub < list.Count ? ub : -1;
    }

    /// <summary>
    /// 値が合致する最後の要素、または値より大きい最小の要素のインデックスを返します。
    /// </summary>
    /// <param name="list">検索対象のソート済みリスト。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>条件に合致する要素のインデックス。存在しない場合は -1。</returns>
    /// <exception cref="ArgumentNullException">list が null の場合にスローされます。</exception>
    public static int FindLastOrSmallestGreaterThan(IReadOnlyList<T> list, T value)
    {
        int lastIndex = FindLast(list, value); // FindLast は null チェックを含む
        if (lastIndex != -1)
        {
            return lastIndex;
        }
        // FindSmallestGreaterThan (UpperBound の結果) を返す
        int ub = UpperBound(list, value); // UpperBound は null チェックを含む
        return ub < list.Count ? ub : -1;
    }

    /// <summary>
    /// 値が合致する最後の要素、または値未満の最大の要素のインデックスを返します。
    /// </summary>
    /// <param name="list">検索対象のソート済みリスト。</param>
    /// <param name="value">検索する値。</param>
    /// <returns>条件に合致する要素のインデックス。存在しない場合は -1。</returns>
    /// <exception cref="ArgumentNullException">list が null の場合にスローされます。</exception>
    public static int FindLastOrLargestLessThan(IReadOnlyList<T> list, T value)
    {
        int lastIndex = FindLast(list, value); // FindLast は null チェックを含む
        if (lastIndex != -1)
        {
            return lastIndex;
        }
        // FindLargestLessThan (LowerBound - 1 の結果) を返す
        int lb = LowerBound(list, value); // LowerBound は null チェックを含む
        return lb > 0 ? lb - 1 : -1;
    }
}