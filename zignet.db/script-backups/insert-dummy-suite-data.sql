DELETE FROM SuiteCategories
INSERT INTO SuiteCategories
VALUES ('DEV')
INSERT INTO SuiteCategories
VALUES ('TEST')
INSERT INTO SuiteCategories
VALUES ('PROD')

DELETE FROM Suites
INSERT INTO Suites
VALUES ('DEV - Web Services')
INSERT INTO Suites
VALUES ('TEST - Web Services')
INSERT INTO Suites
VALUES ('PROD - Web Services')
INSERT INTO Suites
VALUES ('DEV - Web UI')
INSERT INTO Suites
VALUES ('TEST - Web UI')
INSERT INTO Suites
VALUES ('PROD - Web UI')

DELETE FROM Suite_SuiteCategories
INSERT INTO Suite_SuiteCategories
VALUES (1, 1)
INSERT INTO Suite_SuiteCategories
VALUES (4, 1)
INSERT INTO Suite_SuiteCategories
VALUES (2, 2)
INSERT INTO Suite_SuiteCategories
VALUES (5, 2)
INSERT INTO Suite_SuiteCategories
VALUES (3, 3)
INSERT INTO Suite_SuiteCategories
VALUES (6, 3)

SELECT Suites.SuiteID, Suites.SuiteName, SuiteCategories.SuiteCategoryID, SuiteCategories.CategoryName
FROM Suites
JOIN Suite_SuiteCategories
	ON Suites.SuiteID = Suite_SuiteCategories.SuiteId
JOIN SuiteCategories
	ON SuiteCategories.SuiteCategoryID = Suite_SuiteCategories.SuiteCategoryId