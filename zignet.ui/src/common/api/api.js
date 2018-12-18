function url() {
	return process.env.REACT_APP_API_BASE_URL + 'api/';
}

function getSuiteResults(debug) {
	if (debug)
	  return _get(url() + 'suite/latest?debug=true');
	else
	  return _get(url() + 'suite/latest');
}

function getTestResults(suiteId, grouped) {
  return _post(url() + 'testResult/latest?group=true', 25);

  if (grouped)
    return _post(url() + 'testResult/latest?group=true', suiteId);
  else
    return _post(url() + 'testResult/latest', suiteId);
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
