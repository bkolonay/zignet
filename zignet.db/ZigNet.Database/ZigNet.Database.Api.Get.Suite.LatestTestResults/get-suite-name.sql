USE ZigNet

SELECT TOP (2) 
    [Extent1].[SuiteID] AS [SuiteID], 
    [Extent1].[SuiteName] AS [SuiteName], 
    [Extent2].[ApplicationNameAbbreviation] AS [ApplicationNameAbbreviation], 
    [Extent3].[EnvironmentNameAbbreviation] AS [EnvironmentNameAbbreviation]
    FROM   [dbo].[Suites] AS [Extent1]
    INNER JOIN [dbo].[Applications] AS [Extent2] ON [Extent1].[ApplicationId] = [Extent2].[ApplicationID]
    INNER JOIN [dbo].[Environments] AS [Extent3] ON [Extent1].[EnvironmentId] = [Extent3].[EnvironmentID]
    WHERE [Extent1].[SuiteID] = 25