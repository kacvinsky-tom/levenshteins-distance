using System;

namespace HomeWork.Levenshtein;

public class BonusLevenshteinDistance : ILevenshteinDistance
{
    //Reduced space complexity to O(n+m)
    public int Calculate(string source, string target)
    {
        int sourceLength = source.Length;
        int targetLength = target.Length;

        int[] previousRow = new int[targetLength + 1];
        int[] currentRow = new int[targetLength + 1];

        for (int j = 0; j <= targetLength; j++)
        {
            previousRow[j] = j;
        }

        for (int i = 1; i <= sourceLength; i++)
        {
            currentRow[0] = i;

            for (int j = 1; j <= targetLength; j++)
            {
                int cost = (source[i - 1] == target[j - 1]) ? 0 : 1;

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
