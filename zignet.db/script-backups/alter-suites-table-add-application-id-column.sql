--DROP TABLE Applications

--ALTER TABLE Suites
--DROP CONSTRAINT FK_SuiteApplication

--DROP TABLE [ZigNet].[US\bkolonay].Applications

ALTER TABLE Suites
ADD CONSTRAINT FK_SuiteApplication FOREIGN KEY (ApplicationId)
REFERENCES dbo.Applications(ApplicationID)

ALTER TABLE Suites
ADD ApplicationId int NOT NULL
CONSTRAINT DF_SuiteDefaultApplicationId DEFAULT 1,
CONSTRAINT FK_SuiteApplication FOREIGN KEY (ApplicationId)
REFERENCES Applications(ApplicationID)

ALTER TABLE Suites
DROP CONSTRAINT DF_SuiteDefaultApplicationId

SELECT * FROM Suites
SELECT * FROM Applications