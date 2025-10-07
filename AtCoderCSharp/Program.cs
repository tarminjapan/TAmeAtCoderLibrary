using System.Diagnostics.CodeAnalysis;
using TAmeAtCoderLibrary;
using TAmeAtCoderLibrary.Utilities;

internal class Program
{
    static void Main()
    {
        SourceExpander.Expander.Expand();
        Common.EnableConsoleBuffering();
        var solve = new Solve();
        solve.Run();
        Common.FlushConsoleBuffer();
    }

    public class Solve
    {
        [SuppressMessage("Performance", "CA1822")]
        public void Run()
        {
            var iN = ReadLine.Int();
            var iA = ReadLine.Ints();

            var stack = new Stack<int>();
            var queue = new Queue<int>();
            var ans = 0;

            for (int i = 0; i < iN / 2; i++)
                stack.Push(iA[i]);

            for (int i = 0; i < iN / 2; i++)
                queue.Enqueue(iA[^(i + 1)]);

            while (0 < stack.Count)
            {
                var a = stack.Pop();

                if (a * 2 <= queue.Peek())
                {
                    queue.Dequeue();
                    ans++;
                }
            }

            Console.WriteLine(ans);
        }
    }
}