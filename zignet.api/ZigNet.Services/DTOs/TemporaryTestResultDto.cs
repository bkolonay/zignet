using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigNet.Services.DTOs
{
    public class TemporaryTestResultDto
    {
        public int TestResultId { get; set; }
        public int SuiteResultId { get; set; }
        public int SuiteId { get; set; }
        public int TestResultTypeId { get; set; }
    }
}
