--SELECT * FROM Suites_Tests

SELECT Tests.TestID, Tests.TestName, Suites.SuiteID, Suites.SuiteName 
FROM Suites_Tests
JOIN Tests
	ON Suites_Tests.TestId = Tests.TestID
JOIN Suites
	ON Suites_Tests.SuiteId = Suites.SuiteID
WHERE Tests.TestID = 107 -- get suites a test belongs to
--WHERE Suites.SuiteID = 26 -- get tests for a suite