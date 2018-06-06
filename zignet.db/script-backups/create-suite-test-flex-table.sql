--ALTER TABLE Test_TestCategories
--DROP CONSTRAINT FK_TestCategoryTest
--ALTER TABLE Test_TestCategories
--DROP CONSTRAINT FK_TestCategoryCategory
--DROP TABLE Test_TestCategories

CREATE TABLE Suites_Tests (
	SuiteId int NOT NULL,
	TestId int NOT NULL,
	PRIMARY KEY (SuiteId, TestId),
	CONSTRAINT FK_SuiteTest FOREIGN KEY (SuiteId)
	REFERENCES Suites(SuiteID),
	CONSTRAINT FK_TestSuite FOREIGN KEY (TestId)
	REFERENCES Tests(TestID)
)

SELECT * FROM Suites_Tests