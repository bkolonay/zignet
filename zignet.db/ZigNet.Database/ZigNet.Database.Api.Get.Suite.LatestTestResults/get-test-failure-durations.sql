USE ZigNet

--DROP INDEX IX_TestFailureDurations_SuiteId_TestId_FailureEndDateTime
--    ON TestFailureDurations

--CREATE NONCLUSTERED INDEX IX_TestFailureDurations_SuiteId_TestId_FailureEndDateTime
--    ON TestFailureDurations (SuiteId, TestId, FailureEndDateTime)

SELECT 
    [Extent1].[TestFailureDurationID] AS [TestFailureDurationID], 
    [Extent1].[SuiteId] AS [SuiteId], 
    [Extent1].[TestId] AS [TestId], 
    [Extent1].[TestResultId] AS [TestResultId], 
    [Extent1].[FailureStartDateTime] AS [FailureStartDateTime], 
    [Extent1].[FailureEndDateTime] AS [FailureEndDateTime]
    FROM [dbo].[TestFailureDurations] AS [Extent1]
    WHERE ([Extent1].[SuiteId] = 25) AND ([Extent1].[TestId] = 183) AND (([Extent1].[FailureEndDateTime] > '6/5/2018 7:44:05 PM') OR ([Extent1].[FailureEndDateTime] IS NULL))

SELECT 
    TestFailureDurationID, 
    SuiteId, 
    TestId, 
    TestResultId
    FailureStartDateTime,
    FailureEndDateTime
    FROM TestFailureDurations
    WHERE (TestFailureDurations.SuiteId = 25) AND 
		  (TestFailureDurations.TestId = 183) AND
		  ((TestFailureDurations.FailureEndDateTime > '6/5/2018 7:44:05 PM') OR (TestFailureDurations.FailureEndDateTime IS NULL))