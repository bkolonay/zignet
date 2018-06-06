--ALTER TABLE Test_TestCategories
--DROP CONSTRAINT FK_TestCategoryTest
--ALTER TABLE Test_TestCategories
--DROP CONSTRAINT FK_TestCategoryCategory
--DROP TABLE Test_TestCategories

CREATE TABLE TestResult_TestFailureDetails (
	TestResultId int NOT NULL,
	TestFailureDetailId int NOT NULL,
	PRIMARY KEY (TestResultId, TestFailureDetailId),
	CONSTRAINT FK_TestResultFailureDetail FOREIGN KEY (TestResultId)
	REFERENCES TestResults(TestResultID),
	CONSTRAINT FK_FailureDetailTestResult FOREIGN KEY (TestFailureDetailId)
	REFERENCES TestFailureDetails(TestFailureDetailID)
)

SELECT * FROM TestResult_TestFailureDetails