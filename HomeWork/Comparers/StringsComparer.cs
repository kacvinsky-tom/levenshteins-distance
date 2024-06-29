using System;
using HomeWork.Levenshtein;
using HomeWork.Utilities;

namespace HomeWork.Comparers;

public class StringsComparer
{
    private readonly ILevenshteinDistance _levenshteinCalculator;
    private readonly IStringManipulator _stringManipulator;

    public StringsComparer(
        ILevenshteinDistance levenshteinCalculator,
        IStringManipulator stringManipulator
    )
    {
        _levenshteinCalculator = levenshteinCalculator;
        _stringManipulator = stringManipulator;
    }

    public int Compare(string first, string second, int minPercentage = 0)
    {
        //Work with this functiuon as this function will just present similarity between segments to end user so it doesn't need to be 100% accurate but it needs to be fast

        //TODO optimize to do it faster.
        //TODO create case insensitive matching as well.
        //TODO comment the code, what is wrong what is lovering accuracy why its improving performance etc...
        //TODO do NOT use paralel or threading or tasks.
        //TODO BONUS: Try to find a different Levenshtein implementation and refactor the solution a bit to enable use of both implementations

        // For mathematical operations, if possible, it's best to use Math class as they should be wll optimized and foremost more readable
        var maxDistance = Math.Max(first.Length, second.Length);

        //Trimming the common prefix, documented at its implementation
        var stringsPrefixTrimmed = _stringManipulator.TrimPrefix(first, second);
        if (AnyStringIsNullOrEmpty(stringsPrefixTrimmed))
        {
            return CalculatePercentSimilarity(maxDistance, Math.Abs(first.Length - second.Length));
        }

        //Trimming the common suffix, documented at its implementation
        var stringsSuffixTrimmed = _stringManipulator.TrimSuffix(
            stringsPrefixTrimmed.Item1,
            stringsPrefixTrimmed.Item2
        );
        if (AnyStringIsNullOrEmpty(stringsSuffixTrimmed))
        {
            return CalculatePercentSimilarity(maxDistance, Math.Abs(first.Length - second.Length));
        }

        // This is the biggest bottleneck in the code. As the implementation cannot be changed, the aim is either lower the number of it's calls or shorten the input strings
        var distance = _levenshteinCalculator.Calculate(
            stringsSuffixTrimmed.Item1,
            stringsSuffixTrimmed.Item2
        );

        return CalculatePercentSimilarity(maxDistance, distance);
    }

    private static int CalculatePercentSimilarity(int maxDistance, int distance)
    {
        if (maxDistance == 0)
        {
            return 100;
        }
        //If not extra necessary, we want to ideally avoid explicit type casting
        // -> https://www.codeproject.com/Articles/8052/Type-casting-impact-over-execution-performance-in
        return 100 * (maxDistance - distance) / maxDistance;
    }

    private static bool AnyStringIsNullOrEmpty((string, string) prefixTrimmed)
    {
        return string.IsNullOrEmpty(prefixTrimmed.Item1)
            || string.IsNullOrEmpty(prefixTrimmed.Item2);
    }
}
