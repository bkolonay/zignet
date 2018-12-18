namespace ZigNet.Services
{
    // todo: move to ZigNet.Domain package
    // todo: maybe split into SuiteResults... and TestResults
    public class SuiteResultsFilter
    {
        public string[] Applications { get; set; }
        public bool Debug { get; set; }
    }
}
