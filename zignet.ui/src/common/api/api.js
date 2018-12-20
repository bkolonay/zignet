function url() {
	return process.env.REACT_APP_API_BASE_URL + 'api/';
}

function getSuiteResults(filter) {
	if (filter.debug)
	  return _get(url() + 'suite/latest?debug=true');
  else if (filter.showLoopNet)
    return _get(url() + 'suite/latest?applications=LoopNet');
	else
	  return _get(url() + 'suite/latest');
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
