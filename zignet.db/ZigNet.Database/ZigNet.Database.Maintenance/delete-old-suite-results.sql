-- need to delete old TestResults before trying to delete SuiteResults
-- best practice is to use same UTC date/time in both scripts

USE ZigNet

-- need to use a consistent date through the whole query because test result times can vary by milliseconds
--SELECT DATEADD(day, -30, GETUTCDATE())
DECLARE @thirtyDaysAgoUtc DateTime = '2018-07-10 18:46:34.893'
--SELECT @thirtyDaysAgoUtc

DELETE SuiteResults
--SELECT COUNT(SuiteResults.SuiteResultID)
--SELECT *
FROM SuiteResults
WHERE SuiteResultEndDateTime < @thirtyDaysAgoUtc