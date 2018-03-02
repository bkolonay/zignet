class ZigNetApi {
  constructor(apiBaseUrl) {
    this.apiBaseUrl = apiBaseUrl;
  }

  getLatestSuiteResults() {
    return this._get(this.apiBaseUrl + 'latestSuiteResults');
  }

  _get(url) {
    return fetch(url)
      .then(function(response) {
        if (response.ok)
          return response.json();
        throw new Error(`URL ${url} returned status code ${response.status}`);
      });
  }  
}

export default ZigNetApi;
