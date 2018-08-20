USE ZigNet

-- find Tests that have more than 1 TestFailureDuration with a null FailureEndDateTime
-- note: this will _not_ find all tests that have invalid data (just some of them)
SELECT Tests.TestName, COUNT(TestFailureDurations.TestFailureDurationID) AS NullEndTimeTestFailureDurations
FROM TestFailureDurations
JOIN Tests
	ON Tests.TestID = TestFailureDurations.TestId
JOIN Suites
	ON Suites.SuiteID = TestFailureDurations.SuiteId
WHERE TestFailureDurations.FailureEndDateTime IS NULL
GROUP BY Tests.TestName
HAVING COUNT(TestFailureDurations.TestFailureDurationID) > 1

DECLARE @testName varchar(MAX) = 'Create a multi watch listing in FL watch folder'

SELECT TestFailureDurations.TestFailureDurationID, Tests.TestName,
	TestFailureDurations.FailureStartDateTime, TestFailureDurations.FailureEndDateTime
FROM TestFailureDurations
JOIN Tests
	ON Tests.TestID = TestFailureDurations.TestId
WHERE Tests.TestName = @testName
	--AND TestFailureDurations.FailureEndDateTime IS NULL
ORDER BY FailureStartDateTime DESC

UPDATE TestFailureDurations
SET FailureEndDateTime = DATEADD(minute, 2, FailureStartDateTime)
--SELECT *
FROM TestFailureDurations
JOIN Tests
	ON Tests.TestID = TestFailureDurations.TestId
WHERE Tests.TestName = @testName
	AND FailureEndDateTime IS NULL

--SELECT *
--FROM TestFailureDurations
--WHERE TestFailureDurationID IN (42106, 41705)