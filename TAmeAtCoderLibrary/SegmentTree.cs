namespace TAmeAtCoderLibrary;

/// <summary>
/// セグメント木：指定された範囲に対するクエリ（合計、最大、最小）を効率的に実行できるデータ構造を提供します。
/// 任意の区間に対する演算結果をO(log N)で取得することができます。
/// 要素の更新もO(log N)で行うことができます。
/// </summary>
/// <remarks>
/// セグメント木は、配列などの連続したデータ構造に対する区間クエリを効率的に処理するためのデータ構造です。
/// 主に区間の合計、最大値、最小値などを高速に計算するために使用されます。
/// </remarks>
public class SegmentTree
{
    private readonly int _MinIndex = -1;
    private readonly int _NumberOfNodes = -1;
    /// <summary>
    /// セグメント木の最小インデックスを取得します。
    /// 通常は0から始まります。
    /// </summary>
    /// <value>セグメント木の最小インデックス</value>
    public int MinIndex => _MinIndex;
    /// <summary>
    /// セグメント木のノード数を取得します。
    /// これは、このセグメント木で管理できる要素の総数を表します。
    /// </summary>
    /// <value>セグメント木のノード数</value>
    public int NumberOfSegments => _NumberOfNodes;

    private readonly Node _RootNode;

    /// <summary>
    /// セグメント木を初期化します。
    /// </summary>
    /// <param name="numberOfNodes">セグメント木のノード数。管理したい要素の総数を指定します。</param>
    /// <remarks>
    /// 初期化時にはすべての値が0に設定されます。
    /// 値を設定するには、<see cref="Add"/>または<see cref="SetValue"/>メソッドを使用してください。
    /// </remarks>
    public SegmentTree(int numberOfNodes)
    {
        _MinIndex = 0;
        _NumberOfNodes = numberOfNodes;
        _RootNode = new Node(0, numberOfNodes);
    }

    /// <summary>
    /// 指定されたインデックスに値を追加します。
    /// 既存の値に加算されます。
    /// </summary>
    /// <param name="index">値を追加するインデックス。0から始まる整数で、<see cref="NumberOfSegments"/>未満である必要があります。</param>
    /// <param name="value">追加する値。任意の整数値を指定できます。</param>
    /// <remarks>
    /// このメソッドは既存の値に指定された値を加算します。
    /// 値を直接設定する場合は<see cref="SetValue"/>メソッドを使用してください。
    /// </remarks>
    public void Add(int index, long value)
    {
        _RootNode.Add(index, value);
    }

    /// <summary>
    /// 指定されたインデックスの値を設定します。
    /// 既存の値は上書きされます。
    /// </summary>
    /// <param name="index">値を設定するインデックス。0から始まる整数で、<see cref="NumberOfSegments"/>未満である必要があります。</param>
    /// <param name="value">設定する値。任意の整数値を指定できます。</param>
    /// <remarks>
    /// このメソッドは既存の値を上書きします。
    /// 値を加算する場合は<see cref="Add"/>メソッドを使用してください。
    /// </remarks>
    public void SetValue(int index, long value)
    {
        _RootNode.SetValue(index, value);
    }

    /// <summary>
    /// 指定された範囲の合計を取得します。
    /// </summary>
    /// <param name="left">範囲の左端インデックス（含む）。</param>
    /// <param name="right">範囲の右端インデックス（含む）。</param>
    /// <returns>指定された範囲の合計値。</returns>
    /// <remarks>
    /// このメソッドは、left から right までの範囲（両端を含む）の要素の合計を計算します。
    /// 計算量はO(log N)です。
    /// </remarks>
    public long GetSum(int left, int right) => _RootNode.GetSum(left, right - left + 1);

    /// <summary>
    /// 指定された範囲の最大値を取得します。
    /// </summary>
    /// <param name="left">範囲の左端インデックス（含む）。</param>
    /// <param name="right">範囲の右端インデックス（含む）。</param>
    /// <returns>指定された範囲の最大値。範囲内に要素がない場合はlong.MinValueを返します。</returns>
    /// <remarks>
    /// このメソッドは、left から right までの範囲（両端を含む）の要素の最大値を計算します。
    /// 計算量はO(log N)です。
    /// </remarks>
    public long GetMax(int left, int right) => _RootNode.GetMax(left, right - left + 1);

    /// <summary>
    /// 指定された範囲の最小値を取得します。
    /// </summary>
    /// <param name="left">範囲の左端インデックス（含む）。</param>
    /// <param name="right">範囲の右端インデックス（含む）。</param>
    /// <returns>指定された範囲の最小値。範囲内に要素がない場合はlong.MaxValueを返します。</returns>
    /// <remarks>
    /// このメソッドは、left から right までの範囲（両端を含む）の要素の最小値を計算します。
    /// 計算量はO(log N)です。
    /// </remarks>
    public long GetMin(int left, int right) => _RootNode.GetMin(left, right - left + 1);

    /// <summary>
    /// セグメント木のノードを表すクラスです。
    /// 各ノードは範囲の合計、最大値、最小値を管理します。
    /// </summary>
    /// <remarks>
    /// セグメント木の各ノードは、特定の範囲の要素に関する情報（合計、最大値、最小値）を保持します。
    /// 木構造を形成することで、効率的な範囲クエリの処理を可能にします。
    /// </remarks>
    private class Node
    {
        /// <summary>
        /// このノードが担当する範囲の最小インデックスです。
        /// </summary>
        protected readonly int MinIndex = 0;

        /// <summary>
        /// このノードが担当する範囲のノード数です。
        /// </summary>
        protected readonly int NumberOfNodes = 0;

        /// <summary>
        /// このノードが担当する範囲の要素の合計値です。
        /// </summary>
        protected long Sum = 0L;

        /// <summary>
        /// このノードが担当する範囲の要素の最大値です。
        /// </summary>
        protected long Max = 0L;

        /// <summary>
        /// このノードが担当する範囲の要素の最小値です。
        /// </summary>
        protected long Min = 0L;

        /// <summary>
        /// 左の子ノードへの参照です。葉ノードの場合はnullです。
        /// </summary>
        private readonly Node ChildLeft;

        /// <summary>
        /// 右の子ノードへの参照です。葉ノードの場合はnullです。
        /// </summary>
        private readonly Node ChildRight;

        /// <summary>
        /// ノードを初期化します。
        /// </summary>
        /// <param name="minIndex">ノードがカバーする範囲の最小インデックス。</param>
        /// <param name="numberOfNodes">ノードがカバーするノード数。</param>
        /// <remarks>
        /// 初期化時には、ノードは範囲に応じて再帰的に子ノードを作成します。
        /// ノード数が1の場合は葉ノードとなり、それ以上の分割は行いません。
        /// </remarks>
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
        /// <remarks>
        /// 葉ノードの場合は直接値を更新し、内部ノードの場合は適切な子ノードに処理を委譲した後、
        /// 子ノードの値に基づいて自身の合計、最大値、最小値を更新します。
        /// </remarks>
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
        /// <remarks>
        /// 葉ノードの場合は直接値を設定し、内部ノードの場合は適切な子ノードに処理を委譲した後、
        /// 子ノードの値に基づいて自身の合計、最大値、最小値を更新します。
        /// </remarks>
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
        /// <remarks>
        /// このノードが完全に指定された範囲をカバーしている場合は、保存されている合計値を返します。
        /// そうでない場合は、範囲を分割して子ノードに委譲し、結果を合算します。
        /// </remarks>
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
        /// <returns>指定された範囲の最大値。範囲が存在しない場合はlong.MinValueを返します。</returns>
        /// <remarks>
        /// このノードが完全に指定された範囲をカバーしている場合は、保存されている最大値を返します。
        /// そうでない場合は、範囲を分割して子ノードに委譲し、結果の最大値を返します。
        /// </remarks>
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
        /// <returns>指定された範囲の最小値。範囲が存在しない場合はlong.MaxValueを返します。</returns>
        /// <remarks>
        /// このノードが完全に指定された範囲をカバーしている場合は、保存されている最小値を返します。
        /// そうでない場合は、範囲を分割して子ノードに委譲し、結果の最小値を返します。
        /// </remarks>
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

