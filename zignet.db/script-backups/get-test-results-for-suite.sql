SELECT SuiteResults.SuiteResultID, SuiteResults.SuiteId, TestResults.TestResultID, TestResults.TestId, TestResults.TestResultTypeId
	FROM SuiteResults
JOIN TestResults
	ON TestResults.SuiteResultId = SuiteResults.SuiteResultID
JOIN Tests
	ON TestResults.TestId = Tests.TestID
WHERE SuiteResults.SuiteId = 26
ORDER BY TestId ASC

SELECT TestResults.TestResultTypeId, COUNT(TestResults.TestResultTypeId) as RecordCount
	FROM SuiteResults
JOIN TestResults
	ON TestResults.SuiteResultId = SuiteResults.SuiteResultID
WHERE SuiteResults.SuiteId = 26
GROUP BY TestResults.TestResultTypeId