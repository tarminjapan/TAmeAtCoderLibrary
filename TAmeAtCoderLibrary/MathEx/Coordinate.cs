namespace TAmeAtCoderLibrary;

public static partial class MathEx
{
    /// <summary>
    /// 座標に関する計算を行うユーティリティクラスです。
    /// </summary>
    public static class Coordinate
    {
        /// <summary>
        /// 3点の位置関係を判定します（Counter Clock Wise）。
        /// </summary>
        /// <param name="A">1つ目の点の座標 (X, Y)</param>
        /// <param name="B">2つ目の点の座標 (X, Y)</param>
        /// <param name="C">3つ目の点の座標 (X, Y)</param>
        /// <returns>
        /// 1: 点Cは線分ABの反時計回り方向にある（左側）
        /// -1: 点Cは線分ABの時計回り方向にある（右側）
        /// 0: 3点が一直線上にある
        /// </returns>
        public static int CounterClockWise((int X, int Y) A, (int X, int Y) B, (int X, int Y) C)
        {
            // Calculate the determinant of the matrix
            double area2 = (B.X - A.X) * (C.Y - A.Y) - (B.Y - A.Y) * (C.X - A.X);

            if (area2 > 0)
                return 1; // counter-clockwise
            else if (area2 < 0)
                return -1; // clockwise
            else
                return 0; // collinear
        }

        /// <summary>
        /// 3点の位置関係を判定します（Counter Clock Wise）。
        /// 配列形式の座標を使用するオーバーロードです。
        /// </summary>
        /// <param name="A">1つ目の点の座標 [X, Y]</param>
        /// <param name="B">2つ目の点の座標 [X, Y]</param>
        /// <param name="C">3つ目の点の座標 [X, Y]</param>
        /// <returns>
        /// 1: 点Cは線分ABの反時計回り方向にある（左側）
        /// -1: 点Cは線分ABの時計回り方向にある（右側）
        /// 0: 3点が一直線上にある
        /// </returns>
        public static int CounterClockWise(int[] A, int[] B, int[] C) => CounterClockWise((A[0], A[1]), (B[0], B[1]), (C[0], C[1]));
    }
}