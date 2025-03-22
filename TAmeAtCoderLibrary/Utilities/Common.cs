using System.Collections.Generic;

namespace TAmeAtCoderLibrary.Utilities;

/// <summary>
/// 共通のユーティリティ関数を提供するクラスです。
/// </summary>
public class Common
{
    public static char[] ALPHABETS => "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    public static char[] alphabets => "abcdefghijklmnopqrstuvwxyz".ToCharArray();

    /// <summary>
    /// 配列の累積和を生成します。
    /// </summary>
    /// <param name="array">累積和を生成する元の配列。</param>
    /// <returns>累積和を格納した配列。</returns>
    public static long[] GenerateCumSum(long[] array)
    {
        var ary = new long[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            ary[i] = array[i];

            if (1 <= i)
                ary[i] += ary[i - 1];
        }

        return ary;
    }

    /// <summary>
    /// 配列の累積和を生成します。
    /// </summary>
    /// <param name="array">累積和を生成する元の配列。</param>
    /// <returns>累積和を格納した配列。</returns>
    public static int[] GenerateCumSum(int[] array)
    {
        var ary = new int[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            ary[i] = array[i];

            if (1 <= i)
                ary[i] += ary[i - 1];
        }

        return ary;
    }

    /// <summary>
    /// 選択のリストを取得します。
    /// </summary>
    /// <param name="max">選択する数値の最大値。</param>
    /// <param name="digits">選択する桁数。</param>
    /// <returns>選択のリストを格納したキュー。</returns>
    public static Queue<Queue<int>> CombinationLists(int max, int digits)
    {
        var queue = new Queue<Queue<int>>();

        __CombitaionLists(queue, new LinkedList<int>(), 1, max, digits);

        return queue;
    }

    private static void __CombitaionLists(Queue<Queue<int>> queue, LinkedList<int> linkedList, int min, int max, int depth)
    {
        if (depth == 0)
        {
            var tqueue = new Queue<int>();

            foreach (var current in linkedList)
                tqueue.Enqueue(current);

            queue.Enqueue(tqueue);

            return;
        }

        for (int i = min; i <= max; i++)
        {
            linkedList.AddLast(i);
            __CombitaionLists(queue, linkedList, i + 1, max, depth - 1);
            linkedList.RemoveLast();
        }
    }

    /// <summary>
    /// 並び替えのリストを取得します。
    /// </summary>
    /// <param name="max">並び替える数値の最大値。</param>
    /// <param name="digits">並び替える桁数。</param>
    /// <returns>並び替えのリストを格納したキュー。</returns>
    public static Queue<Queue<int>> PermutationLists(int max, int digits)
    {
        var queue = new Queue<Queue<int>>();

        __PermurationLists(queue, new LinkedList<int>(), new bool[max + 1], max, digits);

        return queue;
    }

    private static void __PermurationLists(Queue<Queue<int>> queue, LinkedList<int> linkedList, bool[] selected, int max, int depth)
    {
        if (depth == 0)
        {
            var tqueue = new Queue<int>();

            foreach (var current in linkedList)
                tqueue.Enqueue(current);

            queue.Enqueue(tqueue);

            return;
        }

        for (int i = 1; i <= max; i++)
        {
            if (selected[i])
                continue;

            selected[i] = true;
            linkedList.AddLast(i);

            selected[i] = true;
            __PermurationLists(queue, linkedList, selected, max, depth - 1);

            linkedList.RemoveLast();
            selected[i] = false;
        }
    }

    /// <summary>
    /// 指定された数値を指定されたべき乗にします。
    /// </summary>
    /// <param name="x">べき乗する元の数値。</param>
    /// <param name="y">べき乗する指数。</param>
    /// <returns>数値 x を y 乗した結果。</returns>
    public static long Pow(long x, int y)
    {
        var pow = 1L;

        for (int i = 0; i < y; i++)
            pow *= x;

        return pow;
    }

    /// <summary>
    /// 割り算を行います。（切り上げ）
    /// </summary>
    /// <param name="bloken">割られる数。</param>
    /// <param name="divided">割る数。</param>
    /// <returns>切り上げた割り算の結果。</returns>
    public static long Ceiling(long bloken, long divided) => bloken % divided == 0L ? bloken / divided : bloken / divided + 1L;

    /// <summary>
    /// 割り算を行います。（切り上げ）
    /// </summary>
    /// <param name="bloken">割られる数。</param>
    /// <param name="divided">割る数。</param>
    /// <returns>切り上げた割り算の結果。</returns>
    public static int Ceiling(int bloken, int divided) => (int)Ceiling((long)bloken, divided);

    /// <summary>
    /// 自動フラッシュを無効にします。
    /// </summary>
    public static void DisableAutoFlush()
    { Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false }); }

    /// <summary>
    /// バッファをフラッシュします。
    /// </summary>
    public static void Flush()
    { Console.Out.Flush(); }
}