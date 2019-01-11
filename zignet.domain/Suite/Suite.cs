using System.Collections.Generic;

namespace ZigNet.Domain.Suite
{
    public class Suite
    {
        public int SuiteID { get; set; }
        public string Name { get; set; }
        public Application Application { get; set; }
        public Environment Environment { get; set; }
        public ICollection<SuiteCategory> Categories { get; set; }
        public ICollection<SuiteResult> SuiteResults { get; set; }
        public ICollection<Test.Test> Tests { get; set; }
    }
}
