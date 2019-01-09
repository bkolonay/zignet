USE ZigNet

--DECLARE @TestName NVARCHAR(MAX) = 'Search for brokers'
--DECLARE @TestStepName NVARCHAR(MAX) = 'the endpoint: "/broker/search"'

--INSERT INTO Tests_TestSteps (TestId, TestStepId)
--VALUES 
--(
--	(SELECT TestID
--		FROM Tests
--		WHERE TestName = @TestName),
--	(SELECT TestStepID
--		FROM TestSteps
--		WHERE TestStepName = @TestStepName)
--		--WHERE TestStepID = 1567)
--)

SELECT Tests.TestID, Tests.TestName, TestSteps.TestStepID, TestSteps.TestStepName FROM Tests
INNER JOIN Tests_TestSteps
	ON Tests_TestSteps.TestId = Tests.TestID
INNER JOIN TestSteps
	ON TestSteps.TestStepID = Tests_TestSteps.TestStepId
WHERE TestName = 'Search for brokers'

--SELECT * FROM TestSteps
--WHERE TestStepName = 'I send the request as a "POST" to the endpoint'
--SELECT * FROM Tests_TestSteps