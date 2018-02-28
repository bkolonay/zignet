using System.Collections.Generic;

namespace ZigNet.Domain.Suite
{
    public class SuiteCategory
    {
        public int SuiteCategoryID { get; set; }
        public string Name { get; set; }
        public ICollection<Suite> Suites { get; set; }
    }
}
