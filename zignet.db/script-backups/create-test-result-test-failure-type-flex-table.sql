--ALTER TABLE Test_TestCategories
--DROP CONSTRAINT FK_TestCategoryTest
--ALTER TABLE Test_TestCategories
--DROP CONSTRAINT FK_TestCategoryCategory
--DROP TABLE Test_TestCategories

CREATE TABLE TestResult_TestFailureType (
	TestResultId int NOT NULL,
	TestFailureTypeId int NOT NULL,
	PRIMARY KEY (TestResultId, TestFailureTypeId),
	CONSTRAINT FK_TestResultFailureType FOREIGN KEY (TestResultId)
	REFERENCES TestResults(TestResultID),
	CONSTRAINT FK_FailureTypeTestResult FOREIGN KEY (TestFailureTypeId)
	REFERENCES TestFailureTypes(TestFailureTypeID)
)

SELECT * FROM TestResult_TestFailureType