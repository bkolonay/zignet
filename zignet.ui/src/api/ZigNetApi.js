class ZigNetApi {
  getLatestSuiteResults() {
    return [
        {
          suiteName: 'Suite 01',
          suiteResultId: 1,
          totalPassedTests: 100,
          totalFailedTests: 25
        },
        {
          suiteName: 'Suite 02',
          suiteResultId: 2,
          totalPassedTests: 10,
          totalFailedTests: 120
        },
        {
          suiteName: 'Suite 03',
          suiteResultId: 3,
          totalPassedTests: 10,
          totalFailedTests: 120
        },
        {
          suiteName: 'Suite 04',
          suiteResultId: 4,
          totalPassedTests: 10,
          totalFailedTests: 120
        },
        {
          suiteName: 'Suite 05',
          suiteResultId: 5,
          totalPassedTests: 10,
          totalFailedTests: 120
        }        
    ]  	
  }
}

export default ZigNetApi;
