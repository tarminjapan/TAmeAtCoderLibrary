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
        public void Run()
        {
            var inputs = ReadLine.Ints();
            int iN = inputs[0], iM = inputs[1], iQ = inputs[2];
            var iABC = ReadLine.IntMatrix(iM);
            var iUVW = ReadLine.IntMatrix(iQ);

            var uf = new UnionFindTree<int>();
            var edges1 = iABC.Select((x, i) => new int[] { x[0], x[1], x[2], 0, i }).ToArray();
            var edges2 = iUVW.Select((x, i) => new int[] { x[0], x[1], x[2], 1, i }).ToArray();
            var edges = edges1.Concat(edges2).OrderBy(x => x[2]).ToArray();
            var res = new bool[iQ];

            for (int i = 1; i <= iN; i++)
                uf.Add(i);

            foreach (var edge in edges)
            {
                int v1 = edge[0], v2 = edge[1], w = edge[2], t = edge[3], i = edge[4];

                var r1 = uf.FindRoot(v1);
                var r2 = uf.FindRoot(v2);

                if (r1 == r2)
                    continue;

                if (t == 1)
                    res[i] = true;
                else
                    uf.Union(r1, r2);
            }

            foreach (var bl in res)
                Console.WriteLine(bl ? "Yes" : "No");
        }
    }
}