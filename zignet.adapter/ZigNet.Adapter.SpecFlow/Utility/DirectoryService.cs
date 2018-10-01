using System;
using System.IO;
using System.Reflection;

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
