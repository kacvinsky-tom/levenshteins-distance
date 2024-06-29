namespace HomeWork.Calculators;

public class SimilarityCalculator : ISimilarityCalculator
{
    public int CalculatePercentSimilarity(int maxDistance, int distance)
    {
        if (maxDistance == 0)
        {
            return 100;
        }
        return 100 * (maxDistance - distance) / maxDistance;
    }
}