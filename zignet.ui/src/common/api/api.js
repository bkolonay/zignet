function url() {
	return process.env.REACT_APP_API_BASE_URL + 'api/';
}

function getSuiteResults(filter) {
  let localUrl = url() + 'suite/latest?';

  if (filter.debug)
    localUrl = localUrl + 'debug=true';
  else if (filter.showLoopNet && filter.showLmMobile)
    localUrl = localUrl + 'applications=LoopNet&applications=Listing+Manager+Mobile';
  else if (filter.showLoopNet)
    localUrl = localUrl + 'applications=LoopNet';
  else if (filter.showLmMobile)
    localUrl = localUrl + 'applications=Listing+Manager+Mobile';

  return _get(localUrl);
}

function getTestResults(debug) {
  if (debug)
    return _get(url() + 'testResult/latest?debug=true');
  else
    return _get(url() + 'testResult/latest');
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
