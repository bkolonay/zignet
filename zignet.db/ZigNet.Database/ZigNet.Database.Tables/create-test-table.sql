DROP TABLE IF EXISTS Tests

CREATE TABLE Tests (
	TestID int NOT NULL PRIMARY KEY IDENTITY(1,1),
	TestName NVARCHAR(MAX) NOT NULL
)

SELECT * FROM Tests