USE ZigNet

DROP TABLE IF EXISTS dbo.TestStepResultTypes

CREATE TABLE dbo.TestStepResultTypes (
	TestStepResultTypeID int NOT NULL PRIMARY KEY IDENTITY(1,1),
	TestStepResultTypeName NVARCHAR(MAX) NOT NULL
)

INSERT INTO TestStepResultTypes
VALUES ('Fail')

INSERT INTO TestStepResultTypes
VALUES ('Inconclusive')

INSERT INTO TestStepResultTypes
VALUES ('Pass')

SELECT * FROM TestStepResultTypes