using System;

namespace HomeWork.Utilities;

public class StringManipulator : IStringManipulator
{
    public (string, string) TrimPrefix(string first, string second)
    {
        //First idea was to optimize the performance without lowering the accuracy
        //This method trims the common prefix of both strings before passing them to the levenshteinCalculator
        //The complexity is max O(n), which compared to minimal O(n*n) of levenshteinCalculator is insignificant
        //Based on DotNetBenchmark, the mean time of the Compare method lowered from 4.486s to 1.041s by trimming the common prefix
        var minLength = Math.Min(first.Length, second.Length);
        var prefixLength = 0;
        for (int i = 0; i < minLength; i++)
        {
            if (first[i] == second[i])
            {
                prefixLength++;
            }
            else
            {
                break;
            }
        }

        var firstPrefixTrimmed = first.Substring(prefixLength);
        var secondPrefixTrimmed = second.Substring(prefixLength);

        return (firstPrefixTrimmed, secondPrefixTrimmed);
    }

    public (string, string) TrimSuffix(string first, string second)
    {
        // This method trims the common suffix of both strings before passing them to the levenshteinCalculator
        // The complexity is max O(n), similar to the CommonPrefixLength method
        var minLength = Math.Min(first.Length, second.Length);
        var suffixLength = 0;
        for (var i = 1; i <= minLength; i++)
        {
            if (first[^i] == second[^i])
            {
                suffixLength++;
            }
            else
            {
                break;
            }
        }

        var firstTrimmed = first.Substring(0, first.Length - suffixLength);
        var secondTrimmed = second.Substring(0, second.Length - suffixLength);

        return (firstTrimmed, secondTrimmed);
    }
}
