namespace TAmeAtCoderLibrary;

/// <summary>
/// セグメント木：指定された範囲に対するクエリ（合計、最大、最小）を効率的に実行できるデータ構造を提供します。
/// </summary>
public class SegmentTree
{
    private readonly int _MinIndex = -1;
    private readonly int _NumberOfNodes = -1;
    /// <summary>
    /// セグメント木の最小インデックスを取得します。
    /// </summary>
    public int MinIndex => _MinIndex;
    /// <summary>
    /// セグメント木のノード数を取得します。
    /// </summary>
    public int NumberOfSegments => _NumberOfNodes;

    private readonly Node _RootNode;

    /// <summary>
    /// セグメント木を初期化します。
    /// </summary>
    /// <param name="numberOfNodes">セグメント木のノード数。</param>
    public SegmentTree(int numberOfNodes)
    {
        _MinIndex = 0;
        _NumberOfNodes = numberOfNodes;
        _RootNode = new Node(0, numberOfNodes);
    }

    /// <summary>
    /// 指定されたインデックスに値を追加します。
    /// </summary>
    /// <param name="index">値を追加するインデックス。</param>
    /// <param name="value">追加する値。</param>
    public void Add(int index, long value)
    {
        _RootNode.Add(index, value);
    }

    /// <summary>
    /// 指定されたインデックスの値を設定します。
    /// </summary>
    /// <param name="index">値を設定するインデックス。</param>
    /// <param name="value">設定する値。</param>
    public void SetValue(int index, long value)
    {
        _RootNode.SetValue(index, value);
    }

    /// <summary>
    /// 指定された範囲の合計を取得します。
    /// </summary>
    /// <param name="left">範囲の左端インデックス。</param>
    /// <param name="right">範囲の右端インデックス。</param>
    /// <returns>指定された範囲の合計。</returns>
    public long GetSum(int left, int right) => _RootNode.GetSum(left, right - left + 1);
    /// <summary>
    /// 指定された範囲の最大値を取得します。
    /// </summary>
    /// <param name="left">範囲の左端インデックス。</param>
    /// <param name="right">範囲の右端インデックス。</param>
    /// <returns>指定された範囲の最大値。</returns>
    public long GetMax(int left, int right) => _RootNode.GetMax(left, right - left + 1);
    /// <summary>
    /// 指定された範囲の最小値を取得します。
    /// </summary>
    /// <param name="left">範囲の左端インデックス。</param>
    /// <param name="right">範囲の右端インデックス。</param>
    /// <returns>指定された範囲の最小値。</returns>
    public long GetMin(int left, int right) => _RootNode.GetMin(left, right - left + 1);

    private class Node
    {
        protected readonly int MinIndex = 0;
        protected readonly int NumberOfNodes = 0;

        protected long Sum = 0L;
        protected long Max = 0L;
        protected long Min = 0L;

        private readonly Node ChildLeft;
        private readonly Node ChildRight;

        /// <summary>
        /// ノードを初期化します。
        /// </summary>
        /// <param name="minIndex">ノードがカバーする範囲の最小インデックス。</param>
        /// <param name="numberOfNodes">ノードがカバーするノード数。</param>
        public Node(int minIndex, int numberOfNodes)
        {
            MinIndex = minIndex;
            NumberOfNodes = numberOfNodes;

            if (numberOfNodes == 1) return;

            var tNumofNodes = numberOfNodes / 2;
            ChildLeft = new Node(minIndex, tNumofNodes);
            ChildRight = new Node(minIndex + tNumofNodes, numberOfNodes - tNumofNodes);
        }

        /// <summary>
        /// 指定されたインデックスに値を追加します。
        /// </summary>
        /// <param name="index">値を追加するインデックス。</param>
        /// <param name="value">追加する値。</param>
        public void Add(int index, long value)
        {
            if (NumberOfNodes == 1)
            {
                Sum += value;
                Max = value;
                Min = value;
            }
            else
            {
                if (index < ChildRight.MinIndex)
                    ChildLeft.Add(index, value);
                else
                    ChildRight.Add(index, value);

                Sum = ChildLeft.Sum + ChildRight.Sum;
                Max = Math.Max(ChildLeft.Max, ChildRight.Max);
                Min = Math.Min(ChildLeft.Min, ChildRight.Min);
            }
        }

        /// <summary>
        /// 指定されたインデックスの値を設定します。
        /// </summary>
        /// <param name="index">値を設定するインデックス。</param>
        /// <param name="value">設定する値。</param>
        public void SetValue(int index, long value)
        {
            if (NumberOfNodes == 1)
            {
                Sum = value;
                Max = value;
                Min = value;
            }
            else
            {
                if (index < ChildRight.MinIndex)
                    ChildLeft.Add(index, value);
                else
                    ChildRight.Add(index, value);

                Sum = ChildLeft.Sum + ChildRight.Sum;
                Max = Math.Max(ChildLeft.Max, ChildRight.Max);
                Min = Math.Min(ChildLeft.Min, ChildRight.Min);
            }
        }

        /// <summary>
        /// 指定された範囲の合計を取得します。
        /// </summary>
        /// <param name="left">範囲の左端インデックス。</param>
        /// <param name="numofNodes">範囲のノード数。</param>
        /// <returns>指定された範囲の合計。</returns>
        public long GetSum(int left, int numofNodes)
        {
            if (MinIndex == left && NumberOfNodes == numofNodes) return Sum;

            var sum = 0L;

            if (left < ChildRight.MinIndex)
            {
                var tNumofNodes = Math.Min(numofNodes, ChildRight.MinIndex - left);
                sum += ChildLeft.GetSum(left, tNumofNodes);
            }

            if (ChildRight.MinIndex <= left + numofNodes - 1)
            {
                var tLeft = Math.Max(ChildRight.MinIndex, left);
                var tNumofNodes = left + numofNodes - tLeft;
                sum += ChildRight.GetSum(tLeft, tNumofNodes);
            }

            return sum;
        }

        /// <summary>
        /// 指定された範囲の最大値を取得します。
        /// </summary>
        /// <param name="left">範囲の左端インデックス。</param>
        /// <param name="numofNodes">範囲のノード数。</param>
        /// <returns>指定された範囲の最大値。</returns>
        public long GetMax(int left, int numofNodes)
        {
            if (MinIndex == left && NumberOfNodes == numofNodes) return Max;

            var max = long.MinValue;

            if (left < ChildRight.MinIndex)
            {
                var tNumofNodes = Math.Min(numofNodes, ChildRight.MinIndex - left);
                max = Math.Max(max, ChildLeft.GetMax(left, tNumofNodes));
            }

            if (ChildRight.MinIndex <= left + numofNodes - 1)
            {
                var tLeft = Math.Max(ChildRight.MinIndex, left);
                var tNumofNodes = left + numofNodes - tLeft;
                max = Math.Max(max, ChildRight.GetMax(tLeft, tNumofNodes));
            }

            return max;
        }

        /// <summary>
        /// 指定された範囲の最小値を取得します。
        /// </summary>
        /// <param name="left">範囲の左端インデックス。</param>
        /// <param name="numofNodes">範囲のノード数。</param>
        /// <returns>指定された範囲の最小値。</returns>
        public long GetMin(int left, int numofNodes)
        {
            if (MinIndex == left && NumberOfNodes == numofNodes) return Min;

            var min = long.MinValue;

            if (left < ChildRight.MinIndex)
            {
                var tNumofNodes = Math.Min(numofNodes, ChildRight.MinIndex - left);
                min = Math.Min(min, ChildLeft.GetMin(left, tNumofNodes));
            }

            if (ChildRight.MinIndex <= left + numofNodes - 1)
            {
                var tLeft = Math.Min(ChildRight.MinIndex, left);
                var tNumofNodes = left + numofNodes - tLeft;
                min = Math.Min(min, ChildRight.GetMin(tLeft, tNumofNodes));
            }

            return min;
        }
    }
}

