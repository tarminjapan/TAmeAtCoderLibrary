namespace TAmeAtCoderLibrary.Utilities;

/// <summary>
/// 数値変換や数値操作に関するユーティリティクラスです。
/// 様々な進数変換や数字の操作を提供します。
/// </summary>
public class Number
{
    /// <summary>
    /// 10進数からn進数に変換します。
    /// </summary>
    /// <param name="dec">変換する10進数の値</param>
    /// <param name="n">変換先の進数</param>
    /// <returns>n進数に変換された文字列</returns>
    public static string ConvertBase(long dec, long n)
    {
        if (dec == 0L) return "0";

        var stack = new Stack<long>();
        while (0L < dec)
        {
            var mod = dec % n;
            stack.Push(mod);
            dec /= n;
        }

        return string.Join("", stack);
    }

    /// <summary>
    /// 10進数から2進数に変換します。
    /// </summary>
    /// <param name="dec">変換する10進数の値</param>
    /// <returns>2進数の各桁を表す整数配列</returns>
    public static int[] ConvertToBinary(long dec)
    {
        if (dec == 0L)
            return new int[] { 0 };

        var stack = new Stack<int>();

        while (0L < dec)
        {
            var mod = dec % 2L;
            stack.Push((int)mod);
            dec /= 2L;
        }

        return stack.ToArray();
    }

    /// <summary>
    /// 10進数から2進数に変換します。最小桁数を指定できます。
    /// </summary>
    /// <param name="dec">変換する10進数の値</param>
    /// <param name="minDigits">結果の最小桁数</param>
    /// <returns>2進数の各桁を表す整数配列（指定された最小桁数以上）</returns>
    public static int[] ConvertToBinary(long dec, int minDigits)
    {
        if (dec == 0L)
            return new int[minDigits];

        var stack = new Stack<int>();

        while (0L < dec)
        {
            var mod = dec % 2L;
            stack.Push((int)mod);
            dec /= 2L;
        }

        while (stack.Count < minDigits)
            stack.Push(0);

        return stack.ToArray();
    }

    /// <summary>
    /// n進数の文字列を10進数に変換します。
    /// </summary>
    /// <param name="num">n進数を表す文字列</param>
    /// <param name="n">入力の進数</param>
    /// <returns>変換された10進数の値</returns>
    public static long ConvertToDecimal(string num, long n)
    {
        var dec = 0L;

        for (int d = 0; d < num.Length; d++)
            dec += long.Parse(num[^(d + 1)].ToString()) * (long)Math.Pow(n, d);

        return dec;
    }

    /// <summary>
    /// n進数の配列を10進数に変換します。n≦10の場合に使用します。
    /// </summary>
    /// <param name="num">n進数の各桁を表す整数配列</param>
    /// <param name="n">入力の進数</param>
    /// <returns>変換された10進数の値</returns>
    public static long ConvertToDecimal(int[] num, long n)
    {
        var dec = 0L;

        for (int d = 0; d < num.Length; d++)
            dec += (long)num[^(d + 1)] * (long)Math.Pow(n, d);

        return dec;
    }

    /// <summary>
    /// 数値を1桁ずつの整数リストに分解します。
    /// </summary>
    /// <param name="num">分解する数値</param>
    /// <returns>各桁の数字を格納した整数リスト</returns>
    public static List<int> ToIntList(long num)
    {
        var stack = new Stack<int>();

        while (0 < num)
        {
            var mod = (int)(num % 10);
            stack.Push(mod);
            num /= 10;
        }

        var list = new List<int>();

        while (stack.TryPop(out var d))
            list.Add(d);

        return list;
    }

    /// <summary>
    /// 指定した桁の数字を変更します。
    /// </summary>
    /// <param name="num">対象の数値</param>
    /// <param name="digit">新しい数字</param>
    /// <param name="position">変更する桁の位置（1ベース、右から数えて）</param>
    /// <returns>指定した桁を変更した新しい数値</returns>
    public static long ChangeNum(long num, long digit, int position)
    {
        var num1 = num / Common.Pow(10, position) * Common.Pow(10, position);
        var num2 = num % Common.Pow(10, position - 1);
        var num3 = digit * Common.Pow(10, position - 1);

        return num1 + num2 + num3;
    }

    /// <summary>
    /// 指定した2箇所の桁の数字を入れ替えます。9が含まれる場合のみ入れ替えを行います。
    /// </summary>
    /// <param name="num">対象の数値</param>
    /// <param name="position1">入れ替える1つ目の桁の位置（1ベース、右から数えて）</param>
    /// <param name="position2">入れ替える2つ目の桁の位置（1ベース、右から数えて）</param>
    /// <returns>指定した桁を入れ替えた新しい数値。9を含まない場合は元の数値をそのまま返します。</returns>
    public static long ReplaceNum(long num, int position1, int position2)
    {
        var digit1 = num / Common.Pow(10, position1 - 1) % 10L;
        var digit2 = num / Common.Pow(10, position2 - 1) % 10L;

        if (digit1 != 9 && digit2 != 9) return num;

        num = ChangeNum(num, digit2, position1);
        num = ChangeNum(num, digit1, position2);

        return num;
    }
}
