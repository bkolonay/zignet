SELECT Tests.TestID, Tests.TestName, TestCategories.CategoryName, TestCategories.TestCategoryID
FROM Test_TestCategories
JOIN Tests
	ON Test_TestCategories.TestId = Tests.TestID
JOIN TestCategories
	ON Test_TestCategories.TestCategoryId = TestCategories.TestCategoryID

SELECT * FROM TestCategories