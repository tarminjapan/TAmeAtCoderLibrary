namespace TAmeAtCoderLibrary;

public static partial class MathEx
{
    /// <summary>
    /// 素数に関する便利な関数を提供します。
    /// </summary>
    public static class Prime
    {
        /// <summary>
        /// 指定された最大値以下の素数をすべて列挙します。
        /// </summary>
        /// <param name="max">素数を探索する最大値。</param>
        /// <returns>2からmaxまでの素数を昇順にソートした配列。</returns>
        public static int[] PrimeNumbers(int max)
        {
            var nums = new HashSet<int>();

            for (int i = 2; i <= max; i++)
                nums.Add(i);

            for (int i = 2; i <= (int)Math.Sqrt(max); i++)
            {
                int n = i * 2;

                while (n <= max)
                {
                    nums.Remove(n);
                    n += i;
                }
            }

            var primes = nums.ToArray();
            Array.Sort(primes);
            return primes;
        }

        /// <summary>
        /// 指定された整数が素数であるかどうかを判定します。
        /// </summary>
        /// <param name="num">判定する整数。</param>
        /// <returns>numが素数である場合はtrue、そうでない場合はfalse。</returns>
        public static bool IsPrimeNumber(int num)
        {
            if (num == 1) return false;
            else if (num == 2) return true;
            else if (num % 2 == 0) return false;

            var sqrt = (int)Math.Sqrt(num);

            for (int i = 3; i <= sqrt; i += 2)
                if (num % i == 0)
                    return false;

            return true;
        }

        /// <summary>
        /// 指定された整数を素因数分解します。
        /// </summary>
        /// <param name="num">素因数分解する整数。</param>
        /// <returns>素因数をキー、その指数を値とする辞書。</returns>
        public static Dictionary<long, long> PrimeFactorization(long num)
        {
            var dic = new Dictionary<long, long>();
            var sqrt = (long)Math.Sqrt(num);

            for (int i = 2; i <= sqrt; i++)
            {
                while (num % i == 0)
                {
                    num /= i;
                    if (!dic.ContainsKey(i))
                        dic.Add(i, 0);
                    dic[i]++;
                }
            }

            if (num != 1)
            {
                if (!dic.ContainsKey(num))
                    dic.Add(num, 0);
                dic[num]++;
            }

            return dic;
        }
    }
}