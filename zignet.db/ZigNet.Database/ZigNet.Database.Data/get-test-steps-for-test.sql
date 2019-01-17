USE ZigNet

SELECT Tests.TestID, Tests.TestName, TestSteps.TestStepID, TestSteps.TestStepName
FROM Tests_TestSteps
	INNER JOIN Tests
		ON Tests.TestID = Tests_TestSteps.TestId
	INNER JOIN TestSteps
		ON TestSteps.TestStepID = Tests_TestSteps.TestStepId
WHERE Tests.TestID = 13