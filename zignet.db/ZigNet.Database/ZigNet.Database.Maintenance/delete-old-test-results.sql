USE ZigNet

-- 1. make sure the script is connected to the right db (e.g. LABKOLONAY or LN01SQLTTSM501\XCHANGE)

-- 2. use a consistent date through the whole query because test result times can vary by milliseconds
--SELECT DATEADD(day, -30, GETUTCDATE())
DECLARE @thirtyDaysAgoUtc DateTime = '2018-08-21 17:22:56.380'
--SELECT @thirtyDaysAgoUtc

--SELECT COUNT(SuiteResults.SuiteResultID) from SuiteResults
--SELECT * FROM SuiteResults
--WHERE SuiteResultEndDateTime < @thirtyDaysAgoUtc

-- 3. get count of TestResults older than 30 days old
--SELECT COUNT(TestResults.TestResultID) FROM TestResults
--WHERE TestResultEndDateTime < @thirtyDaysAgoUtc
-- count: 4625785

-- 4. get count of TestResults older than 30 days old _and_ have result type as "Fail"
--   the count of this query should match how many
--   TestResult_TestFailureDetails and TestFailureDetails records are deleted
--SELECT COUNT(TestResults.TestResultID) FROM TestResults
--WHERE TestResultEndDateTime < @thirtyDaysAgoUtc
--	AND TestResults.TestResultTypeId = (
--		SELECT TestResultTypes.TestResultTypeID
--		FROM TestResultTypes
--		WHERE TestResultTypes.TestResultTypeName = 'Fail'
--	)
-- count: 275982

-- 5. delete records

--DELETE TestResult_TestFailureDetails
----SELECT COUNT(TestResult_TestFailureDetails.TestResultId)
--FROM TestResult_TestFailureDetails
--JOIN TestResults
--	ON TestResults.TestResultID = TestResult_TestFailureDetails.TestResultId
--WHERE TestResults.TestResultEndDateTime < @thirtyDaysAgoUtc

-- delete orphaned TestFailureDetails that no longer have a TestResult_TestFailureDetails record
--DELETE TestFailureDetails
----SELECT COUNT(TestFailureDetails.TestFailureDetailID)
--FROM TestFailureDetails
--WHERE NOT EXISTS (
--	SELECT TestResult_TestFailureDetails.TestFailureDetailId 
--	FROM TestResult_TestFailureDetails
--	WHERE TestResult_TestFailureDetails.TestFailureDetailId = TestFailureDetails.TestFailureDetailID
--)

--DELETE TestResult_TestFailureType
----SELECT COUNT(TestResult_TestFailureType.TestResultId)
--FROM TestResult_TestFailureType
--JOIN TestResults
--	ON TestResults.TestResultID = TestResult_TestFailureType.TestResultId
--WHERE TestResults.TestResultEndDateTime < @thirtyDaysAgoUtc

------ query can be used to determine which suites have run before, but haven't been run in over 30 days
--SELECT TemporaryTestResults.TemporaryTestResultID, TemporaryTestResults.TestResultId,
-- Suites.SuiteName, Environments.EnvironmentName, Applications.ApplicationName
----SELECT COUNT(TemporaryTestResultID)
--FROM TemporaryTestResults
--JOIN TestResults
--	ON TestResults.TestResultID = TemporaryTestResults.TestResultId
--JOIN Suites
--	ON Suites.SuiteID = TemporaryTestResults.SuiteId
--JOIN Environments
--	ON Environments.EnvironmentID = Suites.EnvironmentId
--JOIN Applications
--	ON Applications.ApplicationID = Suites.ApplicationId
--WHERE TestResults.TestResultEndDateTime < @thirtyDaysAgoUtc

-- note: deleting stale TemporaryTestResults causes dashboard page to no longer show data
--   (because the tests haven't run in the last 30 days)
--DELETE TemporaryTestResults
----SELECT COUNT(TemporaryTestResults.TemporaryTestResultID)
--FROM TemporaryTestResults
--JOIN TestResults
--	ON TestResults.TestResultID = TemporaryTestResults.TestResultId
--WHERE TestResults.TestResultEndDateTime < @thirtyDaysAgoUtc

---- note: deleting stale LatestTestResults causes list page to no longer show data
----   (because the tests haven't run in the last 30 days)
--DELETE LatestTestResults
----SELECT COUNT(LatestTestResults.LatestTestResultID)
--FROM LatestTestResults
--JOIN TestResults
--	ON TestResults.TestResultID = LatestTestResults.TestResultId
--WHERE TestResults.TestResultEndDateTime < @thirtyDaysAgoUtc

--DELETE TestFailureDurations
----SELECT COUNT(TestFailureDurations.TestFailureDurationID)
--FROM TestFailureDurations
--JOIN TestResults
--	ON TestResults.TestResultID = TestFailureDurations.TestResultId
--WHERE TestResults.TestResultEndDateTime < @thirtyDaysAgoUtc

--DELETE FROM TestResults
----SELECT COUNT(TestResults.TestResultID)
--FROM TestResults
--WHERE TestResultEndDateTime < @thirtyDaysAgoUtc