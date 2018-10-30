USE ZigNet

SELECT TestStepResults.TestStepResultID, TestStepResults.TestStepId, TestSteps.TestStepName,
	TestStepResults.TestResultId, TestStepResults.TestStepResultStartDateTime AS StartDateTime, TestStepResults.TestStepResultEndDateTime AS EndDateTime,
	TestStepResults.TestStepResultTypeId AS ResultTypeId, TestStepResultTypes.TestStepResultTypeName AS ResultTypeName
FROM TestStepResults
INNER JOIN TestSteps
	ON TestSteps.TestStepID = TestStepResults.TestStepId
INNER JOIN TestStepResultTypes
	ON TestStepResultTypes.TestStepResultTypeID = TestStepResults.TestStepResultTypeId


--DELETE FROM TestStepResults
--DELETE FROM TestSteps

SELECT * FROM TestSteps