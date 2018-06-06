DELETE FROM Suite_SuiteCategories
WHERE SuiteId = 1
	AND SuiteCategoryId > 1

DELETE FROM Suite_SuiteCategories
WHERE SuiteId > 6

DELETE FROM SuiteCategories
WHERE SuiteCategoryID > 3

DELETE FROM Suites
WHERE SuiteID > 6

SELECT * FROM Suites
SELECT * FROM SuiteCategories
SELECT * FROM Suite_SuiteCategories