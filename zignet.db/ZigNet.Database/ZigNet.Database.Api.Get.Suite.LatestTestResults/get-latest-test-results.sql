USE ZigNet

SELECT 
    [Extent1].[LatestTestResultID] AS [LatestTestResultID], 
    [Extent1].[SuiteId] AS [SuiteId], 
    [Extent1].[TestId] AS [TestId], 
    [Extent1].[TestResultId] AS [TestResultId], 
    [Extent1].[TestName] AS [TestName], 
    [Extent1].[PassingFromDateTime] AS [PassingFromDateTime], 
    [Extent1].[FailingFromDateTime] AS [FailingFromDateTime], 
    [Extent1].[SuiteName] AS [SuiteName]
    FROM [dbo].[LatestTestResults] AS [Extent1]
    WHERE [Extent1].[SuiteId] = 25