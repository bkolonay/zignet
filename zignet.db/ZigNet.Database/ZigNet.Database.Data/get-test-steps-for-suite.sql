USE ZigNet

DECLARE @environment VARCHAR(MAX) = 'TestMain'
DECLARE @app VARCHAR(MAX) = 'LoopNet'
--DECLARE @app VARCHAR(MAX) = 'CityFeet'
--DECLARE @app VARCHAR(MAX) = 'Listing Manager Mobile'
--DECLARE @app VARCHAR(MAX) = 'Showcase'
DECLARE @suite VARCHAR(MAX) = 'Services'

SELECT TestStepResults.TestStepResultID, 
	Applications.ApplicationName AS AppName, Suites.SuiteName AS SuiteName, Environments.EnvironmentName AS EnvName,
	TestSteps.TestStepName,	TestStepResults.TestResultId, 
	TestStepResults.TestStepResultStartDateTime AS StartDateTime, TestStepResults.TestStepResultEndDateTime AS EndDateTime
FROM TestStepResults
INNER JOIN TestSteps
	ON TestSteps.TestStepID = TestStepResults.TestStepId
INNER JOIN TestStepResultTypes
	ON TestStepResultTypes.TestStepResultTypeID = TestStepResults.TestStepResultTypeId
INNER JOIN TestResults
	ON TestResults.TestResultID = TestStepResults.TestResultId
INNER JOIN SuiteResults
	ON SuiteResults.SuiteResultID = TestResults.SuiteResultId
INNER JOIN Suites
	ON Suites.SuiteId = SuiteResults.SuiteId
INNER JOIN Applications
	ON Suites.ApplicationId = Applications.ApplicationID
INNER JOIN Environments
	ON Suites.EnvironmentId = Environments.EnvironmentID
WHERE Applications.ApplicationName = @app
	AND Suites.SuiteName = @suite
	AND Environments.EnvironmentName = @environment
ORDER BY TestStepResults.TestStepResultStartDateTime DESC