namespace TAmeAtCoderLibrary;

/// <summary>
/// 数学的な計算、特に整数論に関連する拡張メソッドやユーティリティを提供します。
/// </summary>
public static partial class MathEx
{
    /// <summary>
    /// 約数、最大公約数 (GCD)、最小公倍数 (LCM) の計算機能を提供します。
    /// </summary>
    public static class DivAndMulti
    {
        /// <summary>
        /// 指定された正の整数の約数をすべて取得します。
        /// </summary>
        /// <param name="n">約数を求めたい正の整数。</param>
        /// <returns>n の約数を昇順に格納した SortedSet。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="n"/> が 0 以下の場合にスローされます。</exception>
        public static SortedSet<int> Divisors(int n)
        {
            if (n <= 0)
            {
                // 正の整数のみを対象とする
                throw new ArgumentOutOfRangeException(nameof(n), "Input must be a positive integer.");
            }

            var divisors = new SortedSet<int>();
            // i * i <= n は i <= sqrt(n) と等価で、オーバーフローしにくい
            for (int i = 1; i * i <= n; i++)
            {
                if (n % i == 0)
                {
                    divisors.Add(i);
                    // nがiの平方でない場合のみ n/i を追加 (重複回避)
                    if (i * i != n)
                    {
                        divisors.Add(n / i);
                    }
                }
            }
            return divisors;
        }

        /// <summary>
        /// 指定された正の整数の約数をすべて取得します。
        /// </summary>
        /// <param name="n">約数を求めたい正の整数。</param>
        /// <returns>n の約数を昇順に格納した SortedSet。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="n"/> が 0 以下の場合にスローされます。</exception>
        public static SortedSet<long> Divisors(long n)
        {
            if (n <= 0L)
            {
                // 正の整数のみを対象とする
                throw new ArgumentOutOfRangeException(nameof(n), "Input must be a positive integer.");
            }

            var divisors = new SortedSet<long>();
            // i * i <= n は i <= sqrt(n) と等価で、オーバーフローしにくい
            for (long i = 1; i * i <= n; i++)
            {
                if (n % i == 0L)
                {
                    divisors.Add(i);
                    // nがiの平方でない場合のみ n/i を追加 (重複回避)
                    if (i * i != n)
                    {
                        divisors.Add(n / i);
                    }
                }
            }
            return divisors;
        }

        /// <summary>
        /// 複数の整数の最大公約数 (GCD) を計算します。
        /// </summary>
        /// <param name="numbers">最大公約数を求めたい整数のコレクション。要素は1つ以上必要です。</param>
        /// <returns>指定されたすべての整数の最大公約数。入力が負数でも結果は非負になります。すべての入力が0の場合は0を返します。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="numbers"/> が null の場合にスローされます。</exception>
        /// <exception cref="ArgumentException"><paramref name="numbers"/> が空の場合にスローされます。</exception>
        public static int Gcd(params int[] numbers)
        {
            if (numbers == null) throw new ArgumentNullException(nameof(numbers));
            if (numbers.Length == 0) throw new ArgumentException("Input collection must contain at least one element.", nameof(numbers));

            // Math.Abs で負数に対応
            int result = Math.Abs(numbers[0]);
            for (int i = 1; i < numbers.Length; i++)
            {
                result = Gcd(result, Math.Abs(numbers[i]));
                // GCDが1になったらそれ以上計算しても変わらないので早期終了 (任意)
                if (result == 1) break;
            }
            return result;
        }

        /// <summary>
        /// 2つの整数の最大公約数 (GCD) をユークリッドの互除法 (反復版) を用いて計算します。
        /// </summary>
        /// <param name="a">1つ目の整数。</param>
        /// <param name="b">2つ目の整数。</param>
        /// <returns>a と b の最大公約数。常に非負の値を返します。Gcd(0, 0) は 0 を返します。</returns>
        public static int Gcd(int a, int b)
        {
            // 入力の絶対値を取る
            a = Math.Abs(a);
            b = Math.Abs(b);

            // ユークリッドの互除法 (反復版)
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            // ループ終了時、a が最大公約数 (b=0 のケースを含む)
            return a;
        }

        /// <summary>
        /// 複数の整数の最大公約数 (GCD) を計算します。
        /// </summary>
        /// <param name="numbers">最大公約数を求めたい整数のコレクション。要素は1つ以上必要です。</param>
        /// <returns>指定されたすべての整数の最大公約数。入力が負数でも結果は非負になります。すべての入力が0の場合は0を返します。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="numbers"/> が null の場合にスローされます。</exception>
        /// <exception cref="ArgumentException"><paramref name="numbers"/> が空の場合にスローされます。</exception>
        public static long Gcd(params long[] numbers)
        {
            if (numbers == null) throw new ArgumentNullException(nameof(numbers));
            if (numbers.Length == 0) throw new ArgumentException("Input collection must contain at least one element.", nameof(numbers));

            // Math.Abs で負数に対応
            long result = Math.Abs(numbers[0]);
            for (int i = 1; i < numbers.Length; i++)
            {
                result = Gcd(result, Math.Abs(numbers[i]));
                // GCDが1になったらそれ以上計算しても変わらないので早期終了 (任意)
                if (result == 1L) break;
            }
            return result;
        }

        /// <summary>
        /// 2つの整数の最大公約数 (GCD) をユークリッドの互除法 (反復版) を用いて計算します。
        /// </summary>
        /// <param name="a">1つ目の整数。</param>
        /// <param name="b">2つ目の整数。</param>
        /// <returns>a と b の最大公約数。常に非負の値を返します。Gcd(0, 0) は 0 を返します。</returns>
        public static long Gcd(long a, long b)
        {
            // 入力の絶対値を取る
            a = Math.Abs(a);
            b = Math.Abs(b);

            // ユークリッドの互除法 (反復版)
            while (b != 0L)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        /// <summary>
        /// 複数の整数の最小公倍数 (LCM) を計算します。
        /// </summary>
        /// <param name="numbers">最小公倍数を求めたい整数のコレクション。要素は1つ以上必要です。</param>
        /// <returns>指定されたすべての整数の最小公倍数。結果は非負になります。入力に0が含まれる場合、LCMは0になります。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="numbers"/> が null の場合にスローされます。</exception>
        /// <exception cref="ArgumentException"><paramref name="numbers"/> が空の場合にスローされます。</exception>
        /// <exception cref="OverflowException">計算の途中または最終結果が <see cref="long"/> の範囲を超える場合にスローされる可能性があります。</exception>
        public static long Lcm(params long[] numbers)
        {
            if (numbers == null) throw new ArgumentNullException(nameof(numbers));
            if (numbers.Length == 0) throw new ArgumentException("Input collection must contain at least one element.", nameof(numbers));

            // 入力に0が含まれていたらLCMは0
            if (numbers.Any(n => n == 0L))
            {
                return 0L;
            }

            // Math.Abs で負数に対応
            long result = Math.Abs(numbers[0]);
            for (int i = 1; i < numbers.Length; i++)
            {
                // Lcm(long, long) 内でオーバーフローチェックを行う
                result = Lcm(result, Math.Abs(numbers[i]));
            }
            return result;
        }

        /// <summary>
        /// 2つの整数の最小公倍数 (LCM) を計算します。
        /// </summary>
        /// <param name="a">1つ目の整数。</param>
        /// <param name="b">2つ目の整数。</param>
        /// <returns>a と b の最小公倍数。常に非負の値を返します。Lcm(a, 0) または Lcm(0, b) は 0 を返します。</returns>
        /// <exception cref="OverflowException">計算結果が <see cref="long"/> の範囲を超える場合にスローされます。</exception>
        public static long Lcm(long a, long b)
        {
            // 絶対値を取る
            a = Math.Abs(a);
            b = Math.Abs(b);

            // どちらかが0ならLCMは0
            if (a == 0L || b == 0L)
            {
                return 0L;
            }

            // Gcd(a, b) を計算 (Gcdは常に非負を返す)
            long gcdValue = Gcd(a, b);

            // Lcm(a, b) = (|a| / Gcd(a, b)) * |b| を計算
            // オーバーフローをチェックするために checked ブロックを使用
            try
            {
                // a は gcdValue で割り切れる
                long quotient = a / gcdValue;
                // 乗算でオーバーフローが発生しないかチェック
                return checked(quotient * b);
            }
            catch (OverflowException ex)
            {
                // より詳細な情報を含めて再スロー
                throw new OverflowException($"Calculating LCM for {a} and {b} resulted in an overflow.", ex);
            }
        }
    }
}