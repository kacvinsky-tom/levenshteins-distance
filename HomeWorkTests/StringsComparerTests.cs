using HomeWork.Comparers;
using HomeWork.Levenshtein;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HomeWorkTests;

[TestClass]
public class StringsComparerTests
{
    private StringsComparer _comparer;
    private Mock<ILevenshteinDistance> _levenshteinCalculatorMock;
    
    public StringsComparerTests()
    {
        _levenshteinCalculatorMock = new Mock<ILevenshteinDistance>();
        _comparer = new StringsComparer(_levenshteinCalculatorMock.Object);
    }

    [TestMethod]
    public void Compare_IdenticalStrings_Returns100PercentSimilarity()
    {
        _levenshteinCalculatorMock.Setup(x => x.Calculate("testing", "testing")).Returns(0); 
        
        var result = _comparer.Compare("testing", "testing");
        
        Assert.AreEqual(100, result);
    }

    [TestMethod]
    public void Compare_CompletelyDifferentStrings_Returns0PercentSimilarity()
    {
        _levenshteinCalculatorMock.Setup(x => x.Calculate("abcde", "vwxyz")).Returns(5); 

        var result = _comparer.Compare("abcde", "vwxyz");
        
        Assert.AreEqual(0, result);
    }
    
    [TestMethod]
    public void Compare_EmptyStrings_Returns100PercentSimilarity()
    {
        _levenshteinCalculatorMock.Setup(x => x.Calculate("", "")).Returns(0); 

        var result = _comparer.Compare("", "");
        
        Assert.AreEqual(100, result);
    }

    [TestMethod]
    public void Compare_EmptyAndNonEmptyString_Returns0PercentSimilarity()
    {
        _levenshteinCalculatorMock.Setup(x => x.Calculate("", "testing")).Returns(7); 

        var result = _comparer.Compare("", "testing");
        
        Assert.AreEqual(0, result);
    }
}