namespace TAmeAtCoderLibrary;

public static partial class MathEx
{
    /// <summary>
    /// 素数に関する計算機能を提供します。
    /// </summary>
    public static class Prime
    {
        /// <summary>
        /// 指定された上限値以下のすべての素数を列挙します (エラトステネスの篩)。
        /// </summary>
        /// <param name="limit">素数を探索する上限値 (0以上)。</param>
        /// <returns>2から <paramref name="limit"/> までの素数を昇順に格納した配列。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="limit"/> が負の場合にスローされます。</exception>
        public static int[] EnumeratePrimes(int limit)
        {
            if (limit < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(limit), "上限値は0以上である必要があります。");
            }
            if (limit < 2)
            {
                return Array.Empty<int>(); // 2未満に素数はない
            }

            // limit + 1 のサイズの配列を用意 (インデックスを数値と対応させるため)
            var isPrime = new bool[limit + 1];
            // 最初はすべて素数候補とする (true)
            for (int i = 2; i <= limit; i++)
            {
                isPrime[i] = true;
            }

            // 篩にかける
            // √limit まで調べれば十分
            int sqrtLimit = (int)Math.Sqrt(limit);
            for (int p = 2; p <= sqrtLimit; p++)
            {
                // pが素数候補の場合のみ、その倍数を篩い落とす
                if (isPrime[p])
                {
                    // p*p 未満の p の倍数は、より小さい素数によって既に篩い落とされている
                    for (int multiple = p * p; multiple <= limit; multiple += p)
                    {
                        isPrime[multiple] = false;
                    }
                }
            }

            // 素数リストを作成
            var primes = new List<int>();
            for (int i = 2; i <= limit; i++)
            {
                if (isPrime[i])
                {
                    primes.Add(i);
                }
            }
            return primes.ToArray();
        }

        /// <summary>
        /// 指定された整数が素数であるかどうかを判定します (試し割り法)。
        /// </summary>
        /// <param name="number">判定する整数。</param>
        /// <returns><paramref name="number"/> が素数である場合は true、そうでない場合は false。</returns>
        public static bool IsPrime(long number) // より大きな数を扱えるように long に変更も検討
        {
            // 1以下は素数ではない
            if (number <= 1) return false;
            // 2は素数
            if (number == 2) return true;
            // 2以外の偶数は素数ではない
            if (number % 2 == 0) return false;

            // √number まで奇数で割ってみる
            long sqrt = Sqrt(number);
            for (long i = 3; i <= sqrt; i += 2)
            {
                if (number % i == 0)
                {
                    // 割り切れたら素数ではない
                    return false;
                }
            }

            // 上記のいずれにも当てはまらなければ素数
            return true;
        }

        /// <summary>
        /// 指定された正の整数を素因数分解します (試し割り法)。
        /// </summary>
        /// <param name="number">素因数分解する正の整数。</param>
        /// <returns>素因数をキー、その指数を値とする辞書。numberが1以下の場合は空の辞書。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="number"/> が0以下の場合にスローされます。</exception>
        public static Dictionary<long, int> PrimeFactorization(long number)
        {
            if (number <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(number), "素因数分解は正の整数に対してのみ定義されます。");
            }

            var factors = new Dictionary<long, int>();
            if (number == 1)
            {
                return factors; // 1の素因数分解は空
            }

            long tempNumber = number;
            long sqrt = Sqrt(tempNumber);

            // 2から√numberまで割っていく
            for (long i = 2; i <= sqrt; i++)
            {
                if (tempNumber % i == 0) // iで割り切れる場合
                {
                    int count = 0;
                    while (tempNumber % i == 0) // 割り切れなくなるまで割る
                    {
                        tempNumber /= i;
                        count++;
                    }
                    factors.Add(i, count); // 素因数とその指数を追加
                }
                // ループ中に tempNumber が 1 になったら終了
                if (tempNumber == 1) break;
                // √number も動的に更新 (最適化だが必須ではない)
                // sqrt = Sqrt(tempNumber);
            }

            // ループ終了後、tempNumberが1より大きい場合、それは残った大きな素因数
            if (tempNumber > 1)
            {
                factors.Add(tempNumber, 1);
            }

            return factors;
        }

        /// <summary>
        /// 指定された正の整数を素因数分解します (試し割り法、TryGetValue版)。
        /// </summary>
        /// <param name="number">素因数分解する正の整数。</param>
        /// <returns>素因数をキー、その指数を値とする辞書。numberが1以下の場合は空の辞書。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="number"/> が0以下の場合にスローされます。</exception>
        public static Dictionary<long, int> PrimeFactorizationVer2(long number)
        {
            if (number <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(number), "素因数分解は正の整数に対してのみ定義されます。");
            }

            var factors = new Dictionary<long, int>();
            if (number == 1)
            {
                return factors;
            }

            long tempNumber = number;

            // 2で割り切れるだけ割る
            while (tempNumber % 2 == 0)
            {
                factors.TryGetValue(2, out int count); // 現在のカウントを取得、なければ0
                factors[2] = count + 1;                 // カウントを+1して更新
                tempNumber /= 2;
            }

            // 3以上の奇数で割っていく (√number まで)
            long sqrt = Sqrt(tempNumber);
            for (long i = 3; i <= sqrt; i += 2)
            {
                while (tempNumber % i == 0)
                {
                    factors.TryGetValue(i, out int count);
                    factors[i] = count + 1;
                    tempNumber /= i;
                }
                // ループ中に tempNumber が 1 になったら終了
                if (tempNumber == 1) break;
                // √number も動的に更新 (最適化だが必須ではない)
                sqrt = Sqrt(tempNumber);
            }

            // ループ終了後、tempNumberが1より大きい場合、それは残った大きな素因数
            if (tempNumber > 1)
            {
                factors.TryGetValue(tempNumber, out int count);
                factors[tempNumber] = count + 1;
            }

            return factors;
        }
    }
}