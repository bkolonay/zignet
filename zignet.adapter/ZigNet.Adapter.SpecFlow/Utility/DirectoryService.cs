using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZigNet.Adapter.SpecFlow.Utility
{
    public class DirectoryService : IDirectoryService
    {
        public string GetExecutingDirectory()
        {
            return Path.GetDirectoryName(
                new Uri(
                    Assembly.GetExecutingAssembly().GetName().CodeBase
                ).LocalPath
            );
        }
    }
}
