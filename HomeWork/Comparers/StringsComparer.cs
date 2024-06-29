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
        //Case(In)sensitive processing
        var s1 = _stringPreprocessor.Process(string1);
        var s2 = _stringPreprocessor.Process(string2);

        //Work with this functiuon as this function will just present similarity between segments to end user so it doesn't need to be 100% accurate but it needs to be fast

        //TODO optimize to do it faster.
        //TODO create case insensitive matching as well.
        //TODO comment the code, what is wrong what is lovering accuracy why its improving performance etc...
        //TODO do NOT use paralel or threading or tasks.
        //TODO BONUS: Try to find a different Levenshtein implementation and refactor the solution a bit to enable use of both implementations

        // For mathematical operations, if possible, it's best to use Math class as they should be wll optimized and foremost more readable
        var maxDistance = Math.Max(s1.Length, s2.Length);

        //Trimming the common prefix, documented at its implementation
        var prefixTrimmed = _stringManipulator.TrimPrefix(s1, s2);
        if (AnyStringIsNullOrEmpty(prefixTrimmed))
        {
            return _similarityCalculator.CalculatePercentSimilarity(
                maxDistance,
                Math.Abs(s1.Length - s2.Length)
            );
        }

        //Trimming the common suffix, documented at its implementation
        var suffixTrimmed = _stringManipulator.TrimSuffix(
            prefixTrimmed.Item1,
            prefixTrimmed.Item2
        );
        if (AnyStringIsNullOrEmpty(suffixTrimmed))
        {
            return _similarityCalculator.CalculatePercentSimilarity(
                maxDistance,
                Math.Abs(s1.Length - s2.Length)
            );
        }

        // This is the biggest bottleneck in the code. As the implementation cannot be changed, the aim is either lower the number of it's calls or shorten the input strings
        var distance = _levenshteinCalculator.Calculate(
            suffixTrimmed.Item1,
            suffixTrimmed.Item2
        );

        return _similarityCalculator.CalculatePercentSimilarity(maxDistance, distance);
    }

    private static bool AnyStringIsNullOrEmpty((string, string) prefixTrimmed)
    {
        return string.IsNullOrEmpty(prefixTrimmed.Item1)
            || string.IsNullOrEmpty(prefixTrimmed.Item2);
    }
}
