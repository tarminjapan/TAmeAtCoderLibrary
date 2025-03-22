namespace TAmeAtCoderLibrary;

public static partial class MathEx
{
    /// <summary>
    /// 数学的な基本的な機能を提供します。
    /// </summary>
    public static class Basic
    {
        /// <summary>
        /// 指定された数を指定されたべき乗で累乗した値を返します。
        /// </summary>
        /// <param name="x">累乗される数値。</param>
        /// <param name="y">指数。</param>
        /// <returns>数値 x を y 乗した値。</returns>
        public static long Pow(long x, int y)
        {
            try
            {
                var pow = 1L;

                for (int i = 0; i < y; i++)
                    pow = checked(pow * x);

                return pow;
            }
            catch (OverflowException)
            {
                throw;
            }
        }

        /// <summary>
        /// 割り算を行い、結果を切り上げます。
        /// </summary>
        /// <param name="bloken">割られる数。</param>
        /// <param name="divided">割る数。</param>
        /// <returns>bloken を divided で割った結果の切り上げ。</returns>
        public static long Ceiling(long bloken, long divided) => bloken % divided == 0L ? bloken / divided : bloken / divided + 1L;

        /// <summary>
        /// 割り算を行い、結果を切り上げます。
        /// </summary>
        /// <param name="bloken">割られる数。</param>
        /// <param name="divided">割る数。</param>
        /// <returns>bloken を divided で割った結果の切り上げ。</returns>
        public static int Ceiling(int bloken, int divided) => (int)Ceiling((long)bloken, divided);

        /// <summary>
        /// 指定された数値の桁数を取得します。
        /// </summary>
        /// <param name="num">桁数を調べたい数値。</param>
        /// <returns>num の桁数。num が 0 の場合は 1 を返します。</returns>
        public static int CountDigits(long num) => num == 0 ? 1 : (int)Math.Floor(Math.Log10(num) + 1);

        /// <summary>
        /// 二つの数値を加算し、指定された除数による剰余を返します。
        /// </summary>
        /// <param name="a">加算する最初の数値。</param>
        /// <param name="b">加算する二番目の数値。</param>
        /// <param name="divisor">剰余計算に使用する除数。</param>
        /// <returns>(a + b) % divisor の結果。</returns>
        public static long ModAdd(long a, long b, long divisor) => (a + b) % divisor;

        /// <summary>
        /// 指定された整数の約数を昇順で取得します。
        /// </summary>
        /// <param name="n">約数を求めたい整数。</param>
        /// <returns>n の約数を格納したソート済みのセット。</returns>
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
        /// 指定された長整数の約数を昇順で取得します。
        /// </summary>
        /// <param name="n">約数を求めたい長整数。</param>
        /// <returns>n の約数を格納したソート済みのセット。</returns>
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
        /// 指定された整数の配列の最大公約数（Greatest Common Divisor）を求めます。
        /// </summary>
        /// <param name="ns">最大公約数を求めたい整数の配列。</param>
        /// <returns>配列 ns に含まれるすべての整数の最大公約数。</returns>
        public static long Gcd(int[] ns)
        {
            var gcd = ns[0];

            foreach (var n in ns.Skip(1))
                gcd = Gcd(gcd, n);

            return gcd;
        }

        /// <summary>
        /// 指定された二つの整数の最大公約数（Greatest Common Divisor）を求めます。
        /// </summary>
        /// <param name="n1">最初の整数。</param>
        /// <param name="n2">二番目の整数。</param>
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
        /// 指定された長整数の配列の最大公約数（Greatest Common Divisor）を求めます。
        /// </summary>
        /// <param name="ns">最大公約数を求めたい長整数の配列。</param>
        /// <returns>配列 ns に含まれるすべての長整数の最大公約数。</returns>
        public static long Gcd(long[] ns)
        {
            var gcd = ns[0];

            foreach (var n in ns.Skip(1))
                gcd = Gcd(gcd, n);

            return gcd;
        }

        /// <summary>
        /// 指定された二つの長整数の最大公約数（Greatest Common Divisor）を求めます。
        /// </summary>
        /// <param name="n1">最初の長整数。</param>
        /// <param name="n2">二番目の長整数。</param>
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
        /// 指定された長整数の配列の最小公倍数（Least Common Multiple）を求めます。
        /// </summary>
        /// <param name="ns">最小公倍数を求めたい長整数の配列。</param>
        /// <returns>配列 ns に含まれるすべての長整数の最小公倍数。</returns>
        public static long Lcm(long[] ns)
        {
            var lcm = ns[0];

            foreach (var n in ns.Skip(1))
                lcm = Lcm(lcm, n);

            return lcm;
        }

        /// <summary>
        /// 指定された二つの長整数の最小公倍数（Least Common Multiple）を求めます。
        /// </summary>
        /// <param name="n1">最初の長整数。</param>
        /// <param name="n2">二番目の長整数。</param>
        /// <returns>n1 と n2 の最小公倍数。</returns>
        public static long Lcm(long n1, long n2) => n1 / Gcd(n1, n2) * n2;

        /// <summary>
        /// 指定された数値が平方数であるかどうかを確認します。
        /// </summary>
        /// <param name="num">確認したい数値。</param>
        /// <returns>num が平方数であれば true、そうでなければ false。</returns>
        public static bool CheckSquareNumber(long num)
        {
            var a = (long)Math.Sqrt(num);

            return a * a == num;
        }

        /// <summary>
        /// 指定された数値のべき乗の値を計算します。
        /// </summary>
        /// <param name="n">底。</param>
        /// <param name="p">指数。</param>
        /// <returns>n の p 乗の値。</returns>
        public static long Pow(long n, long p)
        {
            var result = 1L;

            for (long i = 0; i < p; i++)
                result *= n;

            return result;
        }

        /// <summary>
        /// 指定された数値のべき乗の剰余（Mod）を高速に計算します。（繰り返し二乗法）
        /// </summary>
        /// <param name="n">底。</param>
        /// <param name="p">指数。</param>
        /// <param name="divisor">除数。</param>
        /// <returns>(n ^ p) % divisor の結果。</returns>
        public static long ModPow(long n, long p, long divisor)
        {
            if (p == 0)
                return 1L % divisor;
            else if (p % 2L == 0L)
            {
                var x = ModPow(n, p / 2L, divisor);

                return x * x % divisor;
            }
            else
                return ModPow(n, p - 1L, divisor) * (n % divisor) % divisor;
        }

        /// <summary>
        /// 指定された数値のモジュラ逆数（Modular Multiplicative Inverse）を、指定された法（mod）の下で求めます。
        /// </summary>
        /// <param name="num">モジュラ逆数を求めたい数値。</param>
        /// <param name="mod">法（modulus）。</param>
        /// <returns>num の mod におけるモジュラ逆数。存在しない場合は -1。</returns>
        public static long ModInverse(long num, long mod)
        {
            var gcd = ExtendedGcd(num, mod);
            long g = gcd[0], x = gcd[1];

            if (g != 1)
                return -1L; // モジュラ逆数が存在しない
            else
                return (x + mod) % mod;
        }

        /// <summary>
        /// 拡張ユークリッドの互除法を用いて、ax + by = gcd(a, b) を満たす g, x, y を求めます。モジュラ逆数を求める際に使用されます。
        /// </summary>
        /// <param name="a">最初の数値。</param>
        /// <param name="b">二番目の数値。</param>
        /// <returns>配列 [gcd(a, b), x, y]。</returns>
        private static long[] ExtendedGcd(long a, long b)
        {
            if (a == 0)
                return new long[] { b, 0L, 1L };
            else
            {
                var gcd = ExtendedGcd(b % a, a);
                long g = gcd[0], x = gcd[1], y = gcd[2];

                return new long[] { g, y - b / a * x, x };
            }
        }

        /// <summary>
        /// 特定の法（divisor）におけるモジュラ逆数を効率的に取得するためのクライアントクラスです。
        /// </summary>
        public class ModInverseClient
        {
            /// <summary>
            /// 計算済みのモジュラ逆数をキャッシュするための辞書です。
            /// </summary>
            public Dictionary<long, long> _Dic;
            /// <summary>
            /// モジュラ逆数を計算する際の法（divisor）です。
            /// </summary>
            public readonly long Divisor;

            /// <summary>
            /// ModInverseClient の新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="divisor">モジュラ逆数を計算する際の法。</param>
            public ModInverseClient(long divisor)
            {
                _Dic = new();
                Divisor = divisor;
            }

            /// <summary>
            /// 指定された数値のモジュラ逆数を取得します。まだ計算されていない場合は計算し、キャッシュします。
            /// </summary>
            /// <param name="num">モジュラ逆数を求めたい数値。</param>
            /// <returns>num の Divisor におけるモジュラ逆数。存在しない場合は -1。</returns>
            public long GetModInverse(long num)
            {
                _Dic.TryAdd(num, -1L);

                if (_Dic[num] == -1L)
                    _Dic[num] = ModInverse(num, this.Divisor);

                return _Dic[num];
            }
        }

        /// <summary>
        /// 三点の座標から形成される三角形の面積を求めます。
        /// </summary>
        /// <param name="x1">一点目の X 座標。</param>
        /// <param name="y1">一点目の Y 座標。</param>
        /// <param name="x2">二点目の X 座標。</param>
        /// <param name="y2">二点目の Y 座標。</param>
        /// <param name="x3">三点目の X 座標。</param>
        /// <param name="y3">三点目の Y 座標。</param>
        /// <returns>三点で形成される三角形の面積。値が 0 の場合は 3 点は 1 直線上に存在します。</returns>
        public static decimal TriangleAreaByThreePoints(decimal x1, decimal y1, decimal x2, decimal y2, decimal x3, decimal y3)
        {
            var dec1 = (x2 - x1) * (y3 - y1);
            var dec2 = (x3 - x1) * (y2 - y1);

            return (dec1 - dec2) / 2m;
        }
    }
}