using HomeWork.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HomeWorkTests;

[TestClass]
public class StringManipulatorTests
{
    private readonly StringManipulator _stringManipulator = new();

    [TestMethod]
    [DataRow("testing", "testing")]
    [DataRow("this is the same string", "this is the same string")]
    public void TrimPrefix_IdenticalStrings_ReturnsEmptyStrings(string s1, string s2)
    {
        var result = _stringManipulator.TrimPrefix(s1, s2);

        Assert.IsTrue(string.IsNullOrEmpty(result.Item1));
        Assert.IsTrue(string.IsNullOrEmpty(result.Item2));
    }

    [TestMethod]
    [DataRow("testing", "testing")]
    [DataRow("this is the same string", "this is the same string")]
    public void TrimSuffix_IdenticalStrings_ReturnsEmptyStrings(string s1, string s2)
    {
        var result = _stringManipulator.TrimSuffix(s1, s2);

        Assert.IsTrue(string.IsNullOrEmpty(result.Item1));
        Assert.IsTrue(string.IsNullOrEmpty(result.Item2));
    }

    [TestMethod]
    [DataRow("hello world", "hello")]
    [DataRow("prefixString", "prefix")]
    public void TrimPrefix_DifferentStrings_ReturnsSuffix(string s1, string s2)
    {
        var result = _stringManipulator.TrimPrefix(s1, s2);

        Assert.IsFalse(string.IsNullOrEmpty(result.Item1));
        Assert.IsTrue(string.IsNullOrEmpty(result.Item2));
    }

    [TestMethod]
    [DataRow("world hello", "hello")]
    [DataRow("StringSuffix", "Suffix")]
    public void TrimSuffix_DifferentStrings_ReturnsPrefix(string s1, string s2)
    {
        var result = _stringManipulator.TrimSuffix(s1, s2);

        Assert.IsFalse(string.IsNullOrEmpty(result.Item1));
        Assert.IsTrue(string.IsNullOrEmpty(result.Item2));
    }

    [TestMethod]
    [DataRow("unrelated", "string")]
    [DataRow("different", "words")]
    public void TrimPrefix_UnrelatedStrings_ReturnsOriginalStrings(string s1, string s2)
    {
        var result = _stringManipulator.TrimPrefix(s1, s2);

        Assert.AreEqual(s1, result.Item1);
        Assert.AreEqual(s2, result.Item2);
    }

    [TestMethod]
    [DataRow("unrelated", "string")]
    [DataRow("different", "words")]
    public void TrimSuffix_UnrelatedStrings_ReturnsOriginalStrings(string s1, string s2)
    {
        var result = _stringManipulator.TrimSuffix(s1, s2);

        Assert.AreEqual(s1, result.Item1);
        Assert.AreEqual(s2, result.Item2);
    }

    [TestMethod]
    [DataRow("same prefix different suffix", "same prefix but different suffix")]
    public void TrimPrefix_DifferentStrings_ReturnsTrimmedStrings(string s1, string s2)
    {
        var result = _stringManipulator.TrimPrefix(s1, s2);

        Assert.AreEqual("different suffix", result.Item1);
        Assert.AreEqual("but different suffix", result.Item2);
    }

    [TestMethod]
    [DataRow("different prefix same suffix", "different prefix but same suffix")]
    public void TrimSuffix_DifferentStrings_ReturnsTrimmedStrings(string s1, string s2)
    {
        var result = _stringManipulator.TrimSuffix(s1, s2);

        Assert.AreEqual("different prefix", result.Item1);
        Assert.AreEqual("different prefix but", result.Item2);
    }
}
