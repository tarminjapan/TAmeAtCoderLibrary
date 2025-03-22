namespace TAmeAtCoderLibrary;

public static partial class MathEx
{
    /// <summary>
    /// 組み合わせに関する計算を行うクラス
    /// </summary>
    public static class Pattern
    {
        /// <summary>
        /// 階乗を計算します。
        /// </summary>
        /// <param name="n">階乗を計算する数値。</param>
        /// <returns>n! の計算結果を返します。n が 0 の場合は 0 を返します。</returns>
        public static long Factorial(int n)
        {
            if (n == 0L)
                return 0L;

            long num = 1L;

            for (long l = n; 0L < l; l--)
                try
                {
                    num = checked(num * l);
                }
                catch (OverflowException) { throw; }

            return num;
        }

        /// <summary>
        /// 階乗を計算し、指定された除数で割った余りを返します。
        /// </summary>
        /// <param name="n">階乗を計算する数値。</param>
        /// <param name="divisor">除数。</param>
        /// <returns>n! を divisor で割った余りを返します。n が 0 の場合は 0 を返します。</returns>
        public static long Factorial(long n, long divisor)
        {
            if (n == 0L)
                return 0L;

            long num = 1L;

            for (long l = num; 0L < l; l--)
            {
                try
                {
                    num = checked(num * l);
                }
                catch (OverflowException) { throw; }

                num %= divisor;
            }

            return num;
        }

        /// <summary>
        /// 順列 (n P r) を計算します。
        /// </summary>
        /// <param name="n">要素の総数。</param>
        /// <param name="r">選択する要素の数。</param>
        /// <returns>n P r の計算結果を返します。n が r より小さい場合は 0 を返します。</returns>
        public static long Permutation(int n, int r)
        {
            if (n < r) return 0L;

            long num = 1L;

            for (long i = n; n - r < i; i--)
                try
                {
                    num = checked(num * i);
                }
                catch (OverflowException) { throw; }

            return num;
        }

        /// <summary>
        /// 順列 (n P r) を計算し、指定された除数で割った余りを返します。
        /// </summary>
        /// <param name="n">要素の総数。</param>
        /// <param name="r">選択する要素の数。</param>
        /// <param name="divisor">除数。</param>
        /// <returns>n P r を divisor で割った余りを返します。n が r より小さい場合は 0 を返します。</returns>
        public static long Permutation(int n, int r, long divisor)
        {
            if (n < r) return 0L;

            long num = 1L;

            for (long i = n; n - r < i; i--)
            {
                try
                {
                    num = checked(num * i);
                }
                catch (OverflowException) { throw; }

                num %= divisor;
            }

            return num;
        }

        /// <summary>
        /// 組み合わせ (n C r) を計算します。
        /// </summary>
        /// <param name="n">要素の総数。</param>
        /// <param name="r">選択する要素の数。</param>
        /// <returns>n C r の計算結果を返します。n が r より小さい場合は 0 を返します。</returns>
        public static long Combination(int n, int r)
        {
            if (n < r)
                return 0L;
            else if (n == r)
                return 1L;

            r = Math.Min(r, n - r);

            long num = 1L;

            for (long l = 1L; l <= r; l++)
            {
                try
                {
                    num = checked(num * (n - l + 1L));
                }
                catch (OverflowException) { throw; }

                num /= l;
            }

            return num;
        }

        /// <summary>
        /// 組み合わせ (n C r) を計算し、指定された除数で割った余りを返します。
        /// </summary>
        /// <param name="n">要素の総数。</param>
        /// <param name="r">選択する要素の数。</param>
        /// <param name="divisor">除数。</param>
        /// <returns>n C r を divisor で割った余りを返します。n が r より小さい場合は 0 を返します。</returns>
        public static long Combination(int n, int r, long divisor)
        {
            if (n < r)
                return 0L;
            else if (n == r)
                return 1L;

            r = Math.Min(r, n - r);

            long num = 1L;

            for (long l = 1L; l <= r; l++)
            {
                try
                {
                    num = checked(num * (n - l + 1L));
                }
                catch (OverflowException) { throw; }

                num /= l;
                num %= divisor;
            }

            return num;
        }
    }
}