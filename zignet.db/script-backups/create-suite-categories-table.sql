DROP TABLE SuiteCategories

CREATE TABLE SuiteCategories (
	SuiteCategoryID int NOT NULL PRIMARY KEY IDENTITY(1,1),
	CategoryName NVARCHAR(MAX) NOT NULL
)

SELECT * FROM SuiteCategories