DROP TABLE IF EXISTS TestFailureDetails

CREATE TABLE TestFailureDetails (
	TestFailureDetailID int NOT NULL PRIMARY KEY IDENTITY(1,1),
	TestFailureDetail NVARCHAR(MAX) NOT NULL
)

SELECT * FROM TestFailureDetails