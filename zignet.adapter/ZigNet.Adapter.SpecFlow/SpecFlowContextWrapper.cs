using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace ZigNet.Adapter.SpecFlow
{
    public class SpecFlowContextWrapper : ISpecFlowContextWrapper
    {
        public string[] GetScenarioAndFeatureTags()
        {
            var testCategories = new List<string>();
            foreach (var tag in ScenarioContext.Current.ScenarioInfo.Tags)
                testCategories.Add(tag.ToString());
            foreach (var tag in FeatureContext.Current.FeatureInfo.Tags)
                testCategories.Add(tag.ToString());
            return testCategories.ToArray();
        }

        public string GetScenarioTitle()
        {
            return ScenarioContext.Current.ScenarioInfo.Title;
        }

        public Exception GetScenarioTestError()
        {
            return ScenarioContext.Current.TestError;
        }

        public string GetTestStepName()
        {
            return ScenarioStepContext.Current.StepInfo.Text;
        }
    }
}
