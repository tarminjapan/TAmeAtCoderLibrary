namespace TAmeAtCoderLibrary;

public class ReadLine
{
    public static double Double() => double.Parse(String());
    public static double[] Doubles() => [.. Strings().Select(double.Parse)];
    public static double[] Doubles(int height) => [.. Enumerable.Range(0, height).Select(_ => Double())];
    public static double[][] DoubleMatrix(int height) => [.. Enumerable.Range(0, height).Select(_ => Doubles())];
    public static decimal Decimal() => decimal.Parse(String());
    public static decimal[] Decimals() => [.. Strings().Select(decimal.Parse)];
    public static decimal[] Decimals(int height) => [.. Enumerable.Range(0, height).Select(_ => Decimal())];
    public static decimal[][] DecimalMatrix(int height) => [.. Enumerable.Range(0, height).Select(_ => Decimals())];
    public static int Int() => int.Parse(String());
    public static int[] Ints() => [.. Strings().Select(int.Parse)];
    public static int[] Ints(int height) => [.. Enumerable.Range(0, height).Select(_ => Int())];
    public static int[][] IntMatrix(int height) => [.. Enumerable.Range(0, height).Select(_ => Ints())];
    public static long Long() => long.Parse(String());
    public static long[] Longs() => [.. Strings().Select(long.Parse)];
    public static long[] Longs(int height) => [.. Enumerable.Range(0, height).Select(_ => Long())];
    public static long[][] LongMatrix(long height) => [.. Enumerable.Range(0, (int)height).Select(_ => Longs())];
    public static string String() => Console.ReadLine().TrimStart().TrimEnd();
    public static string[] Strings() => Console.ReadLine().TrimStart().TrimEnd().Split();
    public static string[] Strings(int height) => [.. Enumerable.Range(0, height).Select(_ => String())];
    public static string[][] StringMatrix(int height) => [.. Enumerable.Range(0, height).Select(_ => Strings())];
    public static char[][] CharMatrix(int height) => [.. Enumerable.Range(0, height).Select(_ => String().ToCharArray())];
}