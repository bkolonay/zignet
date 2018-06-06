ALTER TABLE Test_TestCategories
DROP CONSTRAINT FK_TestCategoryTest
ALTER TABLE Test_TestCategories
DROP CONSTRAINT FK_TestCategoryCategory
DROP TABLE Test_TestCategories

CREATE TABLE Test_TestCategories (
	TestId int NOT NULL,
	TestCategoryId int NOT NULL,
	PRIMARY KEY (TestId, TestCategoryId),
	CONSTRAINT FK_TestCategoryTest FOREIGN KEY (TestId)
	REFERENCES Tests(TestID),
	CONSTRAINT FK_TestCategoryCategory FOREIGN KEY (TestCategoryId)
	REFERENCES TestCategories(TestCategoryID),	
)