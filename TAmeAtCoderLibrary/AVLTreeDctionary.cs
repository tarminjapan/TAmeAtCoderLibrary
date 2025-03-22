namespace TAmeAtCoderLibrary;

using System;

/// <summary>
/// AVL木を使用して、キーの順序を維持する辞書の実装です。
/// このクラスは辞書の高速な検索とAVL木の順序付き走査機能を組み合わせています。
/// </summary>
/// <typeparam name="T1">辞書内のキーの型。IComparableを実装している必要があります。</typeparam>
/// <typeparam name="T2">辞書内の値の型。</typeparam>
public class AvlTreeDictionary<T1, T2> where T1 : IComparable<T1>
{
    private readonly AvlTree<T1> avl = new();
    private readonly Dictionary<T1, T2> dic = new();

    /// <summary>最小のキーを取得します</summary>
    public T1 MinKey => avl.MinKey;
    /// <summary>最大のキーを取得します</summary>
    public T1 MaxKey => avl.MaxKey;
    /// <summary>最小のキーに対応する値を取得します</summary>
    public T2 MinKeyValue => dic[avl.MinKey];
    /// <summary>最大のキーに対応する値を取得します</summary>
    public T2 MaxKeyValue => dic[avl.MaxKey];
    /// <summary>格納されているキーと値のペアの数を取得します</summary>
    public int Count => avl.Count;

    /// <summary>指定されたキーが辞書に存在するかどうかを確認します</summary>
    /// <param name="key">確認するキー</param>
    /// <returns>キーが存在する場合はtrue、それ以外はfalse</returns>
    public bool ContainsKey(T1 key) => avl.Contains(key);

    /// <summary>指定されたキーと値のペアを辞書に追加します</summary>
    /// <param name="key">追加するキー</param>
    /// <param name="value">追加する値</param>
    public void Add(T1 key, T2 value)
    {
        avl.Add(key);
        dic.Add(key, value);
    }

    /// <summary>既にキーが存在しない場合のみ、指定されたキーと値のペアを辞書に追加します</summary>
    /// <param name="key">追加するキー</param>
    /// <param name="value">追加する値</param>
    public void TryAdd(T1 key, T2 value)
    {
        if (!dic.ContainsKey(key))
            Add(key, value);
    }

    /// <summary>指定されたキーに対応する値を取得します</summary>
    /// <param name="key">値を取得するキー</param>
    /// <returns>指定されたキーに対応する値</returns>
    public T2 GetValue(T1 key) => dic[key];

    /// <summary>指定されたキーに対応する値を設定します。キーが存在しない場合は追加します</summary>
    /// <param name="key">設定するキー</param>
    /// <param name="value">設定する値</param>
    public void SetValue(T1 key, T2 value)
    {
        if (!dic.ContainsKey(key))
            Add(key, value);
        else
            dic[key] = value;
    }

    /// <summary>指定されたキーとそれに対応する値を辞書から削除します</summary>
    /// <param name="key">削除するキー</param>
    public void Remove(T1 key)
    {
        avl.Remove(key);
        dic.Remove(key);
    }

    /// <summary>指定されたキーより小さい最大のキーを取得します</summary>
    /// <param name="key">基準となるキー</param>
    /// <param name="minKey">取得できない場合に返すデフォルト値</param>
    /// <returns>指定されたキーより小さい最大のキー、または存在しない場合はminKey</returns>
    public T1 GetBelowKey(T1 key, T1 minKey) => avl.GetBelow(key, minKey);

    /// <summary>指定されたキーより大きい最小のキーを取得します</summary>
    /// <param name="key">基準となるキー</param>
    /// <param name="maxKey">取得できない場合に返すデフォルト値</param>
    /// <returns>指定されたキーより大きい最小のキー、または存在しない場合はmaxKey</returns>
    public T1 GetNextKey(T1 key, T1 maxKey) => avl.GetNext(key, maxKey);

    /// <summary>キーを昇順に並べたリストを取得します</summary>
    /// <returns>順序付けされたキーのリスト</returns>
    public List<T1> InOrderKeys() => avl.InOrder();

    /// <summary>キーの昇順に対応する値のリストを取得します</summary>
    /// <returns>キーの昇順に対応する値のリスト</returns>
    public List<T2> InOrderValues()
    {
        var list1 = InOrderKeys();
        var list2 = new List<T2>();

        foreach (var key in list1)
            list2.Add(dic[key]);

        return list2;
    }
}