--DROP TABLE IF EXISTS SuiteResultTypes

CREATE TABLE SuiteResultTypes (
	SuiteResultTypeID int NOT NULL PRIMARY KEY IDENTITY(1,1),
	SuiteResultTypeName NVARCHAR(MAX) NOT NULL
)

INSERT INTO SuiteResultTypes
VALUES ('Fail')

INSERT INTO SuiteResultTypes
VALUES ('Inconclusive')

INSERT INTO SuiteResultTypes
VALUES ('Pass')

SELECT * FROM SuiteResultTypes