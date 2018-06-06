--DROP TABLE IF EXISTS TestResultTypes

CREATE TABLE TestFailureTypes (
	TestFailureTypeID int NOT NULL PRIMARY KEY IDENTITY(1,1),
	TestFailureTypeName NVARCHAR(MAX) NOT NULL
)

INSERT INTO TestFailureTypes
VALUES ('Assertion')

INSERT INTO TestFailureTypes
VALUES ('Exception')

SELECT * FROM TestFailureTypes