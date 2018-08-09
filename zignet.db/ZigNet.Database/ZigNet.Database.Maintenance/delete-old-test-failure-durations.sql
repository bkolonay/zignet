-- only delete records that have an _end date_ greater than 48 hours ago

SELECT GETUTCDATE() AS UtcDate, DATEADD(day, -2, GETUTCDATE()) AS TwoDaysAgoUtc

--SELECT * FROM TestFailureDurations
--WHERE FailureEndDateTime < DATEADD(day, -2, GETUTCDATE())

SELECT COUNT(TestFailureDurationID) AS StaleTestFailureDurations FROM TestFailureDurations
WHERE FailureEndDateTime < DATEADD(day, -2, GETUTCDATE())

--DELETE FROM TestFailureDurations
--WHERE FailureEndDateTime < DATEADD(day, -2, GETUTCDATE())

SELECT * FROM TestFailureDurations