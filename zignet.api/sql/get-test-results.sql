SELECT TestResults.TestResultID, TestResults.SuiteResultId, TestResults.TestId, Tests.TestName,
	TestResults.TestResultStartDateTime AS StartTime, TestResults.TestResultEndDateTime AS EndTime,
	TestResultTypes.TestResultTypeName
FROM TestResults
JOIN Tests
	ON TestResults.TestId = Tests.TestID
JOIN TestResultTypes
	ON TestResults.TestResultTypeId = TestResultTypes.TestResultTypeID
ORDER BY TestResults.TestResultID


SELECT TestResults.TestResultID, TestResults.TestId, Tests.TestName,
	TestResults.TestResultStartDateTime AS StartTime, TestResults.TestResultEndDateTime AS EndTime,
	TestFailureTypes.TestFailureTypeName
FROM TestResult_TestFailureType
JOIN TestResults
	ON TestResult_TestFailureType.TestResultId = TestResults.TestResultID
JOIN TestFailureTypes
	ON TestResult_TestFailureType.TestFailureTypeId = TestFailureTypes.TestFailureTypeID
JOIN Tests
	ON TestResults.TestId = Tests.TestID

SELECT TestResults.TestResultID, TestResults.TestId, Tests.TestName,
	TestResults.TestResultStartDateTime AS StartTime, TestResults.TestResultEndDateTime AS EndTime,
	TestFailureDetails.TestFailureDetail
FROM TestResult_TestFailureDetails
JOIN TestResults
	ON TestResult_TestFailureDetails.TestResultId = TestResults.TestResultID
JOIN TestFailureDetails
	ON TestResult_TestFailureDetails.TestFailureDetailId = TestFailureDetails.TestFailureDetailID
JOIN Tests
	ON TestResults.TestId = Tests.TestID

SELECT * FROM SuiteCategories


UPDATE Suites
SET SuiteName = 'LoopNet Services - Prod'
WHERE SuiteID = 31
SELECT * FROM Suites

--DELETE FROM Suite_SuiteCategories WHERE SuiteId = 27
--DELETE FROM Suites WHERE SuiteId = 27
--DELETE FROM SuiteCategories WHERE SuiteCategoryID = 31