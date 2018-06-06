SELECT * FROM TestResults
SELECT * FROM Tests
SELECT * FROM TestResultTypes
SELECT * FROM SuiteResults

DELETE FROM TestResults

INSERT INTO TestResults
VALUES (1, 3, '2/28/2018 3:01pm', '2/28/2018 3:02pm', 3)

SELECT TestResults.TestResultID, TestResults.SuiteResultId, TestResults.TestId, Tests.TestName,
	TestResults.TestResultStartDateTime AS StartTime, TestResults.TestResultEndDateTime AS EndTime,
	TestResultTypes.TestResultTypeName
FROM TestResults
JOIN Tests
	ON TestResults.TestId = Tests.TestID
JOIN TestResultTypes
	ON TestResults.TestResultTypeId = TestResultTypes.TestResultTypeID
ORDER BY TestResults.TestResultID