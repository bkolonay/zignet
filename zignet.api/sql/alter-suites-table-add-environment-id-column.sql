USE ZigNet

--DROP TABLE Applications

--ALTER TABLE Suites
--DROP CONSTRAINT FK_SuiteEnvironment

--DROP TABLE [ZigNet].[US\bkolonay].Environments

ALTER TABLE Suites
ADD CONSTRAINT FK_SuiteEnvironment FOREIGN KEY (EnvironmentId)
REFERENCES dbo.Environments(EnvironmentID)

ALTER TABLE Suites
ADD EnvironmentId int NOT NULL
CONSTRAINT DF_SuiteDefaultEnvironmentId DEFAULT 1,
CONSTRAINT FK_SuiteEnvironment FOREIGN KEY (EnvironmentId)
REFERENCES Environments(EnvironmentID)

ALTER TABLE Suites
DROP CONSTRAINT DF_SuiteDefaultEnvironmentId

--INSERT INTO Suites
--VALUES ('LN UI - TSM', 2, 1)

--INSERT INTO Suites
--VALUES ('LN UI - TSM (D)', 7, 1)

--INSERT INTO Suites
--VALUES ('LM Mobile - DVM (D)', 6, 2)

--INSERT INTO Suites
--VALUES ('LM Mobile - DVM', 1, 2)

UPDATE Suites
SET EnvironmentId = 6
--SELECT * FROM Suites
WHERE SuiteID IN (24)

SELECT * FROM Suites
SELECT * FROM Environments
SELECT * FROM Applications

SELECT Suites.SuiteID, Suites.SuiteName,
	/*Environments.EnvironmentName,*/ 
	Environments.EnvironmentNameAbbreviation AS EnvAbbr, Environments.EnvironmentID AS EnvId,
	Applications.ApplicationNameAbbreviation AS AppAbbr, Applications.ApplicationID As AppId
FROM Suites
INNER JOIN Environments
	ON Suites.EnvironmentId = Environments.EnvironmentID
INNER JOIN Applications
	ON Suites.ApplicationId = Applications.ApplicationID

SELECT * FROM Environments

INSERT INTO LatestTestResults
VALUES (33, 57, 209390, 'Test name in LN UI (D) suite', '5/15/2018 11:36am', NULL)

SELECT * FROM LatestTestResults
WHERE SuiteId = 26

SELECT * FROM LatestTestResults 
WHERE SuiteId = 33