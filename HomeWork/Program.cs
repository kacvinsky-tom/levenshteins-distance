using System;
using System.Data;
using System.Diagnostics;

namespace HomeWork
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            Stopwatch ws = new Stopwatch();
            ws.Restart();

            for (int x = 1; x < 50000; x = x + 1)
            {
                CompareStrings("xxxxxxxxxxx xxxxxxxxxxxxxxxxx xxxxxxxxxxxxx", $"xxxxxxxxxxx xxxxxxxxxxxxxxxxx xxxxxxxxxxxxy{x} yyyyyyyy xaaaaaaa", 70);
                CompareStrings("yyy xxxxxx xxxxxxxxxxxxx", "xxxxxxxxxxx xxxxxxxxxxxxxxxxx xxxxxxxxxxxxy", 70);
                CompareStrings("xxxxxxxxxxx xxxxxxx", "xxxxxxxxxxx xxxxxyyyyxxxxxxxx xxxxxxxxxxxxy", 70);
                CompareStrings($"xxxxxxxxxxx {x}xxxxxxxxxxxxxxxxx xxxxxxxxxxxxx", "xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy", 50);

                CompareStrings("xxxxxxxxxxx", "xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy", 50);
                CompareStrings("xxxxxxxxxxx", "xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy", 50);
                CompareStrings("xxxxxxxxxxx", $"xxxxxxxxxxx xxxxxxxxxxxxxxxxx{x} yyyyyyyyyyyyy", 50);
                CompareStrings("xxxxxxxxxxx xxxxxxxxxxxxxxxxx", "xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy xxxxxxxxx", 50);
                CompareStrings("xxxxxxxxxxx xxxxxxxxxxxxxxxxx xxxxxxxxxxxxx", "xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy xxxxxxxx yyyyyyyy", 50);

                CompareStrings("xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy", "xxxxxxxxxxx", 50);
                CompareStrings("xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy", $"xxxxxxxxxxx{x}", 50);
                CompareStrings("xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy", $"xxxxx{x}xxxxxx", 50);
                CompareStrings("xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy xxxxxxxxx", "xxxxxxxxxxx xxxxxxxxxxxxxxxxx", 60);

                CompareStrings($"xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy xxxxxxxx {x}yyyyyyyy", "xxxxxxxxxxx xxxxxxxxxxxxxxxxx xxxxxxxxxxxxx", 50);
            }

            ws.Stop();
            Console.WriteLine($"Finished in: {ws.ElapsedTicks}");
            Console.WriteLine($"Finished in: {ws.Elapsed}");
            Console.ReadKey();
        }

        public static int CompareStrings(string s, string t, int minPercentage = 0)
        {
            //Work with this functiuon as this function will just present similarity between segments to end user so it doesn't need to be 100% accurate but it needs to be fast

            //TODO optimize to do it faster.
            //TODO create case insensitive matching as well.
            //TODO comment the code, what is wrong what is lovering accuracy why its improving performance etc...
            //TODO do NOT use paralel or threading or tasks.
            //TODO BONUS: Try to find a different Levenshtein implementation and refactor the solution a bit to enable use of both implementations

            int maxDistance = s.Length;
            if (t.Length > maxDistance)
            {
                maxDistance = t.Length;
            }

            int distance = LevenshteinDistance(s, t);

            int percentSimilarity = 0;
            if (distance == 0)
            {
                percentSimilarity = 100;
            }
            else
            {
                percentSimilarity = (int)((float)100 / maxDistance) * (maxDistance - distance);
            }
            return percentSimilarity;
        }


        /// <summary>
        /// Levenshteins the distance. (Some very old levenstain, please do not change :))
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        private static int LevenshteinDistance(string s, string t)
        {
            int[,] d = new int[s.Length + 1, t.Length + 1];
            for (int i = 0; i <= s.Length; i++)
                d[i, 0] = i;
            for (int j = 0; j <= t.Length; j++)
                d[0, j] = j;
            for (int j = 1; j <= t.Length; j++)
            for (int i = 1; i <= s.Length; i++)
                if (CharCompare(s[i - 1], t[j - 1]))
                    d[i, j] = d[i - 1, j - 1];  //no operation
                else
                    d[i, j] = Math.Min(Math.Min(
                            d[i - 1, j] + 1,    //a deletion
                            d[i, j - 1] + 1),   //an insertion
                        d[i - 1, j - 1] + 1 //a substitution
                    );
            return d[s.Length, t.Length];
        }

        private static bool CharCompare(char a, char b)
        {
            return a == b;
        }
    }
}
