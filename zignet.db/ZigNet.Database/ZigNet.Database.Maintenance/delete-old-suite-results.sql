-- 1. make sure the script is connected to the right db (e.g. LABKOLONAY or LN01SQLTTSM501\XCHANGE)
-- 2. old TestResults need deleted before trying to delete SuiteResults
-- 3. best practice: use same UTC date/time as TestResult
-- 4. if same utc date/time doesn't work, use the SELECT query at the bottom to tweak the 30 days ago date

USE ZigNet

-- need to use a consistent date through the whole query because test result times can vary by milliseconds
--SELECT DATEADD(day, -30, GETUTCDATE())
DECLARE @thirtyDaysAgoUtc DateTime = '2019-04-20 17:22:56.380'
--SELECT @thirtyDaysAgoUtc

--DELETE SuiteResults
----SELECT COUNT(SuiteResults.SuiteResultID)
----SELECT *
--FROM SuiteResults
--WHERE SuiteResultEndDateTime < @thirtyDaysAgoUtc

-- this helps fix the issue where a SuiteResult falls within the 'past 30 days' window, but TestResult(s)
--  attached to it have an end time before the window (so it throws foreign key errors)
-- use this query to change the '30 days ago' date to farther into the past (e.g. from 4/21 to 4/20)
--SELECT SuiteResults.SuiteResultID, TestResults.TestResultID, SuiteResults.SuiteResultEndDateTime, TestResults.TestResultEndDateTime
--FROM SuiteResults
--	JOIN TestResults ON TestResults.SuiteResultId = SuiteResults.SuiteResultID
--WHERE SuiteResultEndDateTime < @thirtyDaysAgoUtc
--	--AND TestResults.TestResultEndDateTime < @thirtyDaysAgoUtc