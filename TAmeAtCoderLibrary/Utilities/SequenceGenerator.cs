namespace TAmeAtCoderLibrary.Utilities;

/// <summary>
/// 組み合わせや順列などのシーケンスを生成するユーティリティクラス。
/// </summary>
public static class SequenceGenerator
{
    /// <summary>
    /// 指定された範囲の整数から指定された個数の要素を選ぶ組み合わせを生成します。
    /// </summary>
    /// <param name="n">選択対象となる整数の最大値 (1からnまで)。</param>
    /// <param name="k">組み合わせを構成する要素の数。</param>
    /// <returns>
    /// 生成された組み合わせ (各組み合わせは整数のリスト) を順次返す反復子。
    /// 各組み合わせのリストは新しいインスタンスです。
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// kが0未満の場合、またはkがnより大きい場合にスローされます。
    /// </exception>
    public static IEnumerable<IList<int>> GenerateCombinations(int n, int k)
    {
        // 引数チェック
        if (k < 0 || k > n)
        {
            throw new ArgumentOutOfRangeException(nameof(k), $"k must be between 0 and n (inclusive). k={k}, n={n}");
        }
        if (k == 0)
        {
            yield return new List<int>(); // 空の組み合わせ
            yield break;
        }

        // 再帰ヘルパーメソッドが生成する各組み合わせをyield returnする
        var initialCombination = new List<int>(k); // 初期リストを作成
        foreach (var combination in GenerateCombinationsRecursive(1, n, k, initialCombination))
        {
            yield return combination; // ヘルパーからの結果を順次返す
        }
    }

    /// <summary>
    /// 組み合わせを再帰的に生成するヘルパーメソッド。
    /// </summary>
    /// <param name="start">現在の組み合わせに追加できる最小の数値。</param>
    /// <param name="n">選択対象となる整数の最大値。</param>
    /// <param name="k">組み合わせを構成する要素の数。</param>
    /// <param name="currentCombination">現在構築中の組み合わせリスト。</param>
    /// <returns>生成された組み合わせを順次返す反復子。</returns>
    private static IEnumerable<IList<int>> GenerateCombinationsRecursive(int start, int n, int k, List<int> currentCombination)
    {
        // 現在の組み合わせが目的の要素数に達したら、そのコピーを返す
        if (currentCombination.Count == k)
        {
            yield return new List<int>(currentCombination); // 変更を防ぐためコピーを返す
            yield break;
        }

        // startからnまでの各数を試す
        // 残り必要な要素数よりも多くの要素が残っている場合のみループ
        for (int i = start; i <= n && k - currentCombination.Count <= n - i + 1; i++)
        {
            currentCombination.Add(i); // 現在の数を組み合わせに追加

            // 次の要素を探すために再帰呼び出し
            // yield return foreach のような構文はないため、内部でforeachする
            foreach (var combination in GenerateCombinationsRecursive(i + 1, n, k, currentCombination))
            {
                yield return combination;
            }

            currentCombination.RemoveAt(currentCombination.Count - 1); // バックトラック: 追加した数を削除
        }
    }


    /// <summary>
    /// 指定された要素のシーケンスから全ての順列を生成します (辞書順)。
    /// </summary>
    /// <typeparam name="T">要素の型。比較可能である必要があります。</typeparam>
    /// <param name="items">順列を生成する元の要素のシーケンス。</param>
    /// <returns>
    /// 生成された順列 (各順列は要素の配列) を順次返す反反復子。
    /// 各順列の配列は新しいインスタンスです。
    /// </returns>
    /// <exception cref="ArgumentNullException">itemsがnullの場合にスローされます。</exception>
    public static IEnumerable<T[]> GeneratePermutations<T>(IEnumerable<T> items) where T : IComparable<T>
    {
        if (items == null)
        {
            throw new ArgumentNullException(nameof(items));
        }

        T[] a = items.ToArray();
        int n = a.Length;

        if (n == 0)
        {
            yield break; // 要素がなければ順列もなし
        }

        // 最初の順列を返す (コピー)
        yield return a.ToArray();

        while (true)
        {
            // 1. a[i] < a[i+1] となる最大の i を見つける
            int i = n - 2;
            while (i >= 0 && a[i].CompareTo(a[i + 1]) >= 0)
            {
                i--;
            }

            // 2. そのような i が見つからなければ、最後の順列なので終了
            if (i < 0)
            {
                yield break;
            }

            // 3. a[i] < a[j] となる最大の j (j > i) を見つける
            int j = n - 1;
            while (a[i].CompareTo(a[j]) >= 0)
            {
                j--;
            }

            // 4. a[i] と a[j] を交換する
            T tmp = a[i];
            a[i] = a[j];
            a[j] = tmp;

            // 5. a[i+1] 以降の要素を逆順にする
            Array.Reverse(a, i + 1, n - i - 1);

            // 見つかった次の順列を返す (コピー)
            yield return a.ToArray();
        }
    }

    /// <summary>
    /// 1から指定された最大値までの整数の全ての順列を生成します (辞書順)。
    /// </summary>
    /// <param name="max">順列を構成する整数の最大値 (1からmaxまで)。</param>
    /// <returns>
    /// 生成された順列 (各順列は整数の配列) を順次返す反復子。
    /// 各順列の配列は新しいインスタンスです。
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">maxが0以下の場合にスローされます。</exception>
    public static IEnumerable<int[]> GeneratePermutations(int max)
    {
        if (max <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(max), "max must be a positive integer.");
        }
        // ジェネリック版を呼び出す必要があるが、これもイテレータなのでforeachでyieldする
        foreach (var permutation in GeneratePermutations(Enumerable.Range(1, max)))
        {
            yield return permutation;
        }
    }
}