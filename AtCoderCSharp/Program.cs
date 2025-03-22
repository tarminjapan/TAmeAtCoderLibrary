using TAmeAtCoderLibrary;
using TAmeAtCoderLibrary.Utilities;

internal class Program
{
    static void Main()
    {
        SourceExpander.Expander.Expand();
        Common.DisableAutoFlush();
        Solve.Run();
        Common.Flush();
    }

    public class Solve
    {
        public static void Run()
        {
            var iN = ReadLine.Int();
            var iAB = ReadLine.IntMatrix(iN - 1);

            var linkedList = new LinkedList<int>();
            var dic = new Dictionary<int, SortedSet<int>>();

            foreach (var ab in iAB)
            {
                int a = ab[0], b = ab[1];
                dic.TryAdd(a, new SortedSet<int>());
                dic.TryAdd(b, new SortedSet<int>());
                dic[a].Add(b);
                dic[b].Add(a);
            }

            AddRoute(linkedList, 1, dic);

            Console.WriteLine(string.Join(" ", linkedList));
        }

        public static void AddRoute(LinkedList<int> linkedList, int current, Dictionary<int, SortedSet<int>> dic)
        {
            linkedList.AddLast(current);

            if (dic.TryGetValue(current, out var nexts))
                foreach (var next in nexts.ToArray())
                {
                    dic[current].Remove(next);
                    dic[next].Remove(current);
                    AddRoute(linkedList, next, dic);
                    linkedList.AddLast(current);
                }
        }
    }
}