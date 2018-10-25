using System;

namespace ZigNet.Adapter.SpecFlow
{
    public interface ISpecFlowContextWrapper
    {
        string[] GetScenarioAndFeatureTags();
        string GetScenarioTitle();
        string GetTestStepName();
        Exception GetScenarioTestError();
    }
}
