--ALTER TABLE SuiteResults
--DROP CONSTRAINT FK_SuiteResultSuiteResultType

----DROP TABLE IF EXISTS SuiteResultTypes
--DROP TABLE SuiteResults

CREATE TABLE TestResults (
	TestResultID int NOT NULL PRIMARY KEY IDENTITY(1,1),
	TestId int NOT NULL,
	SuiteResultId int NOT NULL,
	TestResultStartDateTime datetime NOT NULL,
	TestResultEndDateTime datetime NOT NULL,
	TestResultTypeId int NOT NULL,
	CONSTRAINT FK_TestResultTest FOREIGN KEY (TestId)
	REFERENCES Tests(TestID),
	CONSTRAINT FK_TestResultSuiteResult FOREIGN KEY (SuiteResultId)
	REFERENCES SuiteResults(SuiteResultID),
	CONSTRAINT FK_TestResultTestResultType FOREIGN KEY (TestResultTypeId)
	REFERENCES TestResultTypes(TestResultTypeID)
)

SELECT * FROM TestResults