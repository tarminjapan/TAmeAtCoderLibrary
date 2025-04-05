namespace TAmeAtCoderLibrary;

/// <summary>
/// セグメント木：指定された範囲に対するクエリ（合計、最大、最小）を効率的に実行できるデータ構造を提供します。
/// 任意の区間に対する演算結果をO(log N)で取得でき、要素の更新もO(log N)で行えます。
/// </summary>
public class SegmentTree
{
    private readonly int _size;
    private readonly Node _rootNode;
    private const int DefaultMinIndex = 0; // 通常は0固定

    /// <summary>
    /// セグメント木が管理する要素の数を取得します。
    /// </summary>
    public int Size => _size;

    /// <summary>
    /// セグメント木のインデックスの開始値を取得します（通常は0）。
    /// </summary>
    public int MinIndex => DefaultMinIndex;

    /// <summary>
    /// セグメント木を指定された要素数で初期化します。
    /// </summary>
    /// <param name="size">セグメント木で管理する要素の総数。1以上の値を指定してください。</param>
    /// <exception cref="ArgumentOutOfRangeException">sizeが0以下の場合にスローされます。</exception>
    public SegmentTree(int size)
    {
        if (size <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(size), "Size must be positive.");
        }
        _size = size;
        _rootNode = new Node(DefaultMinIndex, size);
    }

    /// <summary>
    /// 指定されたインデックスに値を追加します（既存の値に加算）。
    /// </summary>
    /// <param name="index">値を追加するインデックス（0から始まる）。</param>
    /// <param name="value">追加する値。</param>
    /// <exception cref="ArgumentOutOfRangeException">indexが範囲外の場合にスローされます。</exception>
    /// <remarks>この実装では、Addは Sum, Max, Min を更新します。Max/Minの更新は加算後の値がそのまま反映されます。</remarks>
    public void Add(int index, long value)
    {
        if (index < 0 || index >= _size)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"Index must be between 0 and {_size - 1}.");
        }
        _rootNode.Add(index, value);
    }

    /// <summary>
    /// 指定されたインデックスの値を新しい値で設定（上書き）します。
    /// </summary>
    /// <param name="index">値を設定するインデックス（0から始まる）。</param>
    /// <param name="value">設定する新しい値。</param>
    /// <exception cref="ArgumentOutOfRangeException">indexが範囲外の場合にスローされます。</exception>
    public void SetValue(int index, long value)
    {
        if (index < 0 || index >= _size)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"Index must be between 0 and {_size - 1}.");
        }
        _rootNode.SetValue(index, value);
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
        // 要素数を渡す (right - left + 1)
        return _rootNode.GetSum(left, right - left + 1);
    }

    /// <summary>
    /// 指定された範囲 [left, right] (両端含む) の最大値を取得します。
    /// </summary>
    /// <param name="left">範囲の左端インデックス（含む）。</param>
    /// <param name="right">範囲の右端インデックス（含む）。</param>
    /// <returns>指定された範囲の最大値。範囲内に要素がない場合はlong.MinValueを返します。</returns>
    /// <exception cref="ArgumentOutOfRangeException">leftまたはrightが範囲外の場合にスローされます。</exception>
    /// <exception cref="ArgumentException">leftがrightより大きい場合にスローされます。</exception>
    public long GetMax(int left, int right)
    {
        ValidateRange(left, right);
        return _rootNode.GetMax(left, right - left + 1);
    }

    /// <summary>
    /// 指定された範囲 [left, right] (両端含む) の最小値を取得します。
    /// </summary>
    /// <param name="left">範囲の左端インデックス（含む）。</param>
    /// <param name="right">範囲の右端インデックス（含む）。</param>
    /// <returns>指定された範囲の最小値。範囲内に要素がない場合はlong.MaxValueを返します。</returns>
    /// <exception cref="ArgumentOutOfRangeException">leftまたはrightが範囲外の場合にスローされます。</exception>
    /// <exception cref="ArgumentException">leftがrightより大きい場合にスローされます。</exception>
    public long GetMin(int left, int right)
    {
        ValidateRange(left, right);
        return _rootNode.GetMin(left, right - left + 1);
    }

    /// <summary>
    /// 範囲クエリの引数 (left, right) が有効か検証します。
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

    // --- Private Node Class ---

    /// <summary>
    /// セグメント木のノード。範囲の集計値（合計、最大、最小）を保持し、再帰的な操作を行う。
    /// </summary>
    private class Node
    {
        internal readonly int StartIndex; // このノードが担当する範囲の開始インデックス
        internal readonly int Count;      // このノードが担当する要素数
        internal long Sum;
        internal long Max;
        internal long Min;
        internal readonly Node ChildLeft;
        internal readonly Node ChildRight;

        /// <summary>
        /// ノードを初期化し、必要であれば子ノードを再帰的に生成する。
        /// </summary>
        internal Node(int startIndex, int count)
        {
            StartIndex = startIndex;
            Count = count;

            // 初期値の設定 (葉ノード以外は子から集約される)
            Sum = 0L;          // 合計の単位元
            Max = long.MinValue; // 最大値の単位元
            Min = long.MaxValue; // 最小値の単位元

            if (count > 1)
            {
                int leftCount = count / 2;
                int rightCount = count - leftCount;
                ChildLeft = new Node(startIndex, leftCount);
                ChildRight = new Node(startIndex + leftCount, rightCount);
                // Note: 初期状態では子ノードの値も単位元なので、UpdateAggregatesは不要
            }
            // count == 1 の場合は葉ノード。ChildLeft/Rightはnullのまま。
        }

        /// <summary>
        /// 子ノードの値に基づいて、このノードの集計値 (Sum, Max, Min) を更新する。
        /// </summary>
        private void UpdateAggregates()
        {
            // 葉ノードの場合は子がないので更新不要
            if (Count > 1)
            {
                Sum = ChildLeft.Sum + ChildRight.Sum;
                Max = Math.Max(ChildLeft.Max, ChildRight.Max);
                Min = Math.Min(ChildLeft.Min, ChildRight.Min);
            }
        }

        /// <summary>
        /// 指定インデックスに値を追加し、関連ノードを更新する。
        /// </summary>
        internal void Add(int index, long value)
        {
            if (Count == 1) // 葉ノード
            {
                // Add の挙動: Sum に加算し、Max/Min はその結果とする
                Sum += value;
                Max = Sum;
                Min = Sum;
            }
            else // 内部ノード
            {
                // 適切な子ノードに処理を委譲
                if (index < ChildRight.StartIndex)
                    ChildLeft.Add(index, value);
                else
                    ChildRight.Add(index, value);

                // 子ノードの変更を反映
                UpdateAggregates();
            }
        }

        /// <summary>
        /// 指定インデックスの値を設定し、関連ノードを更新する。
        /// </summary>
        internal void SetValue(int index, long value)
        {
            if (Count == 1) // 葉ノード
            {
                Sum = value;
                Max = value;
                Min = value;
            }
            else // 内部ノード
            {
                // 適切な子ノードに処理を委譲
                if (index < ChildRight.StartIndex)
                    ChildLeft.SetValue(index, value);
                else
                    ChildRight.SetValue(index, value);

                // 子ノードの変更を反映
                UpdateAggregates();
            }
        }

        /// <summary>
        /// 指定範囲 [queryStart, queryStart + queryCount) の合計値を取得する。
        /// </summary>
        internal long GetSum(int queryStart, int queryCount)
        {
            int nodeEnd = StartIndex + Count;
            int queryEnd = queryStart + queryCount;

            // 1. クエリ範囲がノード範囲と全く交差しない場合
            if (queryEnd <= StartIndex || nodeEnd <= queryStart)
            {
                return 0L; // 合計の単位元
            }

            // 2. クエリ範囲がノード範囲を完全に含む場合
            if (queryStart <= StartIndex && nodeEnd <= queryEnd)
            {
                return Sum;
            }

            // 3. それ以外（クエリ範囲がノード範囲の一部と交差する場合）
            // 子ノードに問い合わせる
            long leftSum = ChildLeft.GetSum(queryStart, queryCount);
            long rightSum = ChildRight.GetSum(queryStart, queryCount);
            return leftSum + rightSum;
        }


        /// <summary>
        /// 指定範囲 [queryStart, queryStart + queryCount) の最大値を取得する。
        /// </summary>
        internal long GetMax(int queryStart, int queryCount)
        {
            int nodeEnd = StartIndex + Count;
            int queryEnd = queryStart + queryCount;

            // 1. クエリ範囲がノード範囲と全く交差しない場合
            if (queryEnd <= StartIndex || nodeEnd <= queryStart)
            {
                return long.MinValue; // 最大値の単位元
            }

            // 2. クエリ範囲がノード範囲を完全に含む場合
            if (queryStart <= StartIndex && nodeEnd <= queryEnd)
            {
                return Max;
            }

            // 3. それ以外
            long leftMax = ChildLeft.GetMax(queryStart, queryCount);
            long rightMax = ChildRight.GetMax(queryStart, queryCount);
            return Math.Max(leftMax, rightMax);
        }

        /// <summary>
        /// 指定範囲 [queryStart, queryStart + queryCount) の最小値を取得する。
        /// </summary>
        internal long GetMin(int queryStart, int queryCount)
        {
            int nodeEnd = StartIndex + Count;
            int queryEnd = queryStart + queryCount;

            // 1. クエリ範囲がノード範囲と全く交差しない場合
            if (queryEnd <= StartIndex || nodeEnd <= queryStart)
            {
                return long.MaxValue; // 最小値の単位元
            }

            // 2. クエリ範囲がノード範囲を完全に含む場合
            if (queryStart <= StartIndex && nodeEnd <= queryEnd)
            {
                return Min;
            }

            // 3. それ以外
            long leftMin = ChildLeft.GetMin(queryStart, queryCount);
            long rightMin = ChildRight.GetMin(queryStart, queryCount);
            return Math.Min(leftMin, rightMin);
        }
    }
}