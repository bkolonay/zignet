DELETE FROM TestCategories
INSERT INTO TestCategories
VALUES ('Login')
INSERT INTO TestCategories
VALUES ('Search')
INSERT INTO TestCategories
VALUES ('View Details')

DELETE FROM Tests
INSERT INTO Tests
VALUES ('Login')
INSERT INTO Tests
VALUES ('Login')
INSERT INTO Tests
VALUES ('Search')
INSERT INTO Tests
VALUES ('Search')
INSERT INTO Tests
VALUES ('View details')
INSERT INTO Tests
VALUES ('View details')

DELETE FROM Test_TestCategories
INSERT INTO Test_TestCategories
VALUES (1, 1)
INSERT INTO Test_TestCategories
VALUES (2, 1)
INSERT INTO Test_TestCategories
VALUES (3, 2)
INSERT INTO Test_TestCategories
VALUES (4, 2)
INSERT INTO Test_TestCategories
VALUES (5, 3)
INSERT INTO Test_TestCategories
VALUES (6, 3)

SELECT Tests.TestID, Tests.TestName, TestCategories.TestCategoryID, TestCategories.CategoryName
FROM Tests
JOIN Test_TestCategories
	ON Tests.TestID = Test_TestCategories.TestId
JOIN TestCategories
	ON TestCategories.TestCategoryID = Test_TestCategories.TestCategoryId