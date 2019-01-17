USE ZigNet

SELECT Tests.TestID, Tests.TestName, Suites.SuiteID FROM Tests
	INNER JOIN Suites_Tests
		ON Suites_Tests.TestId = Tests.TestID
	INNER JOIN Suites
		ON Suites_Tests.SuiteId = Suites.SuiteID
WHERE Suites.SuiteId = 25