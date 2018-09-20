USE ZigNet

-- 1. make sure the script is connected to the right db (e.g. LABKOLONAY or LN01SQLTTSM501\XCHANGE)

-- 2. run SELECT query to determine which Tests haven't run within x days (e.g. 30, 15, 5, or 1)

--SELECT DATEADD(day, -5, GETUTCDATE())
DECLARE @thirtyDaysAgoUtc DateTime = '2018-08-21 22:12:04.100'
DECLARE @fifteenDaysAgoUtc DateTime = '2018-09-05 19:08:21.837'
DECLARE @fiveDaysAgoUtc DateTime = '2018-09-15 22:40:10.587'
DECLARE @oneDayAgoUtc DateTime = '2018-09-19 19:27:14.393'

SELECT Tests.TestID, Tests.TestName,
	(SELECT COUNT(TestResults.TestResultID)
	 FROM TestResults
	 WHERE TestResults.TestId = Tests.TestID
		AND TestResults.TestResultEndDateTime > @fiveDaysAgoUtc) AS TestResultCount
FROM Tests
--WHERE TestName = 'Verify Company News page is shown correctly'
ORDER BY TestResultCount

-- get number of test results per test (those with low test result count _may_ have been renamed)
--SELECT TestResults.TestId, COUNT(TestResults.TestResultID) AS TestResultCount
--FROM TestResults
--GROUP BY TestResults.TestId
--ORDER BY TestResultCount

-- get number of test results for the test
--SELECT COUNT(TestResults.TestResultID) 
--FROM TestResults
--JOIN Tests
--	ON Tests.TestID = TestResults.TestId
--WHERE Tests.TestName = @testName
---- count: 144

-- 3. paste the Test name here to delete the Test and all of its dependencies
DECLARE @testName varchar(MAX) = 'Verify Company Awards page is shown correctly'

DELETE Test_TestCategories
--SELECT *
FROM Test_TestCategories
JOIN Tests
	ON Tests.TestID = Test_TestCategories.TestId
JOIN TestCategories
	ON TestCategories.TestCategoryID = Test_TestCategories.TestCategoryId
WHERE Tests.TestName = @testName

DELETE Suites_Tests
--SELECT *
FROM Suites_Tests
JOIN Tests
	ON Tests.TestID = Suites_Tests.TestId
JOIN Suites
	ON Suites.SuiteID = Suites_Tests.SuiteId
WHERE Tests.TestName = @testName

-- get count of TestResults for the Test that have result type as "Fail"
--   the count of this query should match how many
--   TestResult_TestFailureDetails and TestFailureDetails records are deleted
--SELECT COUNT(TestResults.TestResultID) FROM TestResults
--JOIN Tests
--	ON Tests.TestID = TestResults.TestId
--WHERE Tests.TestName = @testName
--	AND TestResults.TestResultTypeId = (
--		SELECT TestResultTypes.TestResultTypeID
--		FROM TestResultTypes
--		WHERE TestResultTypes.TestResultTypeName = 'Fail'
--	)
-- --count: 9

DELETE TestResult_TestFailureDetails
--SELECT COUNT(TestResult_TestFailureDetails.TestResultId)
FROM TestResult_TestFailureDetails
JOIN TestResults
	ON TestResults.TestResultID = TestResult_TestFailureDetails.TestResultId
JOIN Tests
	ON Tests.TestID = TestResults.TestId
WHERE Tests.TestName = @testName

-- delete orphaned TestFailureDetails that no longer have a TestResult_TestFailureDetails record
DELETE TestFailureDetails
--SELECT COUNT(TestFailureDetails.TestFailureDetailID)
FROM TestFailureDetails
WHERE NOT EXISTS (
	SELECT TestResult_TestFailureDetails.TestFailureDetailId 
	FROM TestResult_TestFailureDetails
	WHERE TestResult_TestFailureDetails.TestFailureDetailId = TestFailureDetails.TestFailureDetailID
)

DELETE TestResult_TestFailureType
--SELECT COUNT(TestResult_TestFailureType.TestResultId)
FROM TestResult_TestFailureType
JOIN TestResults
	ON TestResults.TestResultID = TestResult_TestFailureType.TestResultId
JOIN Tests
	ON Tests.TestID = TestResults.TestId
WHERE Tests.TestName = @testName

-- should only return 1 record
DELETE TemporaryTestResults
--SELECT COUNT(TemporaryTestResults.TemporaryTestResultID)
FROM TemporaryTestResults
JOIN TestResults
	ON TestResults.TestResultID = TemporaryTestResults.TestResultId
JOIN Tests
	ON Tests.TestID = TestResults.TestId
WHERE Tests.TestName = @testName

-- should only return 1 record (this will remove the test name from the list page)
DELETE LatestTestResults
--SELECT COUNT(LatestTestResults.LatestTestResultID)
FROM LatestTestResults
JOIN TestResults
	ON TestResults.TestResultID = LatestTestResults.TestResultId
JOIN Tests
	ON Tests.TestID = TestResults.TestId
WHERE Tests.TestName = @testName

DELETE TestFailureDurations
--SELECT COUNT(TestFailureDurations.TestFailureDurationID)
FROM TestFailureDurations
JOIN TestResults
	ON TestResults.TestResultID = TestFailureDurations.TestResultId
JOIN Tests
	ON Tests.TestID = TestResults.TestId
WHERE Tests.TestName = @testName

-- should be same count as query at top
DELETE FROM TestResults
--SELECT COUNT(TestResults.TestResultID)
FROM TestResults
JOIN Tests
	ON Tests.TestID = TestResults.TestId
WHERE Tests.TestName = @testName

DELETE Tests
--SELECT *
FROM Tests
WHERE TestName = @testName