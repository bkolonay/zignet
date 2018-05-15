class ZigNetApi {
  constructor(apiBaseUrl) {
    this.apiBaseUrl = apiBaseUrl;
  }

  getLatestSuiteResults(suiteResultsGrouped) {
    if (suiteResultsGrouped)
      return this._get(this.apiBaseUrl + 'latestSuiteResults?group=true');
    else
      return this._get(this.apiBaseUrl + 'latestSuiteResults');
  }

  getLatestTestResultsForSuite(suiteId, testResultsGrouped) {
    if (testResultsGrouped)
      return this._post(this.apiBaseUrl + 'suite/latestTestResults?group=true', suiteId);
    else
      return this._post(this.apiBaseUrl + 'suite/latestTestResults', suiteId);
  }  

  _get(url) {
    return fetch(url)
      .then(function(response) {
        if (response.ok)
          return response.json();
        throw new Error(`URL ${url} returned status code ${response.status}`);
      });
  }

  _post(url, requestBody) {
    return fetch(url, {
      headers: { 'content-type': 'application/json' },
      body: requestBody,
      method: 'POST'
    })
    .then(function(response) {
      if (response.ok)
        return response.json();
      throw new Error(`URL ${url} returned status code ${response.status}`);
    });
  }  
}

export default ZigNetApi;
