#nullable enable
using System.Numerics;

namespace TAmeAtCoderLibrary.Utilities;

/// <summary>
/// 競技プログラミングなどで利用可能な共通ユーティリティ関数を提供します。
/// </summary>
public static class Common
{
    /// <summary>大文字アルファベットの文字配列。</summary>
    public static readonly char[] UpperAlphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    /// <summary>小文字アルファベットの文字配列。</summary>
    public static readonly char[] LowerAlphabets = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

    /// <summary>
    /// 指定された値が指定された範囲内にあるかどうかを確認します。
    /// </summary>
    /// <typeparam name="T">比較する数値型。</typeparam>
    /// <param name="value">確認する値。</param>
    /// <param name="min">範囲の最小値 (含む)。</param>
    /// <param name="max">範囲の最大値 (含む)。</param>
    /// <returns>値が範囲内にある場合は true、それ以外の場合は false。</returns>
    public static bool IsInRange<T>(T value, T min, T max) where T : IComparable<T> => min.CompareTo(value) <= 0 && value.CompareTo(max) <= 0;

    #region Cumulative Sum

    /// <summary>
    /// 配列の累積和を生成します。
    /// </summary>
    /// <typeparam name="T">累積和を計算する数値型 (INumber&lt;T&gt;)。</typeparam>
    /// <param name="array">元の配列。</param>
    /// <returns>累積和を格納した新しい配列。</returns>
    /// <exception cref="ArgumentNullException">arrayがnullの場合。</exception>
    public static T[] GenerateCumulativeSum<T>(T[] array) where T : INumber<T>
    {
        ArgumentNullException.ThrowIfNull(array);

        var cumulativeSum = new T[array.Length];
        if (array.Length == 0) return cumulativeSum;

        cumulativeSum[0] = array[0];
        for (int i = 1; i < array.Length; i++)
        {
            cumulativeSum[i] = cumulativeSum[i - 1] + array[i];
        }
        return cumulativeSum;
    }

    /// <summary>
    /// long配列の累積和を生成します。
    /// </summary>
    /// <param name="array">元の配列。</param>
    /// <returns>累積和を格納した新しい配列。</returns>
    /// <exception cref="ArgumentNullException">arrayがnullの場合。</exception>
    public static long[] GenerateCumulativeSum(long[] array)
    {
        ArgumentNullException.ThrowIfNull(array);
        var result = new long[array.Length];
        if (array.Length == 0) return result;
        result[0] = array[0];
        for (int i = 1; i < array.Length; i++)
        {
            result[i] = result[i - 1] + array[i];
        }
        return result;
    }
    #endregion

    #region Combinations and Permutations (Yield Return for Memory Efficiency)

    /// <summary>
    /// 指定された範囲の数値から指定された個数を選択する組み合わせを列挙します (遅延評価)。
    /// </summary>
    /// <param name="n">選択対象の最大値 (1 から n)。</param>
    /// <param name="k">選択する個数。</param>
    /// <returns>組み合わせのシーケンス。各組み合わせは int の IEnumerable。</returns>
    /// <exception cref="ArgumentOutOfRangeException">n, k が負、または k > n。</exception>
    public static IEnumerable<IEnumerable<int>> GenerateCombinations(int n, int k)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "n must be non-negative.");
        if (k < 0) throw new ArgumentOutOfRangeException(nameof(k), "k must be non-negative.");
        if (k > n) throw new ArgumentOutOfRangeException(nameof(k), "k cannot be greater than n.");

        if (k == 0)
        {
            yield return Enumerable.Empty<int>();
            // yield break; // k=0 の場合は yield return の後に暗黙的に終了するので省略可
        }
        else // k > 0 の場合
        {
            // CS1622 修正: GenerateCombinationsRecursive の結果を foreach で yield return する
            foreach (var combination in GenerateCombinationsRecursive(1, n, k, 0, new int[k]))
            {
                yield return combination;
            }
        }
    }

    private static IEnumerable<IEnumerable<int>> GenerateCombinationsRecursive(int start, int n, int k, int index, int[] currentCombination)
    {
        if (index == k)
        {
            yield return (int[])currentCombination.Clone();
        }
        else
        {
            for (int i = start; i <= n - (k - index - 1); i++)
            {
                currentCombination[index] = i;
                foreach (var combination in GenerateCombinationsRecursive(i + 1, n, k, index + 1, currentCombination))
                    yield return combination;
            }
        }
    }

    /// <summary>
    /// 指定された範囲の数値から指定された個数を選択する順列を列挙します (遅延評価)。
    /// </summary>
    /// <param name="n">選択対象の最大値 (1 から n)。</param>
    /// <param name="k">選択する個数。</param>
    /// <returns>順列のシーケンス。各順列は int の IEnumerable。</returns>
    /// <exception cref="ArgumentOutOfRangeException">n, k が負、または k > n。</exception>
    public static IEnumerable<IEnumerable<int>> GeneratePermutations(int n, int k)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "n must be non-negative.");
        if (k < 0) throw new ArgumentOutOfRangeException(nameof(k), "k must be non-negative.");
        if (k > n) throw new ArgumentOutOfRangeException(nameof(k), "k cannot be greater than n.");

        if (k == 0)
        {
            yield return Enumerable.Empty<int>();
            // yield break; // k=0 の場合は yield return の後に暗黙的に終了するので省略可
        }
        else // k > 0 の場合
        {
            // CS1622 修正: GeneratePermutationsRecursive の結果を foreach で yield return する
            foreach (var permutation in GeneratePermutationsRecursive(n, k, 0, new int[k], new bool[n + 1]))
            {
                yield return permutation;
            }
        }
    }

    private static IEnumerable<IEnumerable<int>> GeneratePermutationsRecursive(int n, int k, int index, int[] currentPermutation, bool[] used)
    {
        if (index == k)
        {
            yield return (int[])currentPermutation.Clone();
        }
        else
        {
            for (int i = 1; i <= n; i++)
            {
                if (!used[i])
                {
                    used[i] = true;
                    currentPermutation[index] = i;
                    foreach (var permutation in GeneratePermutationsRecursive(n, k, index + 1, currentPermutation, used))
                        yield return permutation;
                    used[i] = false; // Backtrack
                }
            }
        }
    }

    #endregion

    #region Console IO Optimization

    private static StreamWriter? bufferedWriter = null;

    /// <summary>
    /// コンソール出力をバッファリングし、自動フラッシュを無効にして高速化します。
    /// 使用後は FlushConsoleBuffer() を呼び出すことを推奨します。
    /// </summary>
    /// <param name="bufferSize">内部バッファのサイズ (バイト単位)。既定値 65536。</param>
    public static void EnableConsoleBuffering(int bufferSize = 65536)
    {
        if (bufferedWriter == null) // Simple check, might need more robust logic
        {
            // bufferedWriter が null でないことがここで保証される
            bufferedWriter = new StreamWriter(Console.OpenStandardOutput(), Console.OutputEncoding, bufferSize) { AutoFlush = false };
            Console.SetOut(bufferedWriter);
        }
    }

    /// <summary>
    /// バッファリングされたコンソール出力をフラッシュします。
    /// EnableConsoleBuffering() を使用した場合、プログラム終了前に呼び出すことが推奨されます。
    /// </summary>
    public static void FlushConsoleBuffer()
    {
        Console.Out.Flush();
    }
    #endregion
}