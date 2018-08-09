USE ZigNet

DECLARE @thirtyDaysAgoUtc DateTime = DATEADD(day, -30, GETUTCDATE())
--SELECT @thirtyDaysAgoUtc

-- get count of TestResults older than 30 days old
--SELECT COUNT(TestResults.TestResultID) FROM TestResults
--WHERE TestResultStartDateTime < @thirtyDaysAgoUtc

-- get count of TestResults older than 30 days old _and_ have result type as "Fail"
--   the count of this query should match how many
--   TestResult_TestFailureDetails and TestFailureDetails records are deleted
--SELECT COUNT(TestResults.TestResultID) FROM TestResults
--WHERE TestResultEndDateTime < @thirtyDaysAgoUtc
--	AND TestResults.TestResultTypeId = (
--		SELECT TestResultTypes.TestResultTypeID
--		FROM TestResultTypes
--		WHERE TestResultTypes.TestResultTypeName = 'Fail'
--	)

--DELETE TestResult_TestFailureDetails
--SELECT COUNT(TestResult_TestFailureDetails.TestResultId)
--FROM TestResult_TestFailureDetails
--JOIN TestResults
--	ON TestResults.TestResultID = TestResult_TestFailureDetails.TestResultId
--WHERE TestResults.TestResultStartDateTime < @thirtyDaysAgoUtc

-- delete orphaned TestFailureDetails that no longer have a TestResult_TestFailureDetails record
--DELETE TestFailureDetails
--SELECT COUNT(TestFailureDetails.TestFailureDetailID)
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

-- query can be used to determine which suites haven't been run in over 30 days
--SELECT TemporaryTestResults.TemporaryTestResultID, TemporaryTestResults.TestResultId,
-- Suites.SuiteName, Environments.EnvironmentName, Applications.ApplicationName
--SELECT COUNT(TemporaryTestResultID)
--FROM TemporaryTestResults
--JOIN TestResults
--	ON TestResults.TestResultID = TemporaryTestResults.TestResultId
----JOIN Suites
----	ON Suites.SuiteID = TemporaryTestResults.SuiteId
----JOIN Environments
----	ON Environments.EnvironmentID = Suites.EnvironmentId
----JOIN Applications
----	ON Applications.ApplicationID = Suites.ApplicationId
--WHERE TestResults.TestResultEndDateTime < @thirtyDaysAgoUtc

-- note: deleting stale TemporaryTestResults causes dashboard page to no longer show data
--   (because the tests haven't run in the last 30 days)
--DELETE TemporaryTestResults
----SELECT COUNT(TemporaryTestResults.TemporaryTestResultID)
--FROM TemporaryTestResults
--JOIN TestResults
--	ON TestResults.TestResultID = TemporaryTestResults.TestResultId
--WHERE TestResults.TestResultEndDateTime < @thirtyDaysAgoUtc

-- note: deleting stale LatestTestResults causes list page to no longer show data
--   (because the tests haven't run in the last 30 days)
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