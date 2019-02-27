USE ZigNet

--SELECT * FROM Tests WHERE TestName LIKE 'Load watch listing notifications%' --66
--SELECT * FROM TestFailureDurations
--SELECT * FROM TestResults WHERE TestID = 57

--INSERT INTO TestFailureDurations
--VALUES (24, 57, 763785, '4/13/2019 7:00PM', NULL)
--VALUES (24, 52, 763785, '4/13/2019 11:00PM', '4/15/2019 11:15PM')
--VALUES (24, 57, 763785, '4/13/2019 7:00AM', '4/15/2019 7:01AM')

--DELETE FROM TestFailureDurations
--WHERE TestFailureDurationID = 17

--UPDATE TestFailureDurations
--SET FailureStartDateTime = '4/13/2019 11:00am', FailureEndDateTime = '4/15/2019 4:00pm'
--WHERE TestFailureDurationID = 7

--UPDATE TestFailureDurations
--SET SuiteId = 24, TestId = 52
----WHERE SuiteId = 24 AND TestId = 14 AND FailureEndDateTime IS NOT NULL
--WHERE TestFailureDurationID IN (3, 4, 6, 9, 10)


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