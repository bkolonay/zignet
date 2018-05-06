USE ZigNet

SELECT TestFailureDurations.TestFailureDurationID, TestFailureDurations.SuiteId, Suites.SuiteName, TestFailureDurations.TestId, Tests.TestName,
	TestFailureDurations.FailureStartDateTime, TestFailureDurations.FailureEndDateTime,
	TestFailureDurations.TestResultId, /*TestResults.TestResultTypeId,*/ TestResultTypes.TestResultTypeName
FROM TestFailureDurations
JOIN Suites
	ON Suites.SuiteID = TestFailureDurations.SuiteId
JOIN Tests
	ON Tests.TestID = TestFailureDurations.TestId
JOIN TestResults
	ON TestResults.TestResultID = TestFailureDurations.TestResultId
JOIN TestResultTypes
	ON TestResultTypes.TestResultTypeID = TestResults.TestResultTypeId