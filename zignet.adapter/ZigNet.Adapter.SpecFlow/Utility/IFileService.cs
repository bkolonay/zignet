using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigNet.Adapter.SpecFlow.Utility
{
    public interface IFileService
    {
        void WriteStringToFile(string filePath, string str);
        string ReadStringFromFile(string filePath);
    }
}
