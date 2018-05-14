--DROP TABLE Applications

ALTER TABLE Suites
ADD EnvironmentId int NOT NULL
CONSTRAINT DF_SuiteDefaultEnvironmentId DEFAULT 1,
CONSTRAINT FK_SuiteEnvironment FOREIGN KEY (EnvironmentId)
REFERENCES Environments(EnvironmentID)

ALTER TABLE Suites
DROP CONSTRAINT DF_SuiteDefaultEnvironmentId

UPDATE Suites
SET EnvironmentId = 4
--SELECT * FROM Suites
WHERE SuiteID IN (29, 31)

SELECT Suites.SuiteID, Suites.SuiteName, /*Environments.EnvironmentName,*/ Environments.EnvironmentNameAbbreviation AS EnvAbbr, Environments.EnvironmentID AS EnvId
FROM Suites
INNER JOIN Environments
	ON Suites.EnvironmentId = Environments.EnvironmentID

SELECT * FROM Environments