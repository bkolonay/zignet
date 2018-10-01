using System.IO;

namespace ZigNet.Adapter.SpecFlow.Utility
{
    public class FileService : IFileService
    {
        public void WriteStringToFile(string filePath, string str)
        {
            using (StreamWriter streamWriter = new StreamWriter(filePath))
                streamWriter.Write(str);
        }

        public string ReadStringFromFile(string filePath)
        {
            using (StreamReader streamReader = new StreamReader(filePath))
                return streamReader.ReadToEnd();
        }
    }
}
