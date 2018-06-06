--ALTER TABLE SuiteResults
--DROP CONSTRAINT FK_SuiteResultSuiteResultType

----DROP TABLE IF EXISTS SuiteResultTypes
--DROP TABLE SuiteResults

--DROP TABLE [ZigNet].[US\bkolonay].TestFailureDurations

CREATE TABLE dbo.TestFailureDurations (
	TestFailureDurationID int NOT NULL PRIMARY KEY IDENTITY(1,1),
	SuiteId int NOT NULL,
	TestId int NOT NULL,
	TestResultId int NOT NULL,
	FailureStartDateTime datetime,
	FailureEndDateTime datetime,
	CONSTRAINT FK_TestFailureDurationSuite FOREIGN KEY (SuiteId)
	REFERENCES Suites(SuiteID),
	CONSTRAINT FK_TestFailureDurationTest FOREIGN KEY (TestId)
	REFERENCES Tests(TestID),
	CONSTRAINT FK_TestFailureDurationTestResult FOREIGN KEY (TestResultId)
	REFERENCES TestResults(TestResultID)
)

SELECT * FROM TestFailureDurations