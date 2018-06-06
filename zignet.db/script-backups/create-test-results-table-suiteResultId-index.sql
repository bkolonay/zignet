DROP INDEX IX_TestResult_SuiteResultId
    ON TestResults

CREATE NONCLUSTERED INDEX IX_TestResult_SuiteResultId
    ON TestResults (SuiteResultId)