--ALTER TABLE SuiteResults
--DROP CONSTRAINT FK_SuiteResultSuiteResultType

----DROP TABLE IF EXISTS SuiteResultTypes
--DROP TABLE SuiteResults

CREATE TABLE TemporaryTestResults (
	TemporaryTestResultID int NOT NULL PRIMARY KEY IDENTITY(1,1),
	TestResultId int NOT NULL,
	SuiteResultId int NOT NULL,
	SuiteId int NOT NULL,
	TestResultTypeId int NOT NULL,
	CONSTRAINT FK_TemporaryTestResultTestResult FOREIGN KEY (TestResultId)
		REFERENCES TestResults(TestResultID),
	CONSTRAINT FK_TemporaryTestResultSuiteResult FOREIGN KEY (SuiteResultId)
		REFERENCES SuiteResults(SuiteResultID),
	CONSTRAINT FK_TemporaryTestResultSuite FOREIGN KEY (SuiteId)
		REFERENCES Suites(SuiteID),
	CONSTRAINT FK_TemporaryTestResultTestResultType FOREIGN KEY (TestResultTypeId)
		REFERENCES TestResultTypes(TestResultTypeID)
)

SELECT * FROM TemporaryTestResults