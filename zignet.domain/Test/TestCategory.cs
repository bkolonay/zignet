using System.Collections.Generic;

namespace ZigNet.Domain.Test
{
    public class TestCategory
    {
        public int TestCategoryID { get; set; }
        public string Name { get; set; }
        public ICollection<Test> Tests { get; set; }
    }
}
