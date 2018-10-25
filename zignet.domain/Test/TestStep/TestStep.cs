using System.Collections.Generic;

namespace ZigNet.Domain.Test.TestStep
{
    public class TestStep
    {
        public int TestStepID { get; set; }
        public string Name { get; set; }
        public ICollection<TestStepResult> TestStepResults { get; set; }
        public ICollection<Test> Tests { get; set; }
    }
}
