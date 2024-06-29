namespace HomeWork.Utilities;

public class CaseInsensitivePreprocessor : IStringPreprocessor
{
    public string Process(string input)
    {
        return input?.ToLowerInvariant();
    }
}