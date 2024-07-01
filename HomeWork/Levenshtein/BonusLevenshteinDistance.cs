using System;

namespace HomeWork.Levenshtein;

public class BonusLevenshteinDistance : ILevenshteinDistance
{
    //Reduced space complexity to O(n+m)
    public int Calculate(string source, string target)
    {
        var sourceLength = source.Length;
        var targetLength = target.Length;

        var previousRow = new int[targetLength + 1];
        var currentRow = new int[targetLength + 1];

        for (var j = 0; j <= targetLength; j++)
        {
            previousRow[j] = j;
        }

        for (var i = 1; i <= sourceLength; i++)
        {
            currentRow[0] = i;

            for (var j = 1; j <= targetLength; j++)
            {
                var cost = (source[i - 1] == target[j - 1]) ? 0 : 1;

                currentRow[j] = Math.Min(
                    Math.Min(
                        currentRow[j - 1] + 1, // Insertion
                        previousRow[j] + 1
                    ), // Deletion
                    previousRow[j - 1] + cost
                ); // Substitution
            }

            var temp = previousRow;
            previousRow = currentRow;
            currentRow = temp;
        }

        return previousRow[targetLength];
    }
}
