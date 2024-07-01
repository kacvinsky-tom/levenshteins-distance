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
        _stringManipulatorMock
            .Setup(x => x.TrimPrefix(s1, s2))
            .Returns((string.Empty, string.Empty));
        _stringManipulatorMock
            .Setup(x => x.TrimSuffix(s1, s2))
            .Returns((string.Empty, string.Empty));
        _levenshteinCalculatorMock.Setup(x => x.Calculate(s1, s2)).Returns(0);
        _similarityCalculatorMock
            .Setup(x => x.CalculatePercentSimilarity(s1.Length, 0))
            .Returns(100);
        _stringPreprocessorMock.Setup(x => x.Process(s1)).Returns(s1);
        _stringPreprocessorMock.Setup(x => x.Process(s2)).Returns(s2);

        var result = _comparer.Compare(s1, s2);

        Assert.AreEqual(100, result);
    }

    [TestMethod]
    [DataRow("abcde", "vwxyz")]
    [DataRow("12345", "6789")]
    public void Compare_CompletelyDifferentStrings_Returns0PercentSimilarity(string s1, string s2)
    {
        _stringManipulatorMock.Setup(x => x.TrimPrefix(s1, s2)).Returns((s1, s2));
        _stringManipulatorMock.Setup(x => x.TrimSuffix(s1, s2)).Returns((s1, s2));
        _levenshteinCalculatorMock.Setup(x => x.Calculate(s1, s2)).Returns(5);
        _stringPreprocessorMock.Setup(x => x.Process(s1)).Returns(s1);
        _stringPreprocessorMock.Setup(x => x.Process(s2)).Returns(s2);

        var result = _comparer.Compare(s1, s2);

        Assert.AreEqual(0, result);
    }

    [TestMethod]
    [DataRow("", "")]
    public void Compare_EmptyStrings_Returns100PercentSimilarity(string s1, string s2)
    {
        _stringManipulatorMock.Setup(x => x.TrimPrefix(s1, s2)).Returns((s1, s2));
        _stringManipulatorMock.Setup(x => x.TrimSuffix(s1, s2)).Returns((s1, s2));
        _levenshteinCalculatorMock.Setup(x => x.Calculate(s1, "")).Returns(0);
        _similarityCalculatorMock.Setup(x => x.CalculatePercentSimilarity(0, 0)).Returns(100);
        _stringPreprocessorMock.Setup(x => x.Process(s1)).Returns(s1);
        _stringPreprocessorMock.Setup(x => x.Process(s2)).Returns(s2);

        var result = _comparer.Compare(s1, s2);

        Assert.AreEqual(100, result);
    }

    [TestMethod]
    [DataRow("", "second")]
    [DataRow("first", "")]
    public void Compare_EmptyAndNonEmptyString_Returns0PercentSimilarity(string s1, string s2)
    {
        _stringManipulatorMock.Setup(x => x.TrimPrefix(s1, s2)).Returns((s1, s2));
        _stringManipulatorMock.Setup(x => x.TrimSuffix(s1, s2)).Returns((s1, s2));
        _levenshteinCalculatorMock.Setup(x => x.Calculate("", "second")).Returns(6);
        _levenshteinCalculatorMock.Setup(x => x.Calculate("first", "")).Returns(5);
        _stringPreprocessorMock.Setup(x => x.Process(s1)).Returns(s1);
        _stringPreprocessorMock.Setup(x => x.Process(s2)).Returns(s2);

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
        var s1Lower = s1.ToLowerInvariant();
        var s2Lower = s2.ToLowerInvariant();

        _stringPreprocessorMock.Setup(x => x.Process(s1)).Returns(s1Lower);
        _stringPreprocessorMock.Setup(x => x.Process(s2)).Returns(s2Lower);
        _stringManipulatorMock
            .Setup(x => x.TrimPrefix(s1Lower, s2Lower))
            .Returns((string.Empty, string.Empty));
        _similarityCalculatorMock
            .Setup(x => x.CalculatePercentSimilarity(s1.Length, 0))
            .Returns(100);

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
        var result = _comparer.Compare(s1, s2, 50);

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
        var s1LowerT1 = s1.ToLowerInvariant();
        var s2LowerT1 = s2.ToLowerInvariant();

        var trimmedPrefix1T1 = s1LowerT1.Substring(3);
        var trimmedPrefix2T1 = s2LowerT1.Substring(3);
        var trimmedSuffix1T1 = trimmedPrefix1T1;
        var trimmedSuffix2T1 = trimmedPrefix2T1;
        var levenshteinDistanceT1 = 3;

        SetupMocks(
            s1LowerT1,
            s2LowerT1,
            trimmedPrefix1T1,
            trimmedPrefix2T1,
            trimmedSuffix1T1,
            trimmedSuffix2T1,
            levenshteinDistanceT1,
            expectedSimilarity
        );

        //Test2 Arrange
        var s1LowerT2 = s1.ToLowerInvariant();
        var s2LowerT2 = s2.ToLowerInvariant();

        var trimmedPrefix1T2 = s1LowerT2.Substring(1);
        var trimmedPrefix2T2 = s2LowerT2.Substring(1);
        var trimmedSuffix1T2 = "bcdefg";
        var trimmedSuffix2T2 = "123456";
        var levenshteinDistanceT2 = 6;

        SetupMocks(
            s1LowerT2,
            s2LowerT2,
            trimmedPrefix1T2,
            trimmedPrefix2T2,
            trimmedSuffix1T2,
            trimmedSuffix2T2,
            levenshteinDistanceT2,
            expectedSimilarity
        );

        var result = _comparer.Compare(s1, s2);

        Assert.AreEqual(expectedSimilarity, result);
    }

    private void SetupMocks(
        string s1Lower,
        string s2Lower,
        string trimmedPrefix1,
        string trimmedPrefix2,
        string trimmedSuffix1,
        string trimmedSuffix2,
        int levenshteinDistance,
        int expectedSimilarity
    )
    {
        _stringPreprocessorMock
            .Setup(x => x.Process(It.IsAny<string>()))
            .Returns<string>(s => s.ToLowerInvariant());
        _stringManipulatorMock
            .Setup(x => x.TrimPrefix(s1Lower, s2Lower))
            .Returns((trimmedPrefix1, trimmedPrefix2));
        _stringManipulatorMock
            .Setup(x => x.TrimSuffix(trimmedPrefix1, trimmedPrefix2))
            .Returns((trimmedSuffix1, trimmedSuffix2));
        _levenshteinCalculatorMock
            .Setup(x => x.Calculate(trimmedSuffix1, trimmedSuffix2))
            .Returns(levenshteinDistance);
        _similarityCalculatorMock
            .Setup(x => x.CalculatePercentSimilarity(s1Lower.Length, levenshteinDistance))
            .Returns(expectedSimilarity);
    }
}
