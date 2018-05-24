function url() {
	return process.env.REACT_APP_API_BASE_URL + 'api/';
}

function getSuiteResults(grouped, debug) {
	if (grouped && debug)
	  return _get(url() + 'latestSuiteResults?group=true&debug=true');
	else if (grouped)
	  return _get(url() + 'latestSuiteResults?group=true');
	else if (debug)
	  return _get(url() + 'latestSuiteResults?debug=true');
	else
	  return _get(url() + 'latestSuiteResults');
}

function getTestResults(suiteId, grouped) {
  if (grouped)
    return _post(url() + 'suite/latestTestResults?group=true', suiteId);
  else
    return _post(url() + 'suite/latestTestResults', suiteId);	
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
