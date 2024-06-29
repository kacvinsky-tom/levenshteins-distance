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
    private readonly Mock<ISimilarityCalculator> _similiarityCalculatorMock;
    private readonly Mock<IStringPreprocessor> _stringPreprocessorMock;

    public StringsComparerTests()
    {
        _levenshteinCalculatorMock = new Mock<ILevenshteinDistance>();
        _stringManipulatorMock = new Mock<IStringManipulator>();
        _similiarityCalculatorMock = new Mock<ISimilarityCalculator>();
        _stringPreprocessorMock = new Mock<IStringPreprocessor>();

        _comparer = new StringsComparer(
            _levenshteinCalculatorMock.Object,
            _stringManipulatorMock.Object,
            _similiarityCalculatorMock.Object,
            _stringPreprocessorMock.Object
        );
    }

    [TestMethod]
    [DataRow("testing", "testing")]
    [DataRow("this is the same string", "this is the same string")]
    public void Compare_IdenticalStrings_Returns100PercentSimilarity(string first, string second)
    {
        _stringManipulatorMock
            .Setup(x => x.TrimPrefix(first, second))
            .Returns((string.Empty, string.Empty));
        _stringManipulatorMock
            .Setup(x => x.TrimSuffix(first, second))
            .Returns((string.Empty, string.Empty));
        _levenshteinCalculatorMock.Setup(x => x.Calculate(first, second)).Returns(0);
        _similiarityCalculatorMock
            .Setup(x => x.CalculatePercentSimilarity(first.Length, 0))
            .Returns(100);
        _stringPreprocessorMock.Setup(x => x.Process(first)).Returns(first);
        _stringPreprocessorMock.Setup(x => x.Process(second)).Returns(second);

        var result = _comparer.Compare(first, second);

        Assert.AreEqual(100, result);
    }

    [TestMethod]
    [DataRow("abcde", "vwxyz")]
    [DataRow("12345", "6789")]
    public void Compare_CompletelyDifferentStrings_Returns0PercentSimilarity(
        string first,
        string second
    )
    {
        _stringManipulatorMock.Setup(x => x.TrimPrefix(first, second)).Returns((first, second));
        _stringManipulatorMock.Setup(x => x.TrimSuffix(first, second)).Returns((first, second));
        _levenshteinCalculatorMock.Setup(x => x.Calculate(first, second)).Returns(5);
        _stringPreprocessorMock.Setup(x => x.Process(first)).Returns(first);
        _stringPreprocessorMock.Setup(x => x.Process(second)).Returns(second);

        var result = _comparer.Compare(first, second);

        Assert.AreEqual(0, result);
    }

    [TestMethod]
    [DataRow("", "")]
    public void Compare_EmptyStrings_Returns100PercentSimilarity(string first, string second)
    {
        _stringManipulatorMock.Setup(x => x.TrimPrefix(first, second)).Returns((first, second));
        _stringManipulatorMock.Setup(x => x.TrimSuffix(first, second)).Returns((first, second));
        _levenshteinCalculatorMock.Setup(x => x.Calculate(first, "")).Returns(0);
        _similiarityCalculatorMock.Setup(x => x.CalculatePercentSimilarity(0, 0)).Returns(100);
        _stringPreprocessorMock.Setup(x => x.Process(first)).Returns(first);
        _stringPreprocessorMock.Setup(x => x.Process(second)).Returns(second);

        var result = _comparer.Compare(first, second);

        Assert.AreEqual(100, result);
    }

    [TestMethod]
    [DataRow("", "second")]
    [DataRow("first", "")]
    public void Compare_EmptyAndNonEmptyString_Returns0PercentSimilarity(
        string first,
        string second
    )
    {
        _stringManipulatorMock.Setup(x => x.TrimPrefix(first, second)).Returns((first, second));
        _stringManipulatorMock.Setup(x => x.TrimSuffix(first, second)).Returns((first, second));
        _levenshteinCalculatorMock.Setup(x => x.Calculate("", "second")).Returns(6);
        _levenshteinCalculatorMock.Setup(x => x.Calculate("first", "")).Returns(5);
        _stringPreprocessorMock.Setup(x => x.Process(first)).Returns(first);
        _stringPreprocessorMock.Setup(x => x.Process(second)).Returns(second);

        var result = _comparer.Compare(first, second);

        Assert.AreEqual(0, result);
    }

    [TestMethod]
    [DataRow("THIS IS THE SAME STRING", "this is the same string")]
    [DataRow("also This is The same String", "alsO thiS is the samE strIng")]
    public void Compare_CaseInsensitiveIdenticalStrings_ReturnsCorrectSimilarity(
        string first,
        string second
    )
    {
        _stringPreprocessorMock.Setup(x => x.Process(first)).Returns(first.ToLowerInvariant());
        _stringPreprocessorMock.Setup(x => x.Process(second)).Returns(second.ToLowerInvariant());
        _stringManipulatorMock
            .Setup(x => x.TrimPrefix(first.ToLowerInvariant(), second.ToLowerInvariant()))
            .Returns((string.Empty, string.Empty));
        _similiarityCalculatorMock
            .Setup(x => x.CalculatePercentSimilarity(first.Length, 0))
            .Returns(100);

        var result = _comparer.Compare(first, second);

        Assert.AreEqual(100, result);
    }
}
