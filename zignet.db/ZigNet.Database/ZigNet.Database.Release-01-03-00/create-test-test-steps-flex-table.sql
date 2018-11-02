--ALTER TABLE Test_TestCategories
--DROP CONSTRAINT FK_TestCategoryTest
--ALTER TABLE Test_TestCategories
--DROP CONSTRAINT FK_TestCategoryCategory
--DROP TABLE Test_TestCategories

USE ZigNet

CREATE TABLE dbo.Tests_TestSteps (
	TestId int NOT NULL,
	TestStepId int NOT NULL,
	PRIMARY KEY (TestId, TestStepId),
	CONSTRAINT FK_TestTestStep FOREIGN KEY (TestId)
	REFERENCES Tests(TestID),
	CONSTRAINT FK_TestStepTest FOREIGN KEY (TestStepId)
	REFERENCES TestSteps(TestStepID)
)

SELECT * FROM Tests_TestSteps