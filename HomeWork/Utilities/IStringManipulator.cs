namespace HomeWork.Utilities;

public interface IStringManipulator
{
    public (string, string) TrimPrefix(string first, string second);
    public (string, string) TrimSuffix(string first, string second);
}
