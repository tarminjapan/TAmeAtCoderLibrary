using TAmeAtCoderLibrary;
using TAmeAtCoderLibrary.Utilities;

internal class Program
{
    static void Main()
    {
        SourceExpander.Expander.Expand();
        Common.EnableConsoleBuffering(); ;
        Solve.Run();
        Common.FlushConsoleBuffer();
    }

    public class Solve
    {
        public static void Run()
        {
            // https://atcoder.jp/contests/abc261/tasks/abc261_e

            var inputs = ReadLine.Ints();
            int iN = inputs[0], iC = inputs[1];
            var iTA = ReadLine.IntMatrix(iN);

            var maxDigits = 30;
            var tbins = Enumerable.Range(0, 2).Select(i => Enumerable.Repeat(i, maxDigits).ToArray()).ToArray();
            var cdec = iC;

            for (int i = 0; i < iN; i++)
            {
                int t = iTA[i][0], a = iTA[i][1];

                if (t == 1)
                {
                    if (1 <= i)
                        cdec = GetCurrent(cdec, tbins, maxDigits);

                    cdec &= a;

                    var abin = Numeric.ConvertToBinary(a, maxDigits);

                    for (int j = 0; j < maxDigits; j++)
                    {
                        tbins[0][j] &= abin[j];
                        tbins[1][j] &= abin[j];
                    }
                }
                else if (t == 2)
                {
                    if (1 <= i)
                        cdec = GetCurrent(cdec, tbins, maxDigits);

                    cdec |= a;

                    var abin = Numeric.ConvertToBinary(a, maxDigits);

                    for (int j = 0; j < maxDigits; j++)
                    {
                        tbins[0][j] |= abin[j];
                        tbins[1][j] |= abin[j];
                    }
                }
                else
                {
                    if (1 <= i)
                        cdec = GetCurrent(cdec, tbins, maxDigits);

                    cdec ^= a;

                    var abin = Numeric.ConvertToBinary(a, maxDigits);

                    for (int j = 0; j < maxDigits; j++)
                    {
                        tbins[0][j] ^= abin[j];
                        tbins[1][j] ^= abin[j];
                    }
                }

                Console.WriteLine(cdec);
            }
        }

        public static int GetCurrent(int cdec, int[][] tbins, int maxDigits)
        {
            var bin = Numeric.ConvertToBinary(cdec, 30);

            for (int i = 0; i < maxDigits; i++)
                bin[i] = tbins[bin[i]][i];

            return (int)Numeric.ConvertToDecimal(bin, 2);
        }
    }
}