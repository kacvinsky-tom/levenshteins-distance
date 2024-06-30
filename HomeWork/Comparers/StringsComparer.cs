using System;
using HomeWork.Calculators;
using HomeWork.Levenshtein;
using HomeWork.Utilities;

namespace HomeWork.Comparers;

public class StringsComparer
{
    private readonly ILevenshteinDistance _levenshteinCalculator;
    private readonly IStringManipulator _stringManipulator;
    private readonly ISimilarityCalculator _similarityCalculator;
    private readonly IStringPreprocessor _stringPreprocessor;

    public StringsComparer(
        ILevenshteinDistance levenshteinCalculator,
        IStringManipulator stringManipulator,
        ISimilarityCalculator similarityCalculator,
        IStringPreprocessor stringPreprocessor
    )
    {
        _levenshteinCalculator = levenshteinCalculator;
        _stringManipulator = stringManipulator;
        _similarityCalculator = similarityCalculator;
        _stringPreprocessor = stringPreprocessor;
    }

    public int Compare(string string1, string string2, int minPercentage = 0)
    {
        // For mathematical operations, if possible, it's best to use Math class as they should be wll optimized and foremost more readable
        var maxLength = Math.Max(string1.Length, string2.Length);
        var minLength = Math.Min(string1.Length, string2.Length);

        // If it's impossible to reach the minimum percentage, early exit
        // This is lowering accuracy, but can significantly lower the number of calculations, which bumps the performance especially with longer strings
        // I also avoid type casting, so the accuracy a little bit lower, however performance better
        // This reduced the mean time of the method execution by +- 500ms
        if (minLength * 100 / maxLength < minPercentage)
        {
            return 0;
        }

        //Case(In)sensitive processing
        var s1 = _stringPreprocessor.Process(string1);
        var s2 = _stringPreprocessor.Process(string2);

        //Trimming the common prefix, documented at its implementation
        var prefixTrimmed = _stringManipulator.TrimPrefix(s1, s2);
        if (AnyStringIsNullOrEmpty(prefixTrimmed))
        {
            return _similarityCalculator.CalculatePercentSimilarity(
                maxLength,
                Math.Abs(s1.Length - s2.Length)
            );
        }

        //Trimming the common suffix, documented at its implementation
        var suffixTrimmed = _stringManipulator.TrimSuffix(prefixTrimmed.Item1, prefixTrimmed.Item2);
        if (AnyStringIsNullOrEmpty(suffixTrimmed))
        {
            return _similarityCalculator.CalculatePercentSimilarity(
                maxLength,
                Math.Abs(s1.Length - s2.Length)
            );
        }

        // This is the biggest bottleneck in the code. As the implementation cannot be changed, the aim is either lower the number of it's calls or shorten the input strings
        var distance = _levenshteinCalculator.Calculate(suffixTrimmed.Item1, suffixTrimmed.Item2);

        return _similarityCalculator.CalculatePercentSimilarity(maxLength, distance);
    }

    private static bool AnyStringIsNullOrEmpty((string, string) prefixTrimmed)
    {
        return string.IsNullOrEmpty(prefixTrimmed.Item1)
            || string.IsNullOrEmpty(prefixTrimmed.Item2);
    }
}
