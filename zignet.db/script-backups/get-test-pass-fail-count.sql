SELECT Tests.TestName,
sum(case when TestResults.TestResultTypeId=3 then 1 else 0 end) as Passed,
sum(case when TestResults.TestResultTypeId=1 then 1 else 0 end) as Failed
	FROM SuiteResults
JOIN TestResults
	ON TestResults.SuiteResultId = SuiteResults.SuiteResultID
JOIN Tests
	ON TestResults.TestId = Tests.TestID
WHERE SuiteResults.SuiteId = 25
	AND TestResults.TestResultEndDateTime > '3/31/2018'
GROUP BY Tests.TestName
ORDER BY Failed DESC