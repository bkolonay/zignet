SELECT * FROM TestResults
WHERE TestResultStartDateTime < '4/27/2018'
-- highest TestResultID: 710030

SELECT * FROM TestResult_TestFailureDetails
WHERE TestResultId <= 710030
-- highest TestFailureDetailId: 135837

DELETE FROM TestResult_TestFailureDetails
WHERE TestResultId <= 710030

DELETE FROM TestFailureDetails
WHERE TestFailureDetailID <= 135837

DELETE FROM LatestTestResults
WHERE TestResultId <= 710030

DELETE FROM TestResult_TestFailureType
WHERE TestResultId <= 710030

DELETE FROM TestResults
WHERE TestResultStartDateTime < '4/27/2018'