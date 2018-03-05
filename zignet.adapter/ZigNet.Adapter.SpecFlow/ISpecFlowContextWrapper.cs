using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigNet.Adapter.SpecFlow
{
    public interface ISpecFlowContextWrapper
    {
        string[] GetScenarioAndFeatureTags();
        string GetScenarioTitle();
        Exception GetScenarioTestError();
    }
}
