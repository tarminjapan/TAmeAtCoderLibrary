using System.Collections.Generic;
using System.Linq;
using System;

namespace TAmeAtCoderLibrary.Utilities;

/// <summary>
/// リスト操作ユーティリティクラス
/// </summary>
public class List
{
    /// <summary>
    /// 組み合わせのリストを取得する。指定した最大値までの数字から指定した桁数の組み合わせをすべて取得する。
    /// </summary>
    /// <param name="max">最大値</param>
    /// <param name="digits">組み合わせる桁数</param>
    /// <returns>組み合わせのリスト</returns>
    public static List<List<int>> CombinationLists(int max, int digits)
    {
        var list = new List<List<int>>();

        __CombitaionLists(list, new LinkedList<int>(), 1, max, digits);

        return list;
    }

    /// <summary>
    /// 組み合わせリストを再帰的に生成する補助メソッド
    /// </summary>
    /// <param name="list">生成される組み合わせリスト</param>
    /// <param name="linkedList">現在構築中の組み合わせ</param>
    /// <param name="min">使用できる最小の数値</param>
    /// <param name="max">使用できる最大の数値</param>
    /// <param name="depth">残りの桁数</param>
    private static void __CombitaionLists(List<List<int>> list, LinkedList<int> linkedList, int min, int max, int depth)
    {
        if (depth == 0)
        {
            var tlist = new List<int>();

            foreach (var current in linkedList)
                tlist.Add(current);

            list.Add(tlist);

            return;
        }

        for (int i = min; i <= max; i++)
        {
            linkedList.AddLast(i);
            __CombitaionLists(list, linkedList, i + 1, max, depth - 1);
            linkedList.RemoveLast();
        }
    }

    /// <summary>
    /// 順列のリストをすべて取得する。指定した最大値までの数字の全ての並び替えを返す。
    /// </summary>
    /// <param name="max">最大値</param>
    /// <returns>生成された順列のキュー</returns>
    public static Queue<int[]> AllPermutation(int max)
    {
        var a = Enumerable.Range(1, max).ToArray();
        var res = new Queue<int[]>();
        res.Enqueue(DeepCopy(a));
        var n = a.Length;
        var next = true;
        while (next)
        {
            next = false;

            // 1
            int i;
            for (i = n - 2; i >= 0; i--)
            {
                if (a[i].CompareTo(a[i + 1]) < 0) break;
            }
            // 2
            if (i < 0) break;

            // 3
            var j = n;
            do
            {
                j--;
            } while (a[i].CompareTo(a[j]) > 0);

            if (a[i].CompareTo(a[j]) < 0)
            {
                // 4
                var tmp = a[i];
                a[i] = a[j];
                a[j] = tmp;
                Array.Reverse(a, i + 1, n - i - 1);
                res.Enqueue(DeepCopy(a));
                next = true;
            }
        }
        return res;
    }

    /// <summary>
    /// 整数配列のディープコピーを作成する
    /// </summary>
    /// <param name="a">コピーする配列</param>
    /// <returns>ディープコピーされた配列</returns>
    private static int[] DeepCopy(int[] a)
    {
        var b = new int[a.Length];

        for (int i = 0; i < a.Length; i++)
            b[i] = a[i];

        return b;
    }
}
