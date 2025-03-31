namespace TAmeAtCoderLibrary;

using System;
using System.Collections.Generic;

/// <summary>
/// 自己平衡二分探索木です。挿入、削除、検索の各操作が常にO(log n)の時間計算量で行えます。
/// </summary>
/// <typeparam name="T">木に格納する要素の型。IComparable<T> インターフェースを実装している必要があります。</typeparam>
public class AvlTree<T> where T : IComparable<T>
{
    /// <summary>
    /// ルートノードを取得または設定します。
    /// </summary>
    public Node RootNode { get; private set; }

    /// <summary>
    /// 木の最小キー値を取得します。木が空の場合は型のデフォルト値を返します。
    /// </summary>
    public T MinKey => RootNode != null ? RootNode.GetMin() : default;

    /// <summary>
    /// 木の最大キー値を取得します。木が空の場合は型のデフォルト値を返します。
    /// </summary>
    public T MaxKey => RootNode != null ? RootNode.GetMax() : default;

    /// <summary>
    /// 空の<see cref="AvlTree{T}"/>クラスの新しいインスタンスを初期化します。
    /// </summary>
    public AvlTree() { }

    /// <summary>
    /// 指定したコレクションに含まれる要素を使用して、<see cref="AvlTree{T}"/>クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="collection">初期化する要素を含むコレクション。</param>
    public AvlTree(IEnumerable<T> collection)
    {
        foreach (var item in collection)
        {
            Add(item);
        }
    }

    /// <summary>
    /// 木に新しい要素を追加します。同じ要素が既に存在する場合は、何も行いません。
    /// </summary>
    /// <param name="item">追加する要素。</param>
    public void Add(T item)
    {
        if (Contains(item))
        {
            return;
        }

        if (RootNode == null)
        {
            RootNode = new Node(item, this);
        }
        else
        {
            RootNode.Add(item);
        }
    }

    /// <summary>
    /// 木からすべての要素を削除します。
    /// </summary>
    public void Clear()
    {
        if (RootNode == null)
        {
            return;
        }

        RootNode.Clear();
        RootNode = null;
    }

    /// <summary>
    /// 指定した要素が木に含まれているかどうかを判断します。
    /// </summary>
    /// <param name="item">木内で検索する要素。</param>
    /// <returns>指定した要素が木に含まれている場合は<c>true</c>。それ以外の場合は<c>false</c>。</returns>
    public bool Contains(T item) => RootNode != null && RootNode.Contains(item);

    /// <summary>
    /// 指定した値よりも小さい最大の値を内部的に取得します。
    /// </summary>
    /// <param name="item">基準となる値。</param>
    /// <param name="currentMax">現在の最大値。</param>
    /// <returns>指定した値よりも小さい最大の値を返します。</returns>
    private T FindLessThan(T item, T currentMax) => RootNode != null ? RootNode.FindLessThan(item, currentMax) : currentMax;

    /// <summary>
    /// 指定した値よりも大きい最小の値を内部的に取得します。
    /// </summary>
    /// <param name="item">基準となる値。</param>
    /// <param name="currentMin">現在の最小値。</param>
    /// <returns>指定した値よりも大きい最小の値を返します。</returns>
    private T FindGreaterThan(T item, T currentMin) => RootNode != null ? RootNode.FindGreaterThan(item, currentMin) : currentMin;

    /// <summary>
    /// 指定したキー以下の最大のキーを返します。
    /// </summary>
    /// <param name="item">基準となるキー。</param>
    /// <param name="minKey">許容される最小のキー。</param>
    /// <returns>指定したキー以下の最大のキー。そのようなキーが存在しない場合は <paramref name="minKey"/> を返します。</returns>
    public T FindLessThanOrEqual(T item, T minKey) => Contains(item) ? item : FindLessThan(item, minKey);

    /// <summary>
    /// 指定したキー以上の最小のキーを返します。
    /// </summary>
    /// <param name="item">基準となるキー。</param>
    /// <param name="maxKey">許容される最大のキー。</param>
    /// <returns>指定したキー以上の最小のキー。そのようなキーが存在しない場合は <paramref name="maxKey"/> を返します。</returns>
    public T FindGreaterThanOrEqual(T item, T maxKey) => Contains(item) ? item : FindGreaterThan(item, maxKey);

    /// <summary>
    /// 木に格納されている要素の数を取得します。
    /// </summary>
    public int Count => RootNode?.Count ?? 0;

    /// <summary>
    /// 木から指定した要素を削除します。要素が見つからない場合は、何も行いません。
    /// </summary>
    /// <param name="item">削除する要素。</param>
    /// <returns>要素が削除された場合は<c>true</c>。それ以外の場合は<c>false</c>。</returns>
    public bool Remove(T item)
    {
        if (RootNode == null)
        {
            return false;
        }

        return RootNode.Remove(item);
    }

    /// <summary>
    /// 木の要素を昇順で含むリストを返します。
    /// </summary>
    /// <returns>木の要素を昇順で並べた<see cref="List{T}"/>。</returns>
    public List<T> ToSortedList()
    {
        var result = new List<T>();

        if (RootNode != null)
        {
            RootNode.InOrderTraversal(result);
        }

        return result;
    }

    /// <summary>
    /// 指定したインデックスにある要素を取得します。
    /// </summary>
    /// <param name="index">取得する要素のインデックス（0から始まる）。</param>
    /// <returns>指定したインデックスにある要素。</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> が範囲外の場合。</exception>
    /// <exception cref="InvalidOperationException">インデックスへの設定操作はサポートされていません。</exception>
    public T this[int index]
    {
        get
        {
            if (RootNode != null)
            {
                return RootNode[index];
            }

            throw new ArgumentOutOfRangeException(nameof(index), "指定されたインデックスは範囲外です。");
        }
        set
        {
            throw new InvalidOperationException("インデックスによる設定操作はサポートされていません。");
        }
    }

    /// <summary>
    /// <see cref="AvlTree{T}"/>クラスのノードを表します。
    /// </summary>
    public class Node
    {
        /// <summary>
        /// <see cref="Node"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="item">ノードに格納する値。</param>
        /// <param name="tree">このノードが属する<see cref="AvlTree{T}"/>インスタンス。</param>
        public Node(T item, AvlTree<T> tree)
        {
            Value = item;
            Height = 1;
            Count = 1;
            Tree = tree;
        }

        /// <summary>
        /// このノードが属する<see cref="AvlTree{T}"/>インスタンスを取得します。
        /// </summary>
        public AvlTree<T> Tree { get; private set; }

        /// <summary>
        /// ノードの値を読み取り専用で取得します。
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// 親ノードを取得または設定します。
        /// </summary>
        public Node Parent { get; private set; }

        /// <summary>
        /// 左の子ノードを取得または設定します。
        /// </summary>
        public Node Left { get; private set; }

        /// <summary>
        /// 右の子ノードを取得または設定します。
        /// </summary>
        public Node Right { get; private set; }

        /// <summary>
        /// このノードの高さを取得または設定します。
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// このノードをルートとする部分木のノード数を取得します。
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// このノードまたはその子孫に新しい要素を追加します。同じ要素が既に存在する場合は、何も行いません。
        /// </summary>
        /// <param name="item">追加する要素。</param>
        public void Add(T item)
        {
            var compareResult = item.CompareTo(Value);
            if (compareResult < 0)
            {
                if (Left == null)
                {
                    Left = new Node(item, Tree) { Parent = this };
                    Rebalance(true);
                }
                else
                {
                    Left.Add(item);
                }
            }
            else
            {
                if (Right == null)
                {
                    Right = new Node(item, Tree) { Parent = this };
                    Rebalance(true);
                }
                else
                {
                    Right.Add(item);
                }
            }
        }

        /// <summary>
        /// このノードをルートとする部分木からすべてのノードを削除します。
        /// </summary>
        public void Clear()
        {
            Left?.Clear();
            Right?.Clear();
            Left = Right = null;
        }

        /// <summary>
        /// 指定した要素がこのノードまたはその子孫に含まれているかどうかを判断します。
        /// </summary>
        /// <param name="item">検索する要素。</param>
        /// <returns>指定した要素がこのノードまたはその子孫に含まれている場合は<c>true</c>。それ以外の場合は<c>false</c>。</returns>
        public bool Contains(T item)
        {
            var compareResult = item.CompareTo(Value);
            if (compareResult < 0)
            {
                return Left != null && Left.Contains(item);
            }
            else if (compareResult == 0)
            {
                return true;
            }
            else
            {
                return Right != null && Right.Contains(item);
            }
        }

        /// <summary>
        /// このノードをルートとする部分木の最小の値を取得します。
        /// </summary>
        /// <returns>部分木の最小の値。</returns>
        public T GetMin() => Left != null ? Left.GetMin() : Value;

        /// <summary>
        /// このノードをルートとする部分木の最大の値を取得します。
        /// </summary>
        /// <returns>部分木の最大の値。</returns>
        public T GetMax() => Right != null ? Right.GetMax() : Value;

        /// <summary>
        /// 指定した値よりも小さい最大の値をこの部分木内で検索します。
        /// </summary>
        /// <param name="item">基準となる値。</param>
        /// <param name="currentMax">現在見つかっている最大のキー。</param>
        /// <returns>指定した値よりも小さい最大のキー。存在しない場合は <paramref name="currentMax"/> を返します。</returns>
        public T FindLessThan(T item, T currentMax)
        {
            var compareResult = item.CompareTo(Value);

            if (compareResult > 0 && Value.CompareTo(currentMax) >= 0)
            {
                currentMax = Value;
            }

            if (compareResult <= 0)
            {
                if (Left != null)
                {
                    return Left.FindLessThan(item, currentMax);
                }
                return currentMax;
            }
            else
            {
                if (Right != null)
                {
                    return Right.FindLessThan(item, currentMax);
                }
                return currentMax;
            }
        }

        /// <summary>
        /// 指定した値よりも大きい最小の値をこの部分木内で検索します。
        /// </summary>
        /// <param name="item">基準となる値。</param>
        /// <param name="currentMin">現在見つかっている最小のキー。</param>
        /// <returns>指定した値よりも大きい最小のキー。存在しない場合は <paramref name="currentMin"/> を返します。</returns>
        public T FindGreaterThan(T item, T currentMin)
        {
            var compareResult = item.CompareTo(Value);

            if (compareResult < 0 && Value.CompareTo(currentMin) <= 0)
            {
                currentMin = Value;
            }

            if (compareResult >= 0)
            {
                if (Right != null)
                {
                    return Right.FindGreaterThan(item, currentMin);
                }
                return currentMin;
            }
            else
            {
                if (Left != null)
                {
                    return Left.FindGreaterThan(item, currentMin);
                }
                return currentMin;
            }
        }

        /// <summary>
        /// このノードまたはその子孫から指定した要素を削除します。要素が見つかった場合は<c>true</c>を返します。
        /// </summary>
        /// <param name="item">削除する要素。</param>
        /// <returns>要素が削除された場合は<c>true</c>。それ以外の場合は<c>false</c>.</returns>
        public bool Remove(T item)
        {
            var compareResult = item.CompareTo(Value);
            if (compareResult == 0)
            {
                if (Left == null && Right == null)
                {
                    if (Parent != null)
                    {
                        if (Parent.Left == this)
                        {
                            Parent.Left = null;
                        }
                        else
                        {
                            Parent.Right = null;
                        }
                        Parent.Rebalance(true);
                    }
                    else
                    {
                        Tree.RootNode = null;
                    }
                }
                else if (Left == null || Right == null)
                {
                    var child = Left ?? Right;
                    if (Parent != null)
                    {
                        if (Parent.Left == this)
                        {
                            Parent.Left = child;
                        }
                        else
                        {
                            Parent.Right = child;
                        }
                        child.Parent = Parent;
                        Parent.Rebalance(true);
                    }
                    else
                    {
                        Tree.RootNode = child;
                        child.Parent = null;
                    }
                }
                else
                {
                    var replacement = Left;
                    while (replacement.Right != null)
                    {
                        replacement = replacement.Right;
                    }
                    Value = replacement.Value;
                    return replacement.Remove(replacement.Value);
                }
                Parent = Left = Right = null;
                return true;
            }
            else if (compareResult < 0)
            {
                return Left?.Remove(item) ?? false;
            }
            else
            {
                return Right?.Remove(item) ?? false;
            }
        }

        /// <summary>
        /// AVL木のバランスを再構築します。
        /// </summary>
        /// <param name="recursive">親ノードに対しても再帰的に再構築を行うかどうか。</param>
        void Rebalance(bool recursive)
        {
            Count = 1;

            int leftHeight = Left?.Height ?? 0;
            int rightHeight = Right?.Height ?? 0;

            if (Left != null)
            {
                Count += Left.Count;
            }
            if (Right != null)
            {
                Count += Right.Count;
            }

            if (leftHeight - rightHeight > 1)
            {
                var leftLeftHeight = Left.Left?.Height ?? 0;
                var leftRightHeight = Left.Right?.Height ?? 0;
                if (leftLeftHeight >= leftRightHeight)
                {
                    Left.RotateRight();
                    Rebalance(true);
                }
                else
                {
                    var pivot = Left.Right;
                    pivot.RotateLeft();
                    pivot.RotateRight();
                    pivot.Left.Rebalance(false);
                    pivot.Right.Rebalance(true);
                }
            }
            else if (rightHeight - leftHeight > 1)
            {
                var rightRightHeight = Right.Right?.Height ?? 0;
                var rightLeftHeight = Right.Left?.Height ?? 0;
                if (rightRightHeight >= rightLeftHeight)
                {
                    Right.RotateLeft();
                    Rebalance(true);
                }
                else
                {
                    var pivot = Right.Left;
                    pivot.RotateRight();
                    pivot.RotateLeft();
                    pivot.Left.Rebalance(false);
                    pivot.Right.Rebalance(true);
                }
            }
            else
            {
                Height = Math.Max(leftHeight, rightHeight) + 1;
                if (Parent != null && recursive)
                {
                    Parent.Rebalance(true);
                }
            }
        }

        /// <summary>
        /// ノードを右回転させます。
        /// </summary>
        void RotateRight()
        {
            var root = Parent;
            var parent = root?.Parent;
            if ((Parent = parent) == null)
            {
                Tree.RootNode = this;
            }
            else
            {
                if (parent.Left == root)
                {
                    parent.Left = this;
                }
                else
                {
                    parent.Right = this;
                }
            }

            if (root.Left == this)
            {
                root.Left = Right;
                if (Right != null)
                {
                    Right.Parent = root;
                }
                Right = root;
                root.Parent = this;
            }
            else
            {
                root.Right = Left;
                if (Left != null)
                {
                    Left.Parent = root;
                }
                Left = root;
                root.Parent = this;
            }
        }

        /// <summary>
        /// ノードを左回転させます。
        /// </summary>
        void RotateLeft()
        {
            var root = Parent;
            var parent = root?.Parent;
            if ((Parent = parent) == null)
            {
                Tree.RootNode = this;
            }
            else
            {
                if (parent.Left == root)
                {
                    parent.Left = this;
                }
                else
                {
                    parent.Right = this;
                }
            }

            if (root.Right == this)
            {
                root.Right = Left;
                if (Left != null)
                {
                    Left.Parent = root;
                }
                Left = root;
                root.Parent = this;
            }
            else
            {
                root.Left = Right;
                if (Right != null)
                {
                    Right.Parent = root;
                }
                Right = root;
                root.Parent = this;
            }
        }

        /// <summary>
        /// 指定したインデックスにある要素を取得します。
        /// </summary>
        /// <param name="index">取得する要素のインデックス（0から始まる）。</param>
        /// <returns>指定したインデックスにある要素。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> が範囲外の場合。</exception>
        /// <exception cref="InvalidOperationException">インデックスへの設定操作はサポートされていません。</exception>
        public T this[int index]
        {
            get
            {
                if (Left != null)
                {
                    if (index < Left.Count)
                    {
                        return Left[index];
                    }
                    index -= Left.Count;
                }
                if (index-- == 0)
                {
                    return Value;
                }
                if (Right != null)
                {
                    if (index < Right.Count)
                    {
                        return Right[index];
                    }
                }
                throw new ArgumentOutOfRangeException(nameof(index), "指定されたインデックスは範囲外です。");
            }
            set
            {
                throw new InvalidOperationException("インデックスによる設定操作はサポートされていません。");
            }
        }

        /// <summary>
        /// このノードをルートとする部分木の要素を昇順で指定されたリストに追加します（中間順巡回）。
        /// </summary>
        /// <param name="result">要素を追加するリスト。</param>
        public void InOrderTraversal(List<T> result)
        {
            Left?.InOrderTraversal(result);
            result.Add(Value);
            Right?.InOrderTraversal(result);
        }
    }
}
