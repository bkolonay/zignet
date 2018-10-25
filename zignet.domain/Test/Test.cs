using System.Collections.Generic;

namespace ZigNet.Domain.Test
{
    public class Test
    {
        public int TestID { get; set; }
        public string Name { get; set; }
        public ICollection<TestCategory> Categories { get; set; }
        public ICollection<TestResult> TestResults { get; set; }
        public ICollection<Suite.Suite> Suites { get; set; }
        public ICollection<TestStep.TestStep> TestSteps { get; set; }
    }
}
