SELECT SuiteResults.SuiteResultID, SuiteResults.SuiteId, TestResults.TestResultID, TestResults.TestId, TestResults.TestResultTypeId AS ResultTypeId, Tests.TestName,
TestResults.TestResultEndDateTime, DATEDIFF(second, TestResults.TestResultStartDateTime, TestResults.TestResultEndDateTime) AS Seconds
	FROM SuiteResults
JOIN TestResults
	ON TestResults.SuiteResultId = SuiteResults.SuiteResultID
JOIN Tests
	ON TestResults.TestId = Tests.TestID
WHERE SuiteResults.SuiteId = 25
	AND TestResults.TestResultEndDateTime > '3/31/2018'
	--AND Tests.TestName != 'Return a FS saved listing search metadata for the mobile user'
ORDER BY Seconds DESC, TestName