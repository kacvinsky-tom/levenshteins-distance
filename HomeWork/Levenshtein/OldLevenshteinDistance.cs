using System;
using HomeWork.Comparers;

namespace HomeWork.Levenshtein;

public class OldLevenshteinDistance : ILevenshteinDistance
{
    /// <summary>
    /// Levenshteins the distance. (Some very old levenstain, please do not change :))
    /// </summary>
    /// <param name="s">The s.</param>
    /// <param name="t">The t.</param>
    /// <returns></returns>
    public int Calculate(string s, string t)
    {
        int[,] d = new int[s.Length + 1, t.Length + 1];
        for (int i = 0; i <= s.Length; i++)
            d[i, 0] = i;
        for (int j = 0; j <= t.Length; j++)
            d[0, j] = j;
        for (int j = 1; j <= t.Length; j++)
        for (int i = 1; i <= s.Length; i++)
            if (CharCompare(s[i - 1], t[j - 1]))
                d[i, j] = d[i - 1, j - 1]; //no operation
            else
                d[i, j] = Math.Min(
                    Math.Min(
                        d[i - 1, j] + 1, //a deletion
                        d[i, j - 1] + 1
                    ), //an insertion
                    d[i - 1, j - 1] + 1 //a substitution
                );
        return d[s.Length, t.Length];
    }

    private static bool CharCompare(char a, char b)
    {
        return CharsComparer.Compare(a, b);
    }
}
