namespace ZigNet.Adapter.SpecFlow.Utility
{
    public interface IFileService
    {
        void WriteStringToFile(string filePath, string str);
        string ReadStringFromFile(string filePath);
    }
}
