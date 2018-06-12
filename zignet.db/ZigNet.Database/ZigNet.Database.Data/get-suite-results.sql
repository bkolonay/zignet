SELECT SuiteResults.SuiteResultID, SuiteResults.SuiteId, Suites.SuiteName,
	SuiteResults.SuiteResultStartDateTime AS StartTime, SuiteResults.SuiteResultEndDateTime AS EndTime,
	SuiteResultTypes.SuiteResultTypeName
FROM SuiteResults
JOIN Suites
	ON SuiteResults.SuiteId = Suites.SuiteID
JOIN SuiteResultTypes
	ON SuiteResults.SuiteResultTypeId = SuiteResultTypes.SuiteResultTypeID
WHERE SuiteResults.SuiteId = 47