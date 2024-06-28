using HomeWork.Levenshtein;

namespace HomeWork.Comparers;

public class StringsComparer
{
    private readonly ILevenshteinDistance _levenshteinCalculator;

    public StringsComparer(ILevenshteinDistance levenshteinCalculator)
    {
        _levenshteinCalculator = levenshteinCalculator;
    }

    public int Compare(string first, string second, int minPercentage = 0)
    {
        //Work with this functiuon as this function will just present similarity between segments to end user so it doesn't need to be 100% accurate but it needs to be fast

        //TODO optimize to do it faster.
        //TODO create case insensitive matching as well.
        //TODO comment the code, what is wrong what is lovering accuracy why its improving performance etc...
        //TODO do NOT use paralel or threading or tasks.
        //TODO BONUS: Try to find a different Levenshtein implementation and refactor the solution a bit to enable use of both implementations

        var maxDistance = first.Length;
        if (second.Length > maxDistance)
        {
            maxDistance = second.Length;
        }

        var distance = _levenshteinCalculator.Calculate(first, second);

        var percentSimilarity = 0;
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
}
