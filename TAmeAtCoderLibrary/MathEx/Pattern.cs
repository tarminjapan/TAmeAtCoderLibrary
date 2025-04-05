namespace TAmeAtCoderLibrary;

public static partial class MathEx
{
    /// <summary>
    /// 組み合わせに関する計算を提供します。
    /// </summary>
    public static class Pattern
    {
        // --- 内部ヘルパーメソッド ---

        /// <summary>
        /// べき乗剰余 (base^exponent % modulus) を効率的に計算します。
        /// </summary>
        /// <param name="baseValue">底</param>
        /// <param name="exponent">指数</param>
        /// <param name="modulus">法</param>
        /// <returns>(baseValue^exponent) % modulus</returns>
        /// <exception cref="ArgumentOutOfRangeException">modulus が 1 以下の場合にスローされます。</exception>
        private static long ModPow(long baseValue, long exponent, long modulus)
        {
            if (modulus <= 0)
                throw new ArgumentOutOfRangeException(nameof(modulus), "Modulus must be positive.");
            // 指数が負の場合は未対応（必要なら拡張）
            if (exponent < 0)
                throw new ArgumentOutOfRangeException(nameof(exponent), "Exponent must be non-negative.");
            if (modulus == 1) return 0; // a % 1 is always 0

            long result = 1L;
            baseValue %= modulus;
            while (exponent > 0)
            {
                if (exponent % 2 == 1)
                    result = (result * baseValue) % modulus;
                baseValue = (baseValue * baseValue) % modulus;
                exponent /= 2;
            }
            return result;
        }

        /// <summary>
        /// モジュラ逆数 (a^-1 mod m) を計算します。
        /// 法 m が素数であることを前提とします (フェルマーの小定理を使用)。
        /// </summary>
        /// <param name="a">逆数を求める値</param>
        /// <param name="modulus">法 (素数)</param>
        /// <returns>a の modulus を法とする逆数</returns>
        /// <exception cref="ArgumentOutOfRangeException">a が 0 以下、または modulus が 1 以下の場合にスローされます。</exception>
        /// <exception cref="InvalidOperationException">a が modulus で割り切れる場合 (逆数が存在しない場合)。</exception>
        private static long ModInverse(long a, long modulus)
        {
            if (a <= 0)
                throw new ArgumentOutOfRangeException(nameof(a), "Value must be positive for modular inverse using Fermat's Little Theorem.");
            if (modulus <= 1)
                throw new ArgumentOutOfRangeException(nameof(modulus), "Modulus must be greater than 1.");
            if (a % modulus == 0)
                throw new InvalidOperationException($"Modular inverse does not exist for a multiple of the modulus ({a} % {modulus} = 0).");

            // Fermat's Little Theorem: a^(m-2) ≡ a^-1 (mod m) where m is prime
            return ModPow(a, modulus - 2, modulus);
        }

        // --- 階乗 ---

        /// <summary>
        /// 階乗 n! を計算します。
        /// 注意: 結果は非常に大きくなる可能性があり、容易に long の最大値を超えます (21! > long.MaxValue)。
        /// </summary>
        /// <param name="n">階乗を計算する非負整数。</param>
        /// <returns>n! の計算結果。n = 0 の場合は 1 を返します。</returns>
        /// <exception cref="ArgumentOutOfRangeException">n が負の場合にスローされます。</exception>
        /// <exception cref="OverflowException">計算結果が long の最大値を超えた場合にスローされます。</exception>
        public static long Factorial(int n)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException(nameof(n), "Factorial is not defined for negative numbers.");
            if (n == 0)
                return 1L;

            long result = 1L;
            try
            {
                checked // このブロック内でのオーバーフローをチェック
                {
                    for (int i = 1; i <= n; i++)
                    {
                        result *= i;
                    }
                }
            }
            catch (OverflowException)
            {
                // そのまま再スローするか、より具体的な例外をラップしてスローすることも可能
                throw new OverflowException($"Factorial calculation for {n} resulted in an overflow.");
            }
            return result;
        }

        /// <summary>
        /// 階乗 n! を計算し、指定された正の整数 divisor で割った余りを返します。
        /// </summary>
        /// <param name="n">階乗を計算する非負整数。</param>
        /// <param name="divisor">正の除数。</param>
        /// <returns>n! % divisor の計算結果。n = 0 の場合は 1 % divisor を返します。</returns>
        /// <exception cref="ArgumentOutOfRangeException">n が負の場合、または divisor が 1 以下の場合にスローされます。</exception>
        public static long Factorial(long n, long divisor)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException(nameof(n), "Factorial is not defined for negative numbers.");
            if (divisor <= 0)
                throw new ArgumentOutOfRangeException(nameof(divisor), "Divisor must be positive.");
            if (divisor == 1) return 0L; // n! % 1 is always 0
            if (n == 0)
                return 1L % divisor; // 0! = 1

            long result = 1L;
            for (long i = 1L; i <= n; i++)
            {
                // checked は不要。剰余を取るため long の範囲を超える可能性は低い
                // (divisor が非常に大きい場合は別だが、実用上問題ないことが多い)
                result = (result * i) % divisor;
            }
            return result;
        }

        // --- 順列 ---

        /// <summary>
        /// 順列 nPr (n 個から r 個を選んで並べる場合の数) を計算します。
        /// 注意: 結果は非常に大きくなる可能性があり、容易に long の最大値を超えます。
        /// </summary>
        /// <param name="n">要素の総数 (非負整数)。</param>
        /// <param name="r">選択する要素の数 (非負整数)。</param>
        /// <returns>nPr の計算結果。n < r の場合は 0 を返します。</returns>
        /// <exception cref="ArgumentOutOfRangeException">n または r が負の場合にスローされます。</exception>
        /// <exception cref="OverflowException">計算結果が long の最大値を超えた場合にスローされます。</exception>
        public static long Permutation(int n, int r)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "n must be non-negative.");
            if (r < 0) throw new ArgumentOutOfRangeException(nameof(r), "r must be non-negative.");
            if (n < r) return 0L;
            if (r == 0) return 1L; // nP0 = 1

            long result = 1L;
            try
            {
                checked
                {
                    for (int i = 0; i < r; i++)
                    {
                        result *= (n - i);
                    }
                }
            }
            catch (OverflowException)
            {
                throw new OverflowException($"Permutation calculation P({n}, {r}) resulted in an overflow.");
            }
            return result;
        }

        /// <summary>
        /// 順列 nPr を計算し、指定された正の整数 divisor で割った余りを返します。
        /// </summary>
        /// <param name="n">要素の総数 (非負整数)。</param>
        /// <param name="r">選択する要素の数 (非負整数)。</param>
        /// <param name="divisor">正の除数。</param>
        /// <returns>nPr % divisor の計算結果。n < r の場合は 0 を返します。</returns>
        /// <exception cref="ArgumentOutOfRangeException">n または r が負の場合、または divisor が 1 以下の場合にスローされます。</exception>
        public static long Permutation(long n, long r, long divisor)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "n must be non-negative.");
            if (r < 0) throw new ArgumentOutOfRangeException(nameof(r), "r must be non-negative.");
            if (divisor <= 0) throw new ArgumentOutOfRangeException(nameof(divisor), "Divisor must be positive.");
            if (divisor == 1) return 0L;
            if (n < r) return 0L;
            if (r == 0) return 1L % divisor; // nP0 = 1

            long result = 1L;
            for (long i = 0; i < r; i++)
            {
                result = (result * (n - i)) % divisor;
                // Ensure result stays non-negative if (n-i) can be negative or very large leading to negative intermediate modulo result
                if (result < 0) result += divisor;
            }
            return result;
        }

        // --- 組み合わせ ---

        /// <summary>
        /// 組み合わせ nCr (n 個から r 個を選ぶ場合の数) を計算します。
        /// nCr = nPr / r! を計算します。
        /// 注意: 中間計算および最終結果が long の最大値を超える可能性があります。
        /// </summary>
        /// <param name="n">要素の総数 (非負整数)。</param>
        /// <param name="r">選択する要素の数 (非負整数)。</param>
        /// <returns>nCr の計算結果。n < r の場合は 0 を返します。</returns>
        /// <exception cref="ArgumentOutOfRangeException">n または r が負の場合にスローされます。</exception>
        /// <exception cref="OverflowException">計算中または最終結果が long の最大値を超えた場合にスローされます。</exception>
        public static long Combination(int n, int r)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "n must be non-negative.");
            if (r < 0) throw new ArgumentOutOfRangeException(nameof(r), "r must be non-negative.");
            if (n < r) return 0L;

            // nCr = nC(n-r) を利用して計算量を最適化
            r = Math.Min(r, n - r);

            if (r == 0) return 1L; // nC0 = 1
            if (r == 1) return n;  // nC1 = n

            long result = 1L;
            try
            {
                checked
                {
                    // 分子 n * (n-1) * ... * (n-r+1) と 分母 1 * 2 * ... * r を同時に計算
                    for (int i = 1; i <= r; i++)
                    {
                        // 安全のため BigInteger を使うのが望ましいが、long での実装例
                        long termNumerator = n - i + 1;
                        long termDenominator = i;

                        // (result * termNumerator) / termDenominator を計算
                        // 先に乗算するとオーバーフローしやすいので注意が必要
                        result = result * termNumerator / termDenominator;
                    }
                }
            }
            catch (OverflowException)
            {
                throw new OverflowException($"Combination calculation C({n}, {r}) resulted in an overflow.");
            }

            return result;
        }


        /// <summary>
        /// 組み合わせ nCr を計算し、指定された素数 divisor で割った余りを返します。
        /// nCr = n! / (r! * (n-r)!) を (mod divisor) で計算します。
        /// 除算はモジュラ逆数を用いて乗算に変換します。
        /// </summary>
        /// <param name="n">要素の総数 (非負整数)。</param>
        /// <param name="r">選択する要素の数 (非負整数)。</param>
        /// <param name="divisor">正の素数である除数。</param>
        /// <returns>nCr % divisor の計算結果。n < r の場合は 0 を返します。</returns>
        /// <exception cref="ArgumentOutOfRangeException">n または r が負の場合、または divisor が 2 未満の場合にスローされます。</exception>
        /// <remarks>
        /// この実装は divisor が素数であることを前提としています。
        /// 頻繁に呼び出す場合は、階乗とその逆元のテーブルを事前計算する方が効率的です。
        /// </remarks>
        public static long Combination(long n, long r, long divisor)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "n must be non-negative.");
            if (r < 0) throw new ArgumentOutOfRangeException(nameof(r), "r must be non-negative.");
            // divisor は素数である必要があるため、2以上
            if (divisor < 2) throw new ArgumentOutOfRangeException(nameof(divisor), "Divisor must be a prime number (>= 2).");
            if (n < r) return 0L;

            // nCr = nC(n-r)
            r = Math.Min(r, n - r);

            if (r == 0) return 1L % divisor;
            if (r == 1) return n % divisor;

            // nCr = n! / (r! * (n-r)!) mod p
            //     = (n! * (r!)^-1 * ((n-r)!)^-1) mod p

            // 分子: n * (n-1) * ... * (n-r+1) mod p
            long numerator = 1L;
            for (long i = 0; i < r; i++)
            {
                numerator = (numerator * (n - i)) % divisor;
            }

            // 分母: r! mod p
            long denominator = 1L;
            for (long i = 1; i <= r; i++)
            {
                denominator = (denominator * i) % divisor;
            }

            // 分母の逆数を計算: (r!)^-1 mod p
            long denominatorInverse;
            try
            {
                denominatorInverse = ModInverse(denominator, divisor);
            }
            catch (InvalidOperationException ex)
            {
                // r! が divisor の倍数の場合、逆数は存在しない。
                // これは r >= divisor の場合に起こる。
                // この場合、nCr は divisor で割り切れるため、結果は 0 になる。
                // (n >= divisor かつ r < divisor の場合は分子に divisor の倍数が含まれるため numerator が 0 になるはず)
                // (n < divisor の場合は r < divisor なので、denominator が 0 になることはない)
                if (denominator == 0) return 0L;
                // 予期せぬエラー
                throw new InvalidOperationException($"Failed to compute modular inverse for denominator {denominator} mod {divisor}. It might not be prime or other issue.", ex);
            }


            long result = (numerator * denominatorInverse) % divisor;
            // 結果が負になる場合があるので補正
            if (result < 0) result += divisor;

            return result;
        }
    }
}