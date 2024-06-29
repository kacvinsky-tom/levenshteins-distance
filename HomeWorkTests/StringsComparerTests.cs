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

    public StringsComparerTests()
    {
        _levenshteinCalculatorMock = new Mock<ILevenshteinDistance>();
        _stringManipulatorMock = new Mock<IStringManipulator>();
        _comparer = new StringsComparer(_levenshteinCalculatorMock.Object, _stringManipulatorMock.Object);
    }

    [TestMethod]
    [DataRow("testing", "testing")]
    [DataRow("this is the same string", "this is the same string")]
    public void Compare_IdenticalStrings_Returns100PercentSimilarity(string first, string second)
    {
        _stringManipulatorMock.Setup(x => x.TrimPrefix(first, second)).Returns((string.Empty, string.Empty));
        _stringManipulatorMock.Setup(x => x.TrimSuffix(first, second)).Returns((string.Empty, string.Empty));
        _levenshteinCalculatorMock.Setup(x => x.Calculate(first, second)).Returns(0);

        var result = _comparer.Compare(first, second);

        Assert.AreEqual(100, result);
    }

    [TestMethod]
    [DataRow("abcde", "vwxyz")]
    [DataRow("12345", "6789")]
    public void Compare_CompletelyDifferentStrings_Returns0PercentSimilarity(string first, string second)
    {
        _stringManipulatorMock.Setup(x => x.TrimPrefix(first, second)).Returns((first, second));
        _stringManipulatorMock.Setup(x => x.TrimSuffix(first, second)).Returns((first, second));
        _levenshteinCalculatorMock.Setup(x => x.Calculate(first, second)).Returns(5);

        var result = _comparer.Compare(first, second);

        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Compare_EmptyStrings_Returns100PercentSimilarity()
    {
        _stringManipulatorMock.Setup(x => x.TrimPrefix("", "")).Returns(("", ""));
        _stringManipulatorMock.Setup(x => x.TrimSuffix("", "")).Returns(("", ""));
        _levenshteinCalculatorMock.Setup(x => x.Calculate("", "")).Returns(0);

        var result = _comparer.Compare("", "");

        Assert.AreEqual(100, result);
    }

    [TestMethod]
    [DataRow("", "second")]
    [DataRow("first", "")]
    public void Compare_EmptyAndNonEmptyString_Returns0PercentSimilarity(string first, string second)
    {
        _stringManipulatorMock.Setup(x => x.TrimPrefix(first, second)).Returns((first, second));
        _stringManipulatorMock.Setup(x => x.TrimSuffix(first, second)).Returns((first, second));
        _levenshteinCalculatorMock.Setup(x => x.Calculate("", "second")).Returns(6);
        _levenshteinCalculatorMock.Setup(x => x.Calculate("first", "")).Returns(5);

        var result = _comparer.Compare(first, second);

        Assert.AreEqual(0, result);
    }
}
