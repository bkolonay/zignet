import { getUrlParams } from '../routing/SearchFilter.js'

function url() {
	return process.env.REACT_APP_API_BASE_URL + 'api/';
}

function getSuiteResults(filter) {
  return _get(url() + 'suite/latest?' + getUrlParams(filter));
}

function getTestResults(filter) {
  return _get(url() + 'testResult/latest?' + getUrlParams(filter));
}

function _get(url) {
  return fetch(url)
    .then(function(response) {
      if (response.ok)
        return response.json();
      throw new Error(`URL ${url} returned status code ${response.status}`);
    });
}

function _post(url, requestBody) {
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

export { getSuiteResults, getTestResults };
