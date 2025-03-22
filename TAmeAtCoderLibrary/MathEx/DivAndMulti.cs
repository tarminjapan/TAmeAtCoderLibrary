namespace TAmeAtCoderLibrary;

/// <summary>
/// 約数や最大公約数、最小公倍数を求めるためのクラスです。
/// </summary>
public static partial class MathEx
{
    /// <summary>
    /// 約数や最大公約数、最小公倍数を求めるためのクラスです。
    /// </summary>
    public static class DivAndMulti
    {
        /// <summary>
        /// 約数一覧を取得する
        /// </summary>
        /// <param name="n">約数を求めたい整数。</param>
        /// <returns>n の約数を昇順に並べた SortedSet。</returns>
        public static SortedSet<int> Divisors(int n)
        {
            var set = new SortedSet<int>();

            for (int l = 1; l <= (int)Math.Sqrt(n); l++)
            {
                if (n % l == 0)
                {
                    set.Add(l);
                    set.Add(n / l);
                }
            }

            return set;
        }

        /// <summary>
        /// 約数一覧を取得する
        /// </summary>
        /// <param name="n">約数を求めたい整数。</param>
        /// <returns>n の約数を昇順に並べた SortedSet。</returns>
        public static SortedSet<long> Divisors(long n)
        {
            var set = new SortedSet<long>();

            for (long l = 1; l <= (long)Math.Sqrt(n); l++)
            {
                if (n % l == 0)
                {
                    set.Add(l);
                    set.Add(n / l);
                }
            }

            return set;
        }

        /// <summary>
        /// 最大公約数 (Greatest Common Divisor) を求めます。
        /// </summary>
        /// <param name="ns">最大公約数を求めたい整数の配列。</param>
        /// <returns>ns に含まれるすべての整数の最大公約数。</returns>
        public static long Gcd(int[] ns)
        {
            var gcd = ns[0];

            foreach (var n in ns.Skip(1))
                gcd = Gcd(gcd, n);

            return gcd;
        }

        /// <summary>
        /// 最大公約数 (Greatest Common Divisor) を求めます。
        /// </summary>
        /// <param name="n1">整数1</param>
        /// <param name="n2">整数2</param>
        /// <returns>n1 と n2 の最大公約数。</returns>
        public static int Gcd(int n1, int n2)
        {
            int a = Math.Max(n1, n2);
            int b = Math.Min(n1, n2);

            if (b == 0)
                return a;

            return Gcd(b, a % b);
        }

        /// <summary>
        /// 最大公約数 (Greatest Common Divisor) を求めます。
        /// </summary>
        /// <param name="ns">最大公約数を求めたい整数の配列。</param>
        /// <returns>ns に含まれるすべての整数の最大公約数。</returns>
        public static long Gcd(long[] ns)
        {
            var gcd = ns[0];

            foreach (var n in ns.Skip(1))
                gcd = Gcd(gcd, n);

            return gcd;
        }

        /// <summary>
        /// 最大公約数 (Greatest Common Divisor) を求めます。
        /// </summary>
        /// <param name="n1">整数1</param>
        /// <param name="n2">整数2</param>
        /// <returns>n1 と n2 の最大公約数。</returns>
        public static long Gcd(long n1, long n2)
        {
            long a = Math.Max(n1, n2);
            long b = Math.Min(n1, n2);

            if (b == 0L)
                return a;

            return Gcd(b, a % b);
        }

        /// <summary>
        /// 最小公倍数 (Least Common Multiple) を求めます。
        /// </summary>
        /// <param name="ns">最小公倍数を求めたい整数の配列。</param>
        /// <returns>ns に含まれるすべての整数の最小公倍数。</returns>
        public static long Lcm(long[] ns)
        {
            var lcm = ns[0];

            foreach (var n in ns.Skip(1))
                lcm = Lcm(lcm, n);

            return lcm;
        }

        /// <summary>
        /// 最小公倍数 (Least Common Multiple) を求めます。
        /// </summary>
        /// <param name="n1">整数1</param>
        /// <param name="n2">整数2</param>
        /// <returns>n1 と n2 の最小公倍数。</returns>
        public static long Lcm(long n1, long n2) => n1 / Gcd(n1, n2) * n2;
    }
}