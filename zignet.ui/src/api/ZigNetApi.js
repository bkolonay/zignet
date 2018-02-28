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
        }        
    ]  	
  }
}

export default ZigNetApi;
