ALTER TABLE Suite_SuiteCategories
DROP CONSTRAINT FK_SuiteCategorySuite
ALTER TABLE Suite_SuiteCategories
DROP CONSTRAINT FK_SuiteCategoryCategory
DROP TABLE Suite_SuiteCategories

CREATE TABLE Suite_SuiteCategories (
	SuiteId int NOT NULL,
	SuiteCategoryId int NOT NULL,
	PRIMARY KEY (SuiteId, SuiteCategoryId),
	CONSTRAINT FK_SuiteCategorySuite FOREIGN KEY (SuiteId)
	REFERENCES Suites(SuiteID),
	CONSTRAINT FK_SuiteCategoryCategory FOREIGN KEY (SuiteCategoryId)
	REFERENCES SuiteCategories(SuiteCategoryID),	
)