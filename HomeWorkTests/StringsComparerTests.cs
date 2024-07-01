using HomeWork.Calculators;
using HomeWork.Comparers;
using HomeWork.Levenshtein;
using HomeWork.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HomeWorkTests;

[TestClass]
public class StringsComparerTests
{
    private readonly StringsComparer _comparer;
    private readonly Mock<ILevenshteinDistance> _levenshteinCalculatorMock;
    private readonly Mock<IStringManipulator> _stringManipulatorMock;
    private readonly Mock<ISimilarityCalculator> _similarityCalculatorMock;
    private readonly Mock<IStringPreprocessor> _stringPreprocessorMock;

    public StringsComparerTests()
    {
        _levenshteinCalculatorMock = new Mock<ILevenshteinDistance>();
        _stringManipulatorMock = new Mock<IStringManipulator>();
        _similarityCalculatorMock = new Mock<ISimilarityCalculator>();
        _stringPreprocessorMock = new Mock<IStringPreprocessor>();

        _comparer = new StringsComparer(
            _levenshteinCalculatorMock.Object,
            _stringManipulatorMock.Object,
            _similarityCalculatorMock.Object,
            _stringPreprocessorMock.Object
        );
    }

    [TestMethod]
    [DataRow("testing", "testing")]
    [DataRow("this is the same string", "this is the same string")]
    public void Compare_IdenticalStrings_Returns100PercentSimilarity(string s1, string s2)
    {
        SetupMocks(s1, s2, string.Empty, string.Empty, string.Empty, string.Empty, 0, 100, true);

        var result = _comparer.Compare(s1, s2);

        Assert.AreEqual(100, result);
    }

    [TestMethod]
    [DataRow("abcde", "vwxyz")]
    [DataRow("12345", "6789")]
    public void Compare_CompletelyDifferentStrings_Returns0PercentSimilarity(string s1, string s2)
    {
        SetupMocks(s1, s2, s1, s2, s1, s2, Math.Max(s1.Length, s2.Length), 0, true);

        var result = _comparer.Compare(s1, s2);

        Assert.AreEqual(0, result);
    }

    [TestMethod]
    [DataRow("", "")]
    public void Compare_EmptyStrings_Returns100PercentSimilarity(string s1, string s2)
    {
        SetupMocks(s1, s2, string.Empty, string.Empty, string.Empty, string.Empty, 0, 100, true);

        var result = _comparer.Compare(s1, s2);

        Assert.AreEqual(100, result);
    }

    [TestMethod]
    [DataRow("", "second")]
    [DataRow("first", "")]
    public void Compare_EmptyAndNonEmptyString_Returns0PercentSimilarity(string s1, string s2)
    {
        SetupMocks(s1, s2, s1, s2, s1, s2, 6, 0, true);

        SetupMocks(s1, s2, s1, s2, s1, s2, 5, 0, true);

        var result = _comparer.Compare(s1, s2);

        Assert.AreEqual(0, result);
    }

    [TestMethod]
    [DataRow("THIS IS THE SAME STRING", "this is the same string")]
    [DataRow("also This is The same String", "alsO thiS is the samE strIng")]
    public void Compare_CaseInsensitiveIdenticalStrings_ReturnsCorrectSimilarity(
        string s1,
        string s2
    )
    {
        SetupMocks(s1, s2, string.Empty, string.Empty, string.Empty, string.Empty, 0, 100, false);

        var result = _comparer.Compare(s1, s2);

        Assert.AreEqual(100, result);
    }

    [TestMethod]
    [DataRow("short string", "this is not that short string", 50)]
    public void Compare_StringsWhereMinPercentageNotReachable_Returns0percent(
        string s1,
        string s2,
        int minPercentage
    )
    {
        var result = _comparer.Compare(s1, s2, minPercentage);

        Assert.AreEqual(0, result);
    }

    [TestMethod]
    [DataRow("123456", "123789", 50)]
    [DataRow("abcdefga", "a123456a", 20)]
    public void Compare_StringsWhereMinPercentageReachable_ReturnsCorrectPercent(
        string s1,
        string s2,
        int expectedSimilarity
    )
    {
        //Test1 Arrange
        var trimmedPrefix1T1 = s1.Substring(3);
        var trimmedPrefix2T1 = s2.Substring(3);

        SetupMocks(
            s1,
            s2,
            trimmedPrefix1T1,
            trimmedPrefix2T1,
            trimmedPrefix1T1,
            trimmedPrefix2T1,
            3,
            expectedSimilarity,
            true
        );

        //Test2 Arrange
        SetupMocks(
            s1,
            s2,
            s1.Substring(1),
            s2.Substring(1),
            "bcdefg",
            "123456",
            6,
            expectedSimilarity,
            true
        );

        var result = _comparer.Compare(s1, s2);

        Assert.AreEqual(expectedSimilarity, result);
    }

    private void SetupMocks(
        string s1Processed,
        string s2Processed,
        string trimmedPrefix1,
        string trimmedPrefix2,
        string trimmedSuffix1,
        string trimmedSuffix2,
        int levenshteinDistance,
        int percentSimilarity,
        bool caseSensitive
    )
    {
        if (caseSensitive)
        {
            _stringPreprocessorMock
                .Setup(x => x.Process(It.IsAny<string>()))
                .Returns<string>(s => s);
        }
        else
        {
            _stringPreprocessorMock
                .Setup(x => x.Process(It.IsAny<string>()))
                .Returns<string>(s => s.ToLowerInvariant());
        }
        _stringManipulatorMock
            .Setup(x => x.TrimPrefix(s1Processed, s2Processed))
            .Returns((trimmedPrefix1, trimmedPrefix2));
        _stringManipulatorMock
            .Setup(x => x.TrimSuffix(trimmedPrefix1, trimmedPrefix2))
            .Returns((trimmedSuffix1, trimmedSuffix2));
        _levenshteinCalculatorMock
            .Setup(x => x.Calculate(trimmedSuffix1, trimmedSuffix2))
            .Returns(levenshteinDistance);
        _similarityCalculatorMock
            .Setup(x =>
                x.CalculatePercentSimilarity(
                    Math.Max(s1Processed.Length, s2Processed.Length),
                    levenshteinDistance
                )
            )
            .Returns(percentSimilarity);
    }
}
