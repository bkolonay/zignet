USE ZigNet

SELECT Suites.SuiteName, LatestTestResults.TestResultId, LatestTestResults.TestId, LatestTestResults.TestName, LatestTestResults.PassingFromDateTime, LatestTestResults.FailingFromDateTime
--UPDATE LatestTestResults
--SET PassingFromDateTime = '7/2/2018 1:00AM',
--	FailingFromDateTime = NULL	
FROM LatestTestResults
JOIN Suites
	ON LatestTestResults.SuiteId = Suites.SuiteID
WHERE 
	Suites.SuiteName = 'UI' AND
	LatestTestResults.TestName = 'Verify the sorted provideo has the provideo icon on the thumbnail in the search results'