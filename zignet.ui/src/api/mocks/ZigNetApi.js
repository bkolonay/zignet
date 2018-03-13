class ZigNetApi {
  constructor(apiBaseUrl) {
    this.apiBaseUrl = apiBaseUrl;
  }

  getLatestTestResultsForSuite(suiteId) {
    return Promise.resolve([
      {
        'TestResultID':1,
        'TestName':'UI: Search for brokers',
        'FailingFromDate':'2018-03-11T15:05:00',
        'PassingFromDate':''
      },
      {
        'TestResultID':2,
        'TestName':'UI: Search for listings',
        'FailingFromDate':'2018-03-12T15:05:00',
        'PassingFromDate':''
      },
      {
        'TestResultID':3,
        'TestName':'Service: Get listing detail',
        'FailingFromDate':'',
        'PassingFromDate':'2018-02-10T15:05:00'
      },
      {
        'TestResultID':4,
        'TestName':'Service: Get typeahead',
        'FailingFromDate':'',
        'PassingFromDate':'2017-01-01T15:05:00'
      },      
    ]);    
  }

  getLatestSuiteResults() {
    return Promise.resolve([
      {
        'SuiteID':1,
        'SuiteName':'LN Services - DVM',
        'TotalPassedTests':205,
        'TotalFailedTests':7,
        'TotalInconclusiveTests':0,
        'SuiteEndTime':'2018-03-12T15:05:00'
      },
      {
        'SuiteID':2,
        'SuiteName':'LN Services - DVM (D)',
        'TotalPassedTests':17,
        'TotalFailedTests':12,
        'TotalInconclusiveTests':0,
        'SuiteEndTime':'2018-02-28T15:05:00'
      },
      {
        'SuiteID':3,
        'SuiteName':'LN Services - TSM',
        'TotalPassedTests':209,
        'TotalFailedTests':3,
        'TotalInconclusiveTests':0,
        'SuiteEndTime':'2018-03-13T15:05:00'
      },
      {
        'SuiteID':4,
        'SuiteName':'LN Services - TSM (D)',
        'TotalPassedTests':3,
        'TotalFailedTests':1,
        'TotalInconclusiveTests':0,
        'SuiteEndTime':'2018-03-13T15:05:00'
      }      
    ]);
  }
}

export default ZigNetApi;
