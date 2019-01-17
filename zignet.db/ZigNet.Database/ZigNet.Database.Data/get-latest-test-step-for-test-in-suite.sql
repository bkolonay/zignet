USE ZigNet

-- option 1 (use suite, test, and test step to get all test step results)
SELECT TestStepResults.TestStepResultID, TestStepResults.TestStepId, TestStepResults.TestResultId,
	TestStepResults.TestStepResultStartDateTime, TestStepResults.TestStepResultEndDateTime,
	SuiteResults.SuiteId, SuiteResults.SuiteResultID
FROM TestStepResults
	INNER JOIN TestResults
		ON TestResults.TestResultID = TestStepResults.TestResultId
	INNER JOIN SuiteResults
		ON SuiteResults.SuiteResultID = TestResults.SuiteResultId
	INNER JOIN Tests
		ON Tests.TestID = TestResults.TestId
WHERE
	SuiteResults.SuiteId = 25 AND
	Tests.TestID = 13 AND
	TestStepResults.TestStepId = 14	
ORDER BY TestStepResults.TestStepResultStartDateTime DESC

-- option 2 (use test & suite to find test results with test steps logged)
SELECT TestResults.TestResultID, TestResults.TestId, TestResults.SuiteResultId FROM TestResults
	INNER JOIN SuiteResults
		ON SuiteResults.SuiteResultID = TestResults.SuiteResultId
WHERE TestResults.TestId = 13
	AND SuiteResults.SuiteId = 25
	AND EXISTS (SELECT 1 FROM TestStepResults
				WHERE TestStepResults.TestResultId = TestResults.TestResultID)
ORDER BY TestResults.TestResultStartDateTime DESC

-- then get the test steps from that test result that we want
SELECT TestStepResults.TestStepResultID, TestStepResults.TestStepId, TestStepResults.TestResultId, TestSteps.TestStepID, TestSteps.TestStepName
FROM TestStepResults
INNER JOIN TestSteps
	ON TestSteps.TestStepID = TestStepResults.TestStepId
WHERE TestStepResults.TestResultId = 27799614
	AND TestSteps.TestStepName LIKE 'I send the request as a%'