using System.Diagnostics;
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
#region Expanded by https://github.com/kzrnm/SourceExpander
namespace SourceExpander{public class Expander{[Conditional("EXP")]public static void Expand(string inputFilePath=null,string outputFilePath=null,bool ignoreAnyError=true){}public static string ExpandString(string inputFilePath=null,bool ignoreAnyError=true){return "";}}}
namespace TAmeAtCoderLibrary { public class ReadLine { public static double Double() => double.Parse(String()); public static double[] Doubles() => Strings().Select(double.Parse).ToArray(); public static double[] Doubles(int height) => Enumerable.Range(0, height).Select(_ => Double()).ToArray(); public static double[][] DoubleMatrix(int height) => Enumerable.Range(0, height).Select(_ => Doubles()).ToArray(); public static decimal Decimal() => decimal.Parse(String()); public static decimal[] Decimals() => Strings().Select(decimal.Parse).ToArray(); public static decimal[] Decimals(int height) => Enumerable.Range(0, height).Select(_ => Decimal()).ToArray(); public static decimal[][] DecimalMatrix(int height) => Enumerable.Range(0, height).Select(_ => Decimals()).ToArray(); public static int Int() => int.Parse(String()); public static int[] Ints() => Strings().Select(int.Parse).ToArray(); public static int[] Ints(int height) => Enumerable.Range(0, height).Select(_ => Int()).ToArray(); public static int[][] IntMatrix(int height) => Enumerable.Range(0, height).Select(_ => Ints()).ToArray(); public static long Long() => long.Parse(String()); public static long[] Longs() => Strings().Select(long.Parse).ToArray(); public static long[] Longs(int height) => Enumerable.Range(0, height).Select(_ => Long()).ToArray(); public static long[][] LongMatrix(long height) => Enumerable.Range(0, (int)height).Select(_ => Longs()).ToArray(); public static string String() => Console.ReadLine().TrimStart().TrimEnd(); public static string[] Strings() => Console.ReadLine().TrimStart().TrimEnd().Split(); public static string[] Strings(int height) => Enumerable.Range(0, height).Select(_ => String()).ToArray(); public static string[][] StringMatrix(int height) => Enumerable.Range(0, height).Select(_ => Strings()).ToArray(); public static char[][] CharMatrix(int height) => Enumerable.Range(0, height).Select(_ => String().ToCharArray()).ToArray(); } }
namespace TAmeAtCoderLibrary.Utilities { public class Common { public static char[] ALPHABETS => "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(); public static char[] alphabets => "abcdefghijklmnopqrstuvwxyz".ToCharArray();  public static long[] GenerateCumSum(long[] array) { var ary = new long[array.Length]; for (int i = 0; i < array.Length; i++) { ary[i] = array[i]; if (1 <= i) ary[i] += ary[i - 1]; }  return ary; }  public static int[] GenerateCumSum(int[] array) { var ary = new int[array.Length]; for (int i = 0; i < array.Length; i++) { ary[i] = array[i]; if (1 <= i) ary[i] += ary[i - 1]; }  return ary; }  public static Queue<Queue<int>> CombinationLists(int max, int digits) { var queue = new Queue<Queue<int>>(); __CombitaionLists(queue, new LinkedList<int>(), 1, max, digits); return queue; }  private static void __CombitaionLists(Queue<Queue<int>> queue, LinkedList<int> linkedList, int min, int max, int depth) { if (depth == 0) { var tqueue = new Queue<int>(); foreach (var current in linkedList) tqueue.Enqueue(current); queue.Enqueue(tqueue); return; }  for (int i = min; i <= max; i++) { linkedList.AddLast(i); __CombitaionLists(queue, linkedList, i + 1, max, depth - 1); linkedList.RemoveLast(); } }  public static Queue<Queue<int>> PermutationLists(int max, int digits) { var queue = new Queue<Queue<int>>(); __PermurationLists(queue, new LinkedList<int>(), new bool[max + 1], max, digits); return queue; }  private static void __PermurationLists(Queue<Queue<int>> queue, LinkedList<int> linkedList, bool[] selected, int max, int depth) { if (depth == 0) { var tqueue = new Queue<int>(); foreach (var current in linkedList) tqueue.Enqueue(current); queue.Enqueue(tqueue); return; }  for (int i = 1; i <= max; i++) { if (selected[i]) continue; selected[i] = true; linkedList.AddLast(i); selected[i] = true; __PermurationLists(queue, linkedList, selected, max, depth - 1); linkedList.RemoveLast(); selected[i] = false; } }  public static long Pow(long x, int y) { var pow = 1L; for (int i = 0; i < y; i++) pow *= x; return pow; }  public static long Ceiling(long bloken, long divided) => bloken % divided == 0L ? bloken / divided : bloken / divided + 1L; public static int Ceiling(int bloken, int divided) => (int)Ceiling((long)bloken, divided); public static void DisableAutoFlush() { Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false }); }  public static void Flush() { Console.Out.Flush(); } } }
#endregion Expanded by https://github.com/kzrnm/SourceExpander
