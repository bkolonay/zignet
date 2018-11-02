--ALTER TABLE SuiteResults
--DROP CONSTRAINT FK_SuiteResultSuiteResultType

----DROP TABLE IF EXISTS SuiteResultTypes
--DROP TABLE SuiteResults

USE ZigNet

CREATE TABLE dbo.TestStepResults (
	TestStepResultID int NOT NULL PRIMARY KEY IDENTITY(1,1),
	TestStepId int NOT NULL,
	TestResultId int NOT NULL,
	TestStepResultStartDateTime datetime NOT NULL,
	TestStepResultEndDateTime datetime NOT NULL,
	TestStepResultTypeId int NOT NULL,
	CONSTRAINT FK_TestStepResultTestStep FOREIGN KEY (TestStepId)
	REFERENCES TestSteps(TestStepID),
	CONSTRAINT FK_TestStepResultTestResult FOREIGN KEY (TestResultId)
	REFERENCES TestResults(TestResultID),
	CONSTRAINT FK_TestStepResultTestStepResultType FOREIGN KEY (TestStepResultTypeId)
	REFERENCES TestStepResultTypes(TestStepResultTypeID)
)

SELECT * FROM TestStepResults