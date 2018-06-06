DROP TABLE IF EXISTS TestResultTypes

CREATE TABLE TestResultTypes (
	TestResultTypeID int NOT NULL PRIMARY KEY IDENTITY(1,1),
	TestResultTypeName NVARCHAR(MAX) NOT NULL
)

INSERT INTO TestResultTypes
VALUES ('Fail')

INSERT INTO TestResultTypes
VALUES ('Inconclusive')

INSERT INTO TestResultTypes
VALUES ('Pass')

SELECT * FROM TestResultTypes