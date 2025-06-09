# TAmeAtCoderLibrary

**TAmeAtCoderLibrary** は、競技プログラミングサイト「[AtCoder](https://atcoder.jp/)」などのコーディングコンテストにC#で挑むプログラマーのために設計された、高機能かつパフォーマンス重視のライブラリです。

複雑なアルゴリズムや頻出のデータ構造がすぐに使える形で実装されており、問題解決に集中できる環境を提供します。初心者から上級者まで、幅広いレベルのプログラマーにご活用いただけます。

## ✨ 主な機能

このライブラリは、競技プログラミングで要求される様々な機能を網羅しています。

| カテゴリ | 主な機能 | ソースファイル |
| :--- | :--- | :--- |
| **データ構造** | `UnionFindTree`, `SegmentTree`, `BinaryIndexedTree` (BIT), `AVLTree` (自己平衡二分探索木), `OrderedKeyMap`, `DoublyLinkedList` | `UnionFindTree.cs`, `SegmentTree.cs`, `BinaryIndexedTree.cs`, `AVLTree.cs`, `OrderedKeyMap.cs`, `DoublyLinkedList.cs` |
| **グラフアルゴリズム** | 有向/無向グラフ、Dijkstra法、トポロジカルソート、閉路検出、隣接リスト表現 | `SimpleDirectedGraph.cs`, `SimpleUndirectedGraph.cs` |
| **数学的アルゴリズム**| 素数判定・列挙、素因数分解、最大公約数(GCD)・最小公倍数(LCM)、階乗・順列・組み合わせ (剰余計算対応)、等差数列の和 | `MathEx/Prime.cs`, `MathEx/DivAndMulti.cs`, `MathEx/Pattern.cs`, `MathEx/ArithmeticSequence.cs` |
| **ユーティリティ** | 高速入出力 (`ReadLine`)、行列操作 (生成、転置、クローン)、2分探索ユーティリティ (`LowerBound`, `UpperBound`)、基数変換、累積和生成 | `ReadLine.cs`, `Matrix.cs`, `BinarySearchUtils.cs`, `Utilities/Numeric.cs`, `Utilities/Common.cs` |

## 🚀 クイックスタート

### 前提条件

* .NET 8.0 以上

### 導入方法

1. **リポジトリをクローンまたはダウンロードします。**

    ```bash
    git clone https://github.com/tarminjapan/TAmeAtCoderLibrary.git
    ```

2. **プロジェクトにライブラリのソースコードを追加します。**
    `TAmeAtCoderLibrary` ディレクトリ内の `.cs` ファイルを、あなたのAtCoder用プロジェクトにコピーしてください。
3. **SourceExpander を利用します。**
    このライブラリは `SourceExpander.Expander.Expand()` を呼び出すことで、必要なソースコードを単一ファイルにまとめ、AtCoderへの提出を容易にします。

### 基本的な使い方

`AtCoderCSharp/Program.cs` をテンプレートとして利用できます。 高速入出力と問題解決ロジックの分離が簡単に行えます。

```csharp
// AtCoderCSharp/Program.cs の例

// 必要なライブラリを using
using TAmeAtCoderLibrary;
using TAmeAtCoderLibrary.Utilities;

internal class Program
{
    static void Main()
    {
        // SourceExpander を使用してコードを展開
        SourceExpander.Expander.Expand();
        // コンソールIOの高速化
        Common.EnableConsoleBuffering();
        
        // 問題を解くためのクラスをインスタンス化
        var solve = new Solve();
        solve.Run();

        // バッファをフラッシュして出力を確定
        Common.FlushConsoleBuffer();
    }

    public class Solve
    {
        public void Run()
        {
            // --- ここに問題解決のコードを記述 ---

            // 例: ReadLineクラスを使った入力
            var inputs = ReadLine.Ints();
            int n = inputs[0];
            int m = inputs[1];

            var edges = ReadLine.IntMatrix(m);

            // UnionFindTreeの使用例
            var uf = new UnionFindTree<int>();
            for (int i = 1; i <= n; i++)
            {
                uf.Add(i);
            }

            foreach(var edge in edges)
            {
                uf.Union(edge[0], edge[1]);
            }

            // 結果の出力
            Console.WriteLine(uf.CountSets());
        }
    }
}
```

### プロジェクトの実行

`dotnet` CLI を使ってプロジェクトをビルド・実行できます。

```bash
dotnet run --project ./AtCoderCSharp/AtCoderCSharp.csproj
```

VS Code をお使いの場合は、デバッグ設定が `.vscode/launch.json` に、ビルドタスクが `.vscode/tasks.json` に含まれています。

## 📖 提供機能一覧

<details>
<summary>詳細なモジュールリストはこちら</summary>

* **`TAmeAtCoderLibrary`**
  * `AVLTree<T>`: 自己平衡二分探索木。
  * `BinaryIndexedTree`: 区間和と一点更新を高速に行うデータ構造。
  * `BinarySearchUtils<T>`: ソート済みリストに対する `LowerBound`, `UpperBound` などの二分探索を提供。
  * `DoublyLinkedList<T>`: 双方向連結リスト。
  * `Fraction`: 分数クラス。四則演算や比較が可能。
  * `Matrix<T>`: 行列の生成、クローン、転置などの操作。
  * `OrderedKeyMap<TKey, TValue>`: キーの順序を保持するマップ。AVL木と辞書で実装。
  * `ReadLine`: `Ints()`, `Longs()`, `String()` など、競技プログラミング向けの高速な標準入力。
  * `SegmentTree`: 区間に対するクエリ（合計、最大、最小）を効率的に処理。
  * `SimpleDirectedGraph`: 有向グラフ。Dijkstra法、トポロジカルソート、閉路検出など。
  * `SimpleUndirectedGraph`: 無向グラフ。`SimpleDirectedGraph` を継承。
  * `UnionFindTree<T>`: 互いに素な集合を管理するデータ構造。
* **`TAmeAtCoderLibrary.MathEx`**
  * `ArithmeticSequence`: 等差数列の和（剰余計算対応）。
  * `Basic`: べき乗、整数平方根、最大公約数、最小公倍数、モジュラ逆数など。
  * `Coordinate`: 3点の位置関係を判定する `CounterClockWise`。
  * `DivAndMulti`: 約数列挙、GCD、LCM。
  * `Pattern`: 階乗、順列、組み合わせ（剰余計算対応）。
  * `Prime`: エラトステネスの篩による素数列挙、素数判定、素因数分解。
* **`TAmeAtCoderLibrary.Utilities`**
  * `Common`: 累積和生成、組み合わせ・順列列挙、コンソールIO高速化。
  * `Numeric`: 基数変換、数値の桁操作。
  * `SequenceGenerator`: 組み合わせや順列を効率的に生成。

</details>

## 📜 ライセンス

このライブラリは **MITライセンス** の下で提供されています。

これは非常に寛容なライセンスであり、要約すると以下の通りです：

* **商用利用可能**: このソフトウェアを商用目的で使用できます。
* **改変可能**: ソフトウェアを自由に変更できます。
* **配布可能**: オリジナルまたは変更したソフトウェアを再配布できます。
* **私的利用可能**: 個人的な目的で自由に使用できます。

唯一の主な条件は、ソフトウェアのコピーまたは重要な部分に、元の著作権表示とこのライセンスの全文を含めることです。 また、本ソフトウェアは「現状のまま」提供され、商品性や特定目的への適合性の保証を含め、いかなる保証もありません。 詳細については、`LICENSE.txt` ファイルをご参照ください。
