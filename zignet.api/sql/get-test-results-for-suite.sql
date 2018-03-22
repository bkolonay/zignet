SELECT SuiteResults.SuiteResultID, SuiteResults.SuiteId, TestResults.TestResultID, TestResults.TestId, TestResults.TestResultTypeId
	FROM SuiteResults
JOIN TestResults
	ON TestResults.SuiteResultId = SuiteResults.SuiteResultID
WHERE SuiteResults.SuiteId = 26


SELECT TestResults.TestResultTypeId, COUNT(TestResults.TestResultTypeId) as RecordCount
	FROM SuiteResults
JOIN TestResults
	ON TestResults.SuiteResultId = SuiteResults.SuiteResultID
WHERE SuiteResults.SuiteId = 26
GROUP BY TestResults.TestResultTypeId