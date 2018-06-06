SELECT TestFailureTypes.TestFailureTypeID, TestFailureTypes.TestFailureTypeName,
	TestResults.TestResultID, Tests.TestName
FROM TestFailureTypes
JOIN TestResult_TestFailureType
	ON TestFailureTypes.TestFailureTypeID = TestResult_TestFailureType.TestFailureTypeId
JOIN TestResults
	ON TestResult_TestFailureType.TestResultId = TestResults.TestResultID
JOIN Tests
	ON TestResults.TestId = Tests.TestID
WHERE TestName = 'Get profile email'
	--AND TestFailureTypes.TestFailureTypeID > 65246
	AND TestFailureTypes.TestFailureTypeName = 'Exception'
ORDER BY TestResults.TestResultID DESC