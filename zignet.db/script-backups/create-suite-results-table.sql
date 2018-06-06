--ALTER TABLE SuiteResults
--DROP CONSTRAINT FK_SuiteResultSuiteResultType

----DROP TABLE IF EXISTS SuiteResultTypes
--DROP TABLE SuiteResults

CREATE TABLE SuiteResults (
	SuiteResultID int NOT NULL PRIMARY KEY IDENTITY(1,1),
	SuiteId int NOT NULL,
	SuiteResultStartDateTime datetime NOT NULL,
	SuiteResultEndDateTime datetime NOT NULL,
	SuiteResultTypeId int NOT NULL,
	CONSTRAINT FK_SuiteResultSuite FOREIGN KEY (SuiteId)
	REFERENCES Suites(SuiteID),
	CONSTRAINT FK_SuiteResultSuiteResultType FOREIGN KEY (SuiteResultTypeId)
	REFERENCES SuiteResultTypes(SuiteResultTypeID)
)

SELECT * FROM SuiteResults