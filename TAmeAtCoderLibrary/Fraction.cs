namespace TAmeAtCoderLibrary;

/// <summary>
/// 分数を表すクラスです。分子と分母を管理し、基本的な算術演算や比較処理を提供いたします。
/// </summary>
public class Fraction : IComparable<Fraction>, IEquatable<Fraction>
{
    /// <summary>
    /// 分子：分数の上部の数値を表します。
    /// </summary>
    public long Numerator { get; private set; }

    /// <summary>
    /// 分母：分数の下部の数値を表します。
    /// </summary>
    public long Denominator { get; private set; }

    /// <summary>
    /// 単一の整数値から分数オブジェクトを初期化します。
    /// </summary>
    /// <param name="numerator">初期化に使用する整数値（分子）。分母は自動的に1に設定されます。</param>
    public Fraction(long numerator = 0)
    {
        this.Numerator = numerator;
        this.Denominator = 1;
    }

    /// <summary>
    /// 分子と分母を指定して分数オブジェクトを初期化します。
    /// </summary>
    /// <param name="numerator">分子となる整数値。</param>
    /// <param name="denominator">分母となる整数値。0より大きい値である必要があります。</param>
    /// <exception cref="DivideByZeroException">分母が0の場合にスローされます。</exception>
    public Fraction(long numerator, long denominator)
    {
        if (denominator == 0)
            throw new DivideByZeroException("分母に0を指定することはできません。");

        this.Numerator = numerator;
        this.Denominator = denominator;
        Normal();
    }

    /// <summary>
    /// 指定したFractionオブジェクトのコピーを作成します。
    /// </summary>
    /// <param name="b">コピー元のFractionオブジェクト。</param>
    public Fraction(Fraction b)
    {
        this.Numerator = b.Numerator;
        this.Denominator = b.Denominator;
    }

    /// <summary>
    /// 2つの整数の最大公約数（GCD）を計算いたします。
    /// </summary>
    /// <param name="n1">1つ目の整数値。</param>
    /// <param name="n2">2つ目の整数値。</param>
    /// <returns>n1とn2の最大公約数を返します。</returns>
    public static long Gcd(long n1, long n2)
    {
        n1 = Math.Abs(n1);
        n2 = Math.Abs(n2);

        while (n2 != 0)
        {
            var temp = n2;
            n2 = n1 % n2;
            n1 = temp;
        }

        return n1;
    }

    /// <summary>
    /// 2つの整数の最小公倍数（LCM）を計算いたします。
    /// </summary>
    /// <param name="n1">1つ目の整数値。</param>
    /// <param name="n2">2つ目の整数値。</param>
    /// <returns>n1とn2の最小公倍数を返します。</returns>
    /// <remarks>オーバーフローを回避するため、計算順序を工夫しています。</remarks>
    public static long Lcm(long n1, long n2)
    {
        if (n1 == 0 || n2 == 0) return 0;

        return Math.Abs(n1) / Gcd(n1, n2) * Math.Abs(n2);
    }

    /// <summary>
    /// 分数を正規化（簡素化）いたします。符号の調整や既約分数への変換を行います。
    /// </summary>
    void Normal()
    {
        if (Denominator < 0)
        {
            Numerator *= -1;
            Denominator *= -1;
        }

        var gcd = Gcd(Math.Abs(Numerator), Denominator);

        Numerator /= gcd;
        Denominator /= gcd;
    }

    /// <summary>
    /// 分数の文字列表現を返します。
    /// </summary>
    /// <returns>分母が1の場合は整数の文字列、それ以外は "分子/分母" の形式の文字列を返します。</returns>
    public override string ToString()
    {
        if (Denominator == 1)
            return Numerator.ToString();
        else
            return Numerator + "/" + Denominator;
    }

    /// <summary>
    /// 指定したオブジェクトとこの分数が等しいかどうかを比較いたします。
    /// </summary>
    /// <param name="b">比較対象のオブジェクト。</param>
    /// <returns>等しければtrue、そうでなければfalseを返します。</returns>
    public override bool Equals(object b)
    {
        if (b is Fraction b2 && this.Numerator == b2.Numerator && this.Denominator == b2.Denominator)
            return true;

        return false;
    }

    /// <summary>
    /// 指定されたオブジェクトが現在のオブジェクトと等しいかどうかを判断します。
    /// </summary>
    /// <param name="other">現在のオブジェクトと比較する分数。</param>
    /// <returns>指定されたオブジェクトが現在のオブジェクトと等しい場合はtrue。</returns>
    public bool Equals(Fraction other)
    {
        if (other is null) return false;
        return this.Numerator == other.Numerator && this.Denominator == other.Denominator;
    }

    /// <summary>
    /// 2つの分数を比較し、その大小関係を整数で返します。
    /// </summary>
    /// <param name="a">1つ目の分数。</param>
    /// <param name="b">2つ目の分数。</param>
    /// <returns>
    /// 0の場合は等しい、正の値の場合はaが大きい、負の値の場合はaが小さいことを示します。
    /// </returns>
    public static long Compare(Fraction a, Fraction b)
    {
        var lcm = Lcm(a.Denominator, b.Denominator);

        return lcm / a.Denominator * a.Numerator - lcm / b.Denominator * b.Numerator;
    }

    public static bool operator ==(Fraction a, Fraction b) => a.Equals(b);
    public static bool operator !=(Fraction a, Fraction b) => !a.Equals(b);
    public static bool operator >=(Fraction a, Fraction b) => Compare(a, b) >= 0;
    public static bool operator >(Fraction a, Fraction b) => Compare(a, b) > 0;
    public static bool operator <=(Fraction a, Fraction b) => Compare(a, b) <= 0;
    public static bool operator <(Fraction a, Fraction b) => Compare(a, b) < 0;

    /// <summary>
    /// この分数を指定した分数と比較いたします。
    /// </summary>
    /// <param name="otherfrac">比較対象の分数。</param>
    /// <returns>
    /// この分数が大きい場合は1、小さい場合は-1、等しい場合は0を返します。
    /// </returns>
    public int CompareTo(Fraction otherfrac)
    {
        if (this > otherfrac) return 1;
        else if (this < otherfrac) return -1;
        else return 0;
    }

    /// <summary>
    /// 2つの分数の加算を行い、その結果の分数を返します。
    /// </summary>
    /// <param name="a">1つ目の分数。</param>
    /// <param name="b">2つ目の分数。</param>
    /// <returns>加算結果を正規化した分数オブジェクト。</returns>
    public static Fraction operator +(Fraction a, Fraction b)
    {
        var lcm = Lcm(a.Denominator, b.Denominator);
        var numer = a.Numerator * (lcm / a.Denominator) + b.Numerator * (lcm / b.Denominator);

        return new Fraction(numer, lcm);
    }

    /// <summary>
    /// 2つの分数の減算を行い、その結果の分数を返します。
    /// </summary>
    /// <param name="a">被減数の分数。</param>
    /// <param name="b">減数の分数。</param>
    /// <returns>減算結果を正規化した分数オブジェクト。</returns>
    public static Fraction operator -(Fraction a, Fraction b)
    {
        var lcm = Lcm(a.Denominator, b.Denominator);
        var numer = a.Numerator * (lcm / a.Denominator) - b.Numerator * (lcm / b.Denominator);

        return new Fraction(numer, lcm);
    }

    /// <summary>
    /// 2つの分数の乗算を行い、その結果の分数を返します。
    /// </summary>
    /// <param name="a">1つ目の分数。</param>
    /// <param name="b">2つ目の分数。</param>
    /// <returns>乗算結果を正規化した分数オブジェクト。</returns>
    public static Fraction operator *(Fraction a, Fraction b)
    {
        var gcd = Gcd(a.Numerator, b.Denominator);
        var numer = a.Numerator / gcd;
        var denom = b.Denominator / gcd;

        gcd = Gcd(b.Numerator, a.Denominator);
        numer *= b.Numerator / gcd;
        denom *= a.Denominator / gcd;

        return new Fraction(numer, denom);
    }

    /// <summary>
    /// 2つの分数の除算を行い、その結果の分数を返します。
    /// </summary>
    /// <param name="a">被除数の分数。</param>
    /// <param name="b">除数の分数。</param>
    /// <returns>除算結果を正規化した分数オブジェクト。</returns>
    /// <exception cref="DivideByZeroException">除数の分子が0の場合にスローされます。</exception>
    public static Fraction operator /(Fraction a, Fraction b)
    {
        if (b.Numerator == 0)
            throw new DivideByZeroException("0で除算することはできません。");

        var gcd = Gcd(a.Numerator, b.Numerator);
        var numer = a.Numerator / gcd;
        var denom = b.Numerator / gcd;

        gcd = Gcd(b.Denominator, a.Denominator);
        numer *= b.Denominator / gcd;
        denom *= a.Denominator / gcd;

        return new Fraction(numer, denom);
    }

    /// <summary>
    /// 単項のプラス演算子。引数の分数をそのまま返します。
    /// </summary>
    /// <param name="a">対象の分数。</param>
    /// <returns>渡された分数と同一の分数オブジェクト。</returns>
    public static Fraction operator +(Fraction a) => new Fraction(a);

    /// <summary>
    /// 単項のマイナス演算子。分子の符号を反転させた分数を返します。
    /// </summary>
    /// <param name="a">対象の分数。</param>
    /// <returns>符号が反転した分数オブジェクト。</returns>
    public static Fraction operator -(Fraction a)
    {
        var b = new Fraction(a);
        b.Numerator *= -1;

        return b;
    }

    /// <summary>
    /// インクリメント演算子。分子の値を1増加させ、正規化した新たな分数を返します。
    /// </summary>
    /// <param name="a">対象の分数。</param>
    /// <returns>分子をインクリメントした分数オブジェクト。</returns>
    public static Fraction operator ++(Fraction a)
    {
        a.Numerator++;
        a.Normal();

        return new Fraction(a);
    }

    /// <summary>
    /// デクリメント演算子。分子の値を1減少させ、正規化した新たな分数を返します。
    /// </summary>
    /// <param name="a">対象の分数。</param>
    /// <returns>分子をデクリメントした分数オブジェクト。</returns>
    public static Fraction operator --(Fraction a)
    {
        a.Numerator--;
        a.Normal();

        return new Fraction(a);
    }

    /// <summary>
    /// long型からFraction型へ暗黙的に変換いたします。
    /// </summary>
    /// <param name="a">変換元のlong型数値。分子に設定され、分母は1となります。</param>
    /// <returns>変換されたFractionオブジェクト。</returns>
    public static implicit operator Fraction(long a) => new Fraction(a);

    /// <summary>
    /// このオブジェクトのハッシュコードを取得いたします。
    /// </summary>
    /// <returns>ハッシュコード。既約分数表現に基づいて計算されます。</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Numerator, Denominator);
    }

    /// <summary>
    /// 分数値を倍精度浮動小数点数として取得します。
    /// </summary>
    /// <returns>分数の小数表現。</returns>
    public double ToDouble()
    {
        return (double)Numerator / Denominator;
    }

    /// <summary>
    /// 分数値を単精度浮動小数点数として取得します。
    /// </summary>
    /// <returns>分数の小数表現。</returns>
    public float ToSingle()
    {
        return (float)Numerator / Denominator;
    }

    /// <summary>
    /// 分数値を十進数として取得します。
    /// </summary>
    /// <returns>分数の十進数表現。</returns>
    public decimal ToDecimal()
    {
        return (decimal)Numerator / Denominator;
    }
}
