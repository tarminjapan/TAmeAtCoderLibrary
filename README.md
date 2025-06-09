<div align="center">
<img src="[https://capsule-render.vercel.app/api?type=rounded&color=gradient&customColorList=2,5,10,15&height=200&section=header&text=TAmeAtCoderLibrary&fontSize=50&animation=twinkling&fontAlignY=35](https://capsule-render.vercel.app/api?type=rounded&color=gradient&customColorList=2,5,10,15&height=200&section=header&text=TAmeAtCoderLibrary&fontSize=50&animation=twinkling&fontAlignY=35)" alt="Header Banner" />
</div>

<div align="center">
<img src="[https://readme-typing-svg.herokuapp.com?font=Fira+Code&size=24&duration=3000&pause=1000&color=00BFFF&center=true&vCenter=true&width=600&lines=A+Comprehensive+C%23+Library;For+Competitive+Programming+%F0%9F%9A%80;Data+Structures+%26+Algorithms;Optimized+for+AtCoder+%E2%9C%A8](https://readme-typing-svg.herokuapp.com?font=Fira+Code&size=24&duration=3000&pause=1000&color=00BFFF&center=true&vCenter=true&width=600&lines=A+Comprehensive+C%23+Library;For+Competitive+Programming+%F0%9F%9A%80;Data+Structures+%26+Algorithms;Optimized+for+AtCoder+%E2%9C%A8)" alt="Typing SVG" />
</div>

<div align="center">
<a href="./LICENSE.txt">
<img src="[https://img.shields.io/badge/License-MIT-yellow.svg](https://img.shields.io/badge/License-MIT-yellow.svg)" alt="License: MIT">
</a>
<img src="[https://img.shields.io/badge/C%23-8.0%2B-blueviolet](https://img.shields.io/badge/C%23-8.0%2B-blueviolet)" alt="CSharp Version">
<img src="[https://img.shields.io/badge/.NET-8.0-blue](https://img.shields.io/badge/.NET-8.0-blue)" alt="Target Framework">
</div>

<br>

-----

### <p align="center"> **`TAmeAtCoderLibrary` は、競技プログラミングのためのオールインワンC#ライブラリです。** </p>

<p align="center"> 頻出のデータ構造から高度なアルゴリズムまで、あなたのコーディングを加速させます。</p>

-----

## 🌟 主な機能

<table>
<tr>
<td align="center" width="25%">
<img src="[https://skillicons.dev/icons?i=cs](https://skillicons.dev/icons?i=cs)" width="60" alt="C#" /><br>
<strong>モダンなC#</strong><br>
<small>C# 8.0以上、.NET 8対応。最新の言語機能で記述されています。</small>
</td>
<td align="center" width="25%">
<img src="[https://user-images.githubusercontent.com/28275993/151894921-82e74d87-25c1-4253-8339-c189c44532ea.png](https://user-images.githubusercontent.com/28275993/151894921-82e74d87-25c1-4253-8339-c189c44532ea.png)" width="60" alt="Algorithm Icon" /><br>
<strong>豊富なアルゴリズム</strong><br>
<small>グラフ、数学、探索など、競技プログラミングに必要なアルゴリズムを網羅。</small>
</td>
<td align="center" width="25%">
<img src="[https://user-images.githubusercontent.com/28275993/151895295-d86b883c-99b3-43e5-8902-e2a22b40342a.png](https://user-images.githubusercontent.com/28275993/151895295-d86b883c-99b3-43e5-8902-e2a22b40342a.png)" width="60" alt="Data Structure Icon" /><br>
<strong>必須のデータ構造</strong><br>
<small>Union-Find, セグメント木, AVL木など、すぐに使える強力なデータ構造群。</small>
</td>
<td align="center" width="25%">
<img src="[https://user-images.githubusercontent.com/28275993/151894982-3783a669-122e-436f-8367-e9b407a9ebb2.png](https://user-images.githubusercontent.com/28275993/151894982-3783a669-122e-436f-8367-e9b407a9ebb2.png)" width="60" alt="Utility Icon" /><br>
<strong>便利なユーティリティ</strong><br>
<small>高速I/O、行列演算、組み合わせ生成など、コーディングを効率化するツールが満載。</small>
</td>
</tr>
</table>

## 📚 ライブラリ詳細 (Modules Overview)

<details>
<summary><strong>📁 データ構造 (Data Structures)</strong></summary>
<br>
<table>
<thead>
<tr>
<th>クラス</th>
<th>説明</th>
</tr>
</thead>
<tbody>
<tr>
<td><code>AvlTree&lt;T&gt;</code></td>
<td>自己平衡二分探索木。挿入、削除、検索が <em>O(log n)</em> で行えます。</td>
</tr>
<tr>
<td><code>BinaryIndexedTree</code></td>
<td>区間和の取得と一点更新を <em>O(log n)</em> で行うFenwick Tree。</td>
</tr>
<tr>
<td><code>DoublyLinkedList&lt;T&gt;</code></td>
<td>双方向連結リスト。要素の挿入、削除が <em>O(1)</em> で行えます。</td>
</tr>
<tr>
<td><code>OrderedKeyMap&lt;TKey, TValue&gt;</code></td>
<td>AVL木をベースにした順序付きマップ。キーの順序を維持しつつ高速なアクセスを提供します。</td>
</tr>
<tr>
<td><code>SegmentTree</code></td>
<td>セグメント木。区間に対する合計、最大値、最小値のクエリを <em>O(log n)</em> で処理します。</td>
</tr>
<tr>
<td><code>UnionFindTree&lt;T&gt;</code></td>
<td>互いに素な集合を管理するデータ構造。経路圧縮とサイズによる統合で最適化済み。</td>
</tr>
</tbody>
</table>
</details>

<details>
<summary><strong>🌍 グラフアルゴリズム (Graph Algorithms)</strong></summary>
<br>
<table>
<thead>
<tr>
<th>クラス</th>
<th>説明</th>
</tr>
</thead>
<tbody>
<tr>
<td><code>SimpleDirectedGraph</code></td>
<td><strong>有向グラフ</strong>の実装。以下のアルゴリズムを内蔵しています：<br>
<ul>
<li><b>ダイクストラ法</b>: 単一始点からの最短経路を計算します。</li>
<li><b>トポロジカルソート</b>: 閉路のない有向グラフの頂点を線形に順序付けします。</li>
<li><b>閉路検出</b>: グラフ内にサイクルが存在するかを検出します。</li>
</ul>
</td>
</tr>
<tr>
<td><code>SimpleUndirectedGraph</code></td>
<td><strong>無向グラフ</strong>の実装。<code>SimpleDirectedGraph</code>を継承し、無向グラフ特有の機能（次数計算、サイクル検出など）を提供します。</td>
</tr>
</tbody>
</table>
</details>

<details>
<summary><strong>🔢 数学ユーティリティ (Mathematical Utilities)</strong></summary>
<br>
<table>
<thead>
<tr>
<th>名前空間 / クラス</th>
<th>説明</th>
</tr>
</thead>
<tbody>
<tr>
<td><code>MathEx.Basic</code></td>
<td>べき乗、平方根、最大公約数(GCD)、最小公倍数(LCM)、べき乗剰余(ModPow)など。</td>
</tr>
<tr>
<td><code>MathEx.Prime</code></td>
<td>エラトステネスの篩、素数判定、素因数分解。</td>
</tr>
<tr>
<td><code>MathEx.Pattern</code></td>
<td>階乗、順列、組み合わせの計算（剰余計算対応）。</td>
</tr>
<tr>
<td><code>MathEx.ArithmeticSequence</code></td>
<td>等差数列の和を高速に計算します。</td>
</tr>
<tr>
<td><code>Fraction</code></td>
<td>分数を扱うクラス。四則演算や比較が可能です。</td>
</tr>
</tbody>
</table>
</details>

<details>
<summary><strong>🛠️ 汎用ユーティリティ (General Utilities)</strong></summary>
<br>
<table>
<thead>
<tr>
<th>クラス</th>
<th>説明</th>
</tr>
</thead>
<tbody>
<tr>
<td><code>ReadLine</code></td>
<td>コンソールからの入力を型に合わせて高速に読み込む静的クラス。</td>
</tr>
<tr>
<td><code>BinarySearchUtils&lt;T&gt;</code></td>
<td>ソート済みリストに対する <code>LowerBound</code>, <code>UpperBound</code> などの二分探索機能。</td>
</tr>
<tr>
<td><code>Matrix&lt;T&gt;</code></td>
<td>2次元配列（行列）の生成、クローン、転置などの操作をサポート。</td>
</tr>
<tr>
<td><code>Utilities.Common</code></td>
<td>累積和生成、組み合わせ・順列の列挙、高速コンソールI/Oなど。</td>
</tr>
</tbody>
</table>
</details>

## 🚀 使用例 (Usage Example)

`Program.cs` のテンプレートです。`Solve`クラス内にロジックを記述するだけですぐに問題を解き始められます。

```csharp:atcodercsharp/program.cs
using TAmeAtCoderLibrary;
using TAmeAtCoderLibrary.Utilities;

internal class Program
{
    static void Main()
    {
        // 高速I/Oを有効化
        Common.EnableConsoleBuffering();
        
        var solve = new Solve();
        solve.Run();

        // バッファをフラッシュして出力
        Common.FlushConsoleBuffer();
    }

    public class Solve
    {
        public void Run()
        {
            // --- ここに問題解決のコードを記述 ---

            // 例: ReadLineクラスで簡単に入力を受け取る
            var inputs = ReadLine.Ints();
            int n = inputs[0];
            int m = inputs[1];

            // 例: UnionFindTreeを使って問題を解く
            var uf = new UnionFindTree<int>();
            for (int i = 1; i <= n; i++)
            {
                uf.Add(i);
            }
            // ...
        }
    }
}
```

## 💭 ランダムな開発者の名言

<div align="center">
<img src="[https://quotes-github-readme.vercel.app/api?type=horizontal&theme=dark](https://quotes-github-readme.vercel.app/api?type=horizontal&theme=dark)" alt="Random Dev Quote"/>
</div>

<div align="center">
<img src="[https://capsule-render.vercel.app/api?type=rounded&color=gradient&customColorList=2,5,10,15&height=120&section=footer&animation=twinkling](https://capsule-render.vercel.app/api?type=rounded&color=gradient&customColorList=2,5,10,15&height=120&section=footer&animation=twinkling)" alt="Footer Banner"/>
</div>
