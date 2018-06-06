--DELETE FROM TestResult_TestFailureDetails
--DELETE FROM TestResult_TestFailureType
--DELETE FROM TestFailureDetails
--DELETE FROM TestResults
--DELETE FROM SuiteResults
--DELETE FROM Suites_Tests
--DELETE FROM Tests

SELECT * FROM Tests
SELECT * FROM TestResults
--SELECT * FROM TestResultTypes
--SELECT * FROM TestCategories
--SELECT * FROM Test_TestCategories
SELECT * FROM TestResult_TestFailureDetails
SELECT * FROM TestResult_TestFailureType
SELECT * FROM TestFailureDetails
--SELECT * FROM TestFailureTypes
--SELECT * FROM Suites_Tests
--SELECT * FROM TestFailureTypes