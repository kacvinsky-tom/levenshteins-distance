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
    public void Compare_CompletelyDifferentStrings_Returns0PercentSimilarity(
        string s1,
        string s2
    )
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
    public void Compare_EmptyAndNonEmptyString_Returns0PercentSimilarity(
        string s1,
        string s2
    )
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
        _stringPreprocessorMock.Setup(x => x.Process(s1)).Returns(s1.ToLowerInvariant());
        _stringPreprocessorMock.Setup(x => x.Process(s2)).Returns(s2.ToLowerInvariant());
        _stringManipulatorMock
            .Setup(x => x.TrimPrefix(s1.ToLowerInvariant(), s2.ToLowerInvariant()))
            .Returns((string.Empty, string.Empty));
        _similarityCalculatorMock
            .Setup(x => x.CalculatePercentSimilarity(s1.Length, 0))
            .Returns(100);

        var result = _comparer.Compare(s1, s2);

        Assert.AreEqual(100, result);
    }
}
