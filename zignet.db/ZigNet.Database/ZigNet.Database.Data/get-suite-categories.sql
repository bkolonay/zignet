SELECT Suites.SuiteID, Suites.SuiteName, SuiteCategories.CategoryName, SuiteCategories.SuiteCategoryID
FROM Suite_SuiteCategories
JOIN Suites
	ON Suite_SuiteCategories.SuiteId = Suites.SuiteID
JOIN SuiteCategories
	ON Suite_SuiteCategories.SuiteCategoryId = SuiteCategories.SuiteCategoryID

SELECT * FROM SuiteCategories