DROP TABLE IF EXISTS TestCategories

CREATE TABLE TestCategories (
	TestCategoryID int NOT NULL PRIMARY KEY IDENTITY(1,1),
	CategoryName NVARCHAR(MAX) NOT NULL
)

SELECT * FROM TestCategories