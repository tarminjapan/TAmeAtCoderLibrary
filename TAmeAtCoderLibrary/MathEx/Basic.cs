using System.Collections.Concurrent;

namespace TAmeAtCoderLibrary;

/// <summary>
/// 数学的な基本的な静的メソッドを提供します。
/// </summary>
public static partial class MathEx // partial は削除 (単一ファイルの場合)
{
    #region Basic Arithmetic & Power

    /// <summary>
    /// 指定された底を非負整数乗した値を計算します。（繰り返し二乗法）
    /// オーバーフローは例外をスローします。負の指数には対応していません。
    /// </summary>
    /// <param name="baseValue">累乗される底。</param>
    /// <param name="exponent">指数（非負整数）。</param>
    /// <returns>baseValue の exponent 乗。指数が 0 の場合は 1。</returns>
    /// <exception cref="ArgumentOutOfRangeException">指数が負の場合。</exception>
    /// <exception cref="OverflowException">計算結果が long の範囲を超える場合。</exception>
    public static long Power(long baseValue, long exponent)
    {
        if (exponent < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(exponent), "指数は負であってはなりません。");
        }
        if (exponent == 0) return 1L;
        // 簡単なケースを早期に処理
        if (baseValue == 0) return 0L;
        if (baseValue == 1) return 1L;
        if (baseValue == -1) return (exponent % 2 == 0) ? 1L : -1L;

        long result = 1L;
        long currentPower = baseValue;

        try
        {
            checked // オーバーフローのチェックを有効化
            {
                while (exponent > 0)
                {
                    if ((exponent % 2) == 1) // 指数が奇数の場合
                    {
                        result = result * currentPower;
                    }

                    exponent /= 2;

                    if (exponent > 0) // 最後のステップ後に二乗を避ける
                        currentPower = checked(currentPower * currentPower);
                }
            }
        }
        catch (OverflowException ex)
        {
            // 必要に応じて追加のコンテキストを提供
            throw new OverflowException($"累乗の計算中にオーバーフローが発生しました。", ex);
        }
        return result;
    }

    /// <summary>
    /// 指定された非負整数の平方根の整数部分を計算します。（二分探索法）但し、sqrt値の最大値はint32まで対応
    /// </summary>
    /// <param name="number">平方根を求めたい非負整数。</param>
    /// <returns>number の平方根の整数部分（切り捨て）。例: Sqrt(8) = 2, Sqrt(9) = 3</returns>
    /// <exception cref="ArgumentOutOfRangeException">number が負の場合。</exception>
    public static long Sqrt(long number)
    {
        if (number < 0)
            throw new ArgumentOutOfRangeException(nameof(number), "入力は非負整数である必要があります。");

        // int32に収まる場合は標準の関数を使用する。（パフォーマンス向上のため）
        if (number <= int.MaxValue)
            return (long)Math.Sqrt(number);

        return SqrtInternal(0L, number, number);
    }

    /// <summary>
    /// 二分探索を使用して整数の平方根を計算します。（内部実装）　但し、sqrt値の最大値はint32まで対応
    /// </summary>
    /// <param name="left">探索区間の下限値。</param>
    /// <param name="right">探索区間の上限値。</param>
    /// <param name="number">平方根を求めたい値。</param>
    /// <returns>number の平方根の整数部分。</returns>
    private static long SqrtInternal(long left, long right, long number)
    {
        right = Math.Max((long)int.MaxValue, right);

        while (left < right)
        {
            // 切り上げで中間値を計算（探索区間を適切に縮小するため）
            var middle = Ceiling(left + right, 2L);

            // 中間値の二乗を計算
            try
            {
                var squared = checked(middle * middle);

                if (squared <= number)
                    left = middle;
                else
                    right = middle - 1L;
            }
            catch (OverflowException)
            {
                right = middle - 1L;
                continue;
            }
        }

        // 探索終了時の下限値が平方根の整数部分
        return left;
    }

    /// <summary>
    /// 整数除算を行い、結果を天井関数（正の無限大方向への丸め）で求めます。
    /// </summary>
    /// <param name="dividend">割られる数（被除数）。</param>
    /// <param name="divisor">割る数（除数）。</param>
    /// <returns>dividend を divisor で割った結果の切り上げ。</returns>
    /// <exception cref="DivideByZeroException">divisor が 0 の場合。</exception>
    public static long Ceiling(long dividend, long divisor)
    {
        if (divisor == 0) throw new DivideByZeroException("除数は 0 であってはなりません。");

        long quotient = dividend / divisor;
        long remainder = dividend % divisor;

        // 剰余があり、かつ除算がゼロ方向に切り捨てられた場合、正の無限大方向に丸める必要がある場合は商を増加
        // これは、dividend と divisor が同じ符号を持つ場合に発生します。
        if (remainder != 0 && (dividend > 0 == divisor > 0)) // 同符号チェック
        {
            // 増加前にオーバーフローをチェック
            if (quotient == long.MaxValue) throw new OverflowException("天井計算によりオーバーフローが発生しました。");
            quotient++;
        }
        return quotient;
    }

    /// <summary>
    /// 指定された数値の桁数を取得します（10進数）。
    /// </summary>
    /// <param name="number">桁数を調べたい数値。</param>
    /// <returns>number の桁数。number が 0 の場合は 1 を返します。</returns>
    public static int CountDigits(long number)
    {
        if (number == 0) return 1;

        int count = 0;
        long absNum = Math.Abs(number);
        // long.MinValue の場合を特別に処理（Math.Abs(long.MinValue) == long.MinValue）
        if (number == long.MinValue) absNum = long.MaxValue; // または別途処理

        while (absNum > 0)
        {
            absNum /= 10;
            count++;
        }
        if (number == long.MinValue) return 19; // long.MinValue の桁数を正しく返す
        return count;
    }

    #endregion

    #region Divisors, GCD, LCM

    /// <summary>
    /// 指定された正の長整数の約数を昇順で取得します。
    /// </summary>
    /// <param name="n">約数を求めたい正の長整数。</param>
    /// <returns>n の約数を格納したソート済みのセット。</returns>
    /// <exception cref="ArgumentOutOfRangeException">n が 0 以下の場合。</exception>
    public static SortedSet<long> Divisors(long n)
    {
        if (n <= 0) throw new ArgumentOutOfRangeException(nameof(n), "入力は正の整数である必要があります。");

        var divisors = new SortedSet<long>();
        // sqrt(n) までチェック
        long limit = Sqrt(n);
        // 非常に大きな n に対して、limit チェックがオーバーフロー問題を引き起こさないようにする
        if (limit > long.MaxValue / limit && n > limit * limit) limit++; // 必要に応じて調整

        for (long i = 1; i <= limit; i++)
        {
            // 最適化: i*i がオーバーフローする場合、i はすでに sqrt(n) より大きい
            try { checked { var ii = i * i; } } catch (OverflowException) { break; }

            if (n % i == 0)
            {
                divisors.Add(i);
                long quotient = n / i;
                if (i != quotient) // 平方根を二重に追加しないようにする
                {
                    divisors.Add(quotient);
                }
            }
        }
        return divisors;
    }

    /// <summary>
    /// 指定された二つの長整数の最大公約数（GCD）を求めます。結果は非負です。
    /// ユークリッドの互除法を使用します。
    /// </summary>
    /// <param name="a">最初の長整数。</param>
    /// <param name="b">二番目の長整数。</param>
    /// <returns>a と b の最大公約数（非負）。</returns>
    public static long Gcd(long a, long b)
    {
        // 特殊ケース: Math.Abs(long.MinValue) は OverflowException をスローします
        if (a == long.MinValue && b == long.MinValue) return long.MaxValue; // 必要に応じて定義、技術的には無限の約数
        if (a == long.MinValue) a = long.MaxValue; // 近似値または特別に処理
        if (b == long.MinValue) b = long.MaxValue; // 近似値または特別に処理
                                                   // 代替案: Abs の前にチェック
        if (b == 0) return Math.Abs(a); // a が MinValue の場合、Abs(MinValue) を正しく処理
        if (a == 0) return Math.Abs(b);

        a = Math.Abs(a);
        b = Math.Abs(b);

        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    /// <summary>
    /// 指定された長整数の配列の最大公約数（GCD）を求めます。結果は非負です。
    /// </summary>
    /// <param name="numbers">最大公約数を求めたい長整数の配列。null や空であってはなりません。</param>
    /// <returns>配列 numbers に含まれるすべての長整数の最大公約数（非負）。</returns>
    /// <exception cref="ArgumentNullException">配列が null の場合。</exception>
    /// <exception cref="ArgumentException">配列が空の場合。</exception>
    public static long Gcd(params long[] numbers)
    {
        if (numbers == null) throw new ArgumentNullException(nameof(numbers));
        if (numbers.Length == 0) throw new ArgumentException("入力配列は空であってはなりません。", nameof(numbers));

        // Abs の前に long.MinValue を処理
        if (numbers.Length == 1) return numbers[0] == long.MinValue ? long.MaxValue : Math.Abs(numbers[0]); // 必要に応じて GCD(MinValue) を定義

        long result = numbers[0] == long.MinValue ? long.MaxValue : Math.Abs(numbers[0]); // 初期値は MinValue を考慮
        for (int i = 1; i < numbers.Length; i++)
        {
            long currentNum = numbers[i] == long.MinValue ? long.MaxValue : Math.Abs(numbers[i]);
            result = Gcd(result, currentNum);
            if (result == 1) break; // 最適化
        }
        return result;

        // Aggregate アプローチ（MinValue の処理が必要）
        // return numbers.Select(n => n == long.MinValue ? long.MaxValue : Math.Abs(n)).Aggregate(Gcd);
    }

    /// <summary>
    /// 指定された二つの長整数の最小公倍数（LCM）を求めます。結果は非負です。
    /// </summary>
    /// <param name="a">最初の長整数。</param>
    /// <param name="b">二番目の長整数。</param>
    /// <returns>a と b の最小公倍数（非負）。</returns>
    /// <exception cref="OverflowException">計算結果が long の範囲を超える場合。</exception>
    public static long Lcm(long a, long b)
    {
        if (a == 0 || b == 0) return 0L;

        // 絶対値を使用し、GCD で MinValue を慎重に処理
        long gcdVal = Gcd(a, b); // この GCD が MinValue を処理し、非負を返すことを確認

        // LCM = |a * b| / GCD(a, b) を計算
        // 中間オーバーフローを避けるため、|a| / GCD(a, b) * |b| として計算
        long absA = a == long.MinValue ? long.MaxValue : Math.Abs(a); // MinValue に対して近似値を使用
        long absB = b == long.MinValue ? long.MaxValue : Math.Abs(b); // MinValue に対して近似値を使用

        // GCD がゼロの場合のゼロ除算をチェック（a,b != 0 の場合は発生しないはず）
        if (gcdVal == 0) return 0L; // または例外をスロー？ GCD(a,b) がゼロになるのは a=b=0 の場合のみ、上で処理済み。

        long part1 = absA / gcdVal;

        try
        {
            // 乗算前にオーバーフローをチェック
            return checked(part1 * absB);
        }
        catch (OverflowException ex)
        {
            throw new OverflowException($"LCM の計算中にオーバーフローが発生しました。{a} と {b} の場合。", ex);
        }
    }

    /// <summary>
    /// 指定された長整数の配列の最小公倍数（LCM）を求めます。結果は非負です。
    /// </summary>
    /// <param name="numbers">最小公倍数を求めたい長整数の配列。null や空であってはなりません。</param>
    /// <returns>配列 numbers に含まれるすべての長整数の最小公倍数（非負）。</returns>
    /// <exception cref="ArgumentNullException">配列が null の場合。</exception>
    /// <exception cref="ArgumentException">配列が空の場合。</exception>
    /// <exception cref="OverflowException">計算中に結果が long の範囲を超える場合。</exception>
    public static long Lcm(params long[] numbers)
    {
        if (numbers == null) throw new ArgumentNullException(nameof(numbers));
        if (numbers.Length == 0) throw new ArgumentException("入力配列は空であってはなりません。", nameof(numbers));

        // 数値が 0 の場合、LCM は 0
        if (numbers.Any(n => n == 0)) return 0L;

        // 二引数の Lcm 関数を使用して Aggregate
        try
        {
            return numbers.Aggregate(Lcm);
        }
        catch (OverflowException ex)
        {
            // 二引数の Lcm からのオーバーフローをキャッチし、必要に応じて再スロー
            throw new OverflowException("配列の LCM 計算中にオーバーフローが発生しました。", ex);
        }
    }

    #endregion

    #region Modular Arithmetic

    /// <summary>
    /// 二つの数を加算し、指定された正の除数による剰余（常に非負）を返します。
    /// </summary>
    /// <param name="a">加算する最初の数値。</param>
    /// <param name="b">加算する二番目の数値。</param>
    /// <param name="modulus">剰余計算に使用する正の除数。</param>
    /// <returns>(a + b) % modulus の結果（0 以上 modulus 未満）。</returns>
    /// <exception cref="ArgumentOutOfRangeException">modulus が 1 未満の場合。</exception>
    public static long ModAdd(long a, long b, long modulus)
    {
        if (modulus <= 0) throw new ArgumentOutOfRangeException(nameof(modulus), "除数は正である必要があります。");
        // 結果が非負になるようにする
        long result = (a % modulus + b % modulus) % modulus;
        return result < 0 ? result + modulus : result;
    }

    /// <summary>
    /// 指定された数値のべき乗の剰余（Mod）を高速に計算します。（繰り返し二乗法）
    /// 指数が負の場合はモジュラ逆数を使用します。
    /// </summary>
    /// <param name="baseValue">底。</param>
    /// <param name="exponent">指数。</param>
    /// <param name="modulus">正の除数。</param>
    /// <returns>(baseValue ^ exponent) % modulus の結果（0 以上 modulus 未満）。</exception>
    /// <exception cref="ArgumentOutOfRangeException">modulus が 1 未満の場合。</exception>
    /// <exception cref="ArithmeticException">負の指数で、かつモジュラ逆数が存在しない場合。</exception>
    public static long ModPow(long baseValue, long exponent, long modulus)
    {
        if (modulus <= 0) throw new ArgumentOutOfRangeException(nameof(modulus), "除数は正である必要があります。");
        if (modulus == 1) return 0L; // 除数が 1 の場合、剰余は常に 0

        // 負の指数をモジュラ逆数を使用して処理
        if (exponent < 0)
        {
            long inverseBase = ModInverse(baseValue, modulus);
            if (inverseBase == -1)
            {
                throw new ArithmeticException($"モジュラ逆数 {baseValue} mod {modulus} が存在しないため、負の指数を計算できません。");
            }
            // (base^-1)^(-exponent) mod modulus を計算
            // exponent が long.MinValue の場合、-exponent のオーバーフローに注意
            if (exponent == long.MinValue)
            {
                // この特定のエッジケースを処理: (base^-1)^long.MaxValue * (base^-1)
                long part1 = ModPow(inverseBase, long.MaxValue, modulus);
                return (part1 * inverseBase) % modulus;
            }
            return ModPow(inverseBase, -exponent, modulus);
        }

        // 底を [0, modulus - 1] の範囲に正規化
        long B = (baseValue % modulus + modulus) % modulus;
        long result = 1L;

        while (exponent > 0)
        {
            if (exponent % 2 == 1)
            {
                result = (result * B) % modulus;
            }
            B = (B * B) % modulus;
            exponent /= 2;
        }
        return result;
    }

    /// <summary>
    /// 拡張ユークリッドの互除法を用いて、a*x + b*y = gcd(a, b) を満たす (gcd, x, y) を求めます。
    /// 結果の gcd は非負です。
    /// </summary>
    /// <param name="a">最初の数値。</param>
    /// <param name="b">二番目の数値。</param>
    /// <returns>タプル (Gcd, X, Y)。Gcd は a と b の最大公約数（非負）、X, Y はベズー係数。</returns>
    private static (long Gcd, long X, long Y) ExtendedGcd(long a, long b)
    {
        // 標準的な再帰的拡張ユークリッドアルゴリズムに基づく
        // 非負の GCD 結果を保証
        if (a == 0)
        {
            return (Math.Abs(b), 0L, Math.Sign(b)); // Abs(b) を返し、y の符号を調整
        }

        var (g, x1, y1) = ExtendedGcd(b % a, a);
        long x = y1 - (b / a) * x1;
        long y = x1;

        return (g, x, y); // g は基底ケースから非負になる
    }

    /// <summary>
    /// 指定された数値のモジュラ逆数（Modular Multiplicative Inverse）を、指定された正の法（mod）の下で求めます。
    /// </summary>
    /// <param name="number">モジュラ逆数を求めたい数値。</param>
    /// <param name="modulus">正の法（modulus）。</param>
    /// <returns>number の modulus におけるモジュラ逆数（0 以上 modulus 未満）。存在しない場合は -1。</returns>
    /// <exception cref="ArgumentOutOfRangeException">modulus が 1 未満の場合。</exception>
    public static long ModInverse(long number, long modulus)
    {
        if (modulus <= 0) throw new ArgumentOutOfRangeException(nameof(modulus), "除数は正である必要があります。");
        if (modulus == 1) return 0; // 除数が 1 の場合、逆数は 0

        // ExtendedGcd を使用して負数を正しく処理するために number を正規化
        long numNormalized = (number % modulus + modulus) % modulus;
        if (numNormalized == 0 && modulus != 1) return -1; // 0 の逆数は除数が 1 の場合のみ存在

        var (g, x, y) = ExtendedGcd(numNormalized, modulus);

        if (g != 1)
        {
            return -1L; // モジュラ逆数が存在しない（number と modulus が互いに素でない）
        }
        else
        {
            // 結果 x が非負になるようにする
            return (x % modulus + modulus) % modulus;
        }
    }

    /// <summary>
    /// 特定の法（modulus）におけるモジュラ算術を効率的に行うためのクライアントクラスです。
    /// スレッドセーフです。
    /// </summary>
    public class ModularArithmetic
    {
        private readonly ConcurrentDictionary<long, long> _inverseCache;

        /// <summary>
        /// モジュラ算術の法（modulus）です。正の整数である必要があります。
        /// </summary>
        public long Modulus { get; }

        /// <summary>
        /// ModularArithmetic の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="modulus">モジュラ算術の法（正の整数）。</param>
        /// <exception cref="ArgumentOutOfRangeException">modulus が 1 未満の場合。</exception>
        public ModularArithmetic(long modulus)
        {
            if (modulus <= 0) throw new ArgumentOutOfRangeException(nameof(modulus), "法は正である必要があります。");
            _inverseCache = new ConcurrentDictionary<long, long>();
            Modulus = modulus;
        }

        /// <summary>
        /// 指定された数値のモジュラ逆数を取得します。キャッシュを利用し、必要なら計算します。
        /// </summary>
        /// <param name="number">モジュラ逆数を求めたい数値。</param>
        /// <returns>number の Modulus におけるモジュラ逆数（0 以上 Modulus 未満）。存在しない場合は -1。</returns>
        public long Inverse(long number)
        {
            if (Modulus == 1) return 0;

            long normalizedNum = (number % Modulus + Modulus) % Modulus;
            if (normalizedNum == 0) return -1; // 法が 1 より大きい場合、0 の逆数は存在しない

            return _inverseCache.GetOrAdd(normalizedNum, num => MathEx.ModInverse(num, Modulus));
        }

        /// <summary>
        /// 二つの数を加算し、法（Modulus）による剰余を返します。
        /// </summary>
        public long Add(long a, long b) => ((a % Modulus) + (b % Modulus) + Modulus) % Modulus;

        /// <summary>
        /// 二つの数を減算し、法（Modulus）による剰余を返します。
        /// </summary>
        public long Subtract(long a, long b) => ((a % Modulus) - (b % Modulus) + Modulus) % Modulus;

        /// <summary>
        /// 二つの数を乗算し、法（Modulus）による剰余を返します。
        /// </summary>
        public long Multiply(long a, long b) => ((a % Modulus) * (b % Modulus) + Modulus) % Modulus;

        /// <summary>
        /// 数のべき乗を計算し、法（Modulus）による剰余を返します。
        /// </summary>
        public long Power(long baseValue, long exponent) => MathEx.ModPow(baseValue, exponent, Modulus);

        /// <summary>
        /// 除算（乗法逆元を使用）を行い、法（Modulus）による剰余を返します。
        /// </summary>
        /// <exception cref="InvalidOperationException">逆元が存在しない場合。</exception>
        public long Divide(long dividend, long divisor)
        {
            long inverseDivisor = Inverse(divisor);
            if (inverseDivisor == -1)
            {
                throw new InvalidOperationException($"法 {Modulus} において {divisor} の逆元は存在しません。");
            }
            return Multiply(dividend, inverseDivisor);
        }
    }

    #endregion

    #region Other Utility Methods

    /// <summary>
    /// 指定された数値が完全平方数であるかどうかを確認します。
    /// </summary>
    /// <param name="number">確認したい数値（非負）。</param>
    /// <returns>number が完全平方数であれば true、そうでなければ false。</returns>
    /// <exception cref="ArgumentOutOfRangeException">number が負の場合。</exception>
    public static bool IsPerfectSquare(long number)
    {
        if (number < 0) throw new ArgumentOutOfRangeException(nameof(number), "入力数値は負であってはなりません。");
        if (number == 0) return true;
        // 最後の桁をチェック（簡易フィルタ） - 平方数は 0, 1, 4, 5, 6, 9 で終わる
        int lastDigit = (int)(number % 10);
        if (lastDigit == 2 || lastDigit == 3 || lastDigit == 7 || lastDigit == 8) return false;

        // 整数平方根チェックを使用
        long root = Sqrt(number);
        return root * root == number;
    }

    /// <summary>
    /// 三点の座標から形成される三角形の符号付き面積を求めます。
    /// 面積の絶対値が実際の面積です。符号は点の順序（時計回り/反時計回り）を示します。
    /// </summary>
    /// <param name="x1">一点目の X 座標。</param>
    /// <param name="y1">一点目の Y 座標。</param>
    /// <param name="x2">二点目の X 座標。</param>
    /// <param name="y2">二点目の Y 座標。</param>
    /// <param name="x3">三点目の X 座標。</param>
    /// <param name="y3">三点目の Y 座標。</param>
    /// <returns>三点で形成される三角形の符号付き面積。値が 0 の場合は 3 点は同一直線上にあります。</returns>
    public static decimal TriangleSignedArea(decimal x1, decimal y1, decimal x2, decimal y2, decimal x3, decimal y3)
    {
        // 行列式の公式を使用（靴紐公式 / クロス積のz成分に関連）
        // 面積 = 0.5 * ((x2 - x1)(y3 - y1) - (x3 - x1)(y2 - y1))
        // 倍精度浮動小数点数より高い精度を持つ可能性があるため、decimal型を使用
        try
        {
            checked // 中間計算中にオーバーフローをチェックする必要がある場合
            {
                var term1 = (x2 - x1) * (y3 - y1);
                var term2 = (x3 - x1) * (y2 - y1);
                return (term1 - term2) / 2m;
            }
        }
        catch (OverflowException ex)
        {
            throw new OverflowException("TriangleSignedArea の中間計算中にオーバーフローが発生しました。", ex);
        }
    }

    #endregion
}