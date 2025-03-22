namespace TAmeAtCoderLibrary;

public static partial class MathEx
{
    /// <summary>
    /// 数列の総和を計算するためのユーティリティクラス
    /// </summary>
    public static class Aggregate
    {
        /// <summary>
        /// 等差1の数列の和を計算します
        /// </summary>
        /// <param name="a">初項（数列の最初の値）</param>
        /// <param name="l">末項（数列の最後の値）</param>
        /// <returns>数列の総和</returns>
        public static long CumSum1(long a, long l) => CumSum2(a, l, l - a + 1);

        /// <summary>
        /// 等差数列の和を計算します（初項と末項から）
        /// </summary>
        /// <param name="a">初項（数列の最初の値）</param>
        /// <param name="l">末項（数列の最後の値）</param>
        /// <param name="n">項数（数列の要素数）</param>
        /// <returns>数列の総和</returns>
        public static long CumSum2(long a, long l, long n) => n * (a + l) / 2L;

        /// <summary>
        /// 等差数列の和を計算します（初項と公差から）
        /// </summary>
        /// <param name="a">初項（数列の最初の値）</param>
        /// <param name="d">公差（隣接する項の差）</param>
        /// <param name="n">項数（数列の要素数）</param>
        /// <returns>数列の総和</returns>
        public static long CumSum3(long a, long d, long n) => n * (2L * a + (n - 1L) * d) / 2L;

        /// <summary>
        /// 等差1の数列の和を計算し、指定した数で剰余を取ります
        /// </summary>
        /// <param name="a">初項（数列の最初の値）</param>
        /// <param name="l">末項（数列の最後の値）</param>
        /// <param name="divisor">剰余を取る数（法）</param>
        /// <returns>数列の総和を指定した数で割った余り</returns>
        public static long CumSumMod1(long a, long l, long divisor) => CumSumMod2(a, l, l - a + 1, divisor);

        /// <summary>
        /// 等差数列の和を計算し、指定した数で剰余を取ります
        /// </summary>
        /// <param name="a">初項（数列の最初の値）</param>
        /// <param name="l">末項（数列の最後の値）</param>
        /// <param name="n">項数（数列の要素数）</param>
        /// <param name="divisor">剰余を取る数（法）</param>
        /// <returns>数列の総和を指定した数で割った余り</returns>
        public static long CumSumMod2(long a, long l, long n, long divisor)
        {
            a %= divisor;
            l %= divisor;
            n %= divisor;

            return CumSum2(a, l, n) % divisor;
        }
    }
}