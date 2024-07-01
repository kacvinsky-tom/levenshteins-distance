using HomeWork.Calculators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HomeWorkTests;

[TestClass]
public class SimilarityCalculatorTests
{
    private readonly SimilarityCalculator _similarityCalculator = new();

    [TestMethod]
    [DataRow(0, 1)]
    public void CalculateSimilarity_MaxDistanceIsZero_Returns100(int maxDistance, int distance)
    {
        var result = _similarityCalculator.CalculatePercentSimilarity(maxDistance, distance);

        Assert.AreEqual(100, result);
    }

    [TestMethod]
    [DataRow(6, 2, 66)]
    [DataRow(8, 3, 62)]
    [DataRow(5, 1, 80)]
    [DataRow(7, 3, 57)]
    [DataRow(10, 4, 60)]
    public void CalculateSimilarity_MaxDistanceIsNotZero_ReturnsCorrectPercent(
        int maxDistance,
        int distance,
        int correctPercent
    )
    {
        var result = _similarityCalculator.CalculatePercentSimilarity(maxDistance, distance);

        Assert.AreEqual(correctPercent, result);
    }
}
