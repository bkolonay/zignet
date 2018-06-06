--ALTER TABLE SuiteResults
--DROP CONSTRAINT FK_SuiteResultSuiteResultType

----DROP TABLE IF EXISTS SuiteResultTypes
--DROP TABLE SuiteResults

CREATE TABLE LatestTestResults (
	LatestTestResultID int NOT NULL PRIMARY KEY IDENTITY(1,1),
	SuiteId int NOT NULL,
	TestId int NOT NULL,
	TestResultId int NOT NULL,
	TestName NVARCHAR(MAX) NOT NULL,
	PassingFromDateTime datetime,
	FailingFromDateTime datetime,
	CONSTRAINT FK_LatestTestResultSuite FOREIGN KEY (SuiteId)
	REFERENCES Suites(SuiteID),
	CONSTRAINT FK_LatestTestResultTest FOREIGN KEY (TestId)
	REFERENCES Tests(TestID),
	CONSTRAINT FK_LatestTestResultTestResult FOREIGN KEY (TestResultId)
	REFERENCES TestResults(TestResultID)
)

SELECT * FROM LatestTestResults