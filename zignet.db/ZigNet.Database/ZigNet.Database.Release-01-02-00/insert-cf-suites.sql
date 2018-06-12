--INSERT INTO Environments (EnvironmentName, EnvironmentNameAbbreviation)
--VALUES ('Test', 'Test')
--INSERT INTO Environments (EnvironmentName, EnvironmentNameAbbreviation)
--VALUES ('Test Debug', 'Test (D)')
--INSERT INTO Environments (EnvironmentName, EnvironmentNameAbbreviation)
--VALUES ('Development Debug', 'Dev (D)')
--SELECT * FROM Environments

INSERT INTO Suites (SuiteName, EnvironmentId, ApplicationId)
VALUES	('Services (D)',
		(SELECT EnvironmentID
		 FROM Environments
		 WHERE EnvironmentNameAbbreviation = 'Dev (D)'),
		(SELECT ApplicationID
		 FROM Applications
		 WHERE ApplicationName = 'CityFeet'))

INSERT INTO Suites (SuiteName, EnvironmentId, ApplicationId)
VALUES	('Services (D)',
		(SELECT EnvironmentID
		 FROM Environments
		 WHERE EnvironmentNameAbbreviation = 'Test (D)'),
		(SELECT ApplicationID
		 FROM Applications
		 WHERE ApplicationName = 'CityFeet'))

INSERT INTO Suites (SuiteName, EnvironmentId, ApplicationId)
VALUES	('Services',
		(SELECT EnvironmentID
		 FROM Environments
		 WHERE EnvironmentNameAbbreviation = 'Dev'),
		(SELECT ApplicationID
		 FROM Applications
		 WHERE ApplicationName = 'CityFeet'))

INSERT INTO Suites (SuiteName, EnvironmentId, ApplicationId)
VALUES	('Services',
		(SELECT EnvironmentID
		 FROM Environments
		 WHERE EnvironmentNameAbbreviation = 'Test'),
		(SELECT ApplicationID
		 FROM Applications
		 WHERE ApplicationName = 'CityFeet'))


SELECT SuiteID, ApplicationName AS App, ApplicationNameAbbreviation AS AppAbbr, SuiteName, EnvironmentNameAbbreviation AS EnvAbbr, EnvironmentName As Env
FROM Suites
INNER JOIN Applications
	ON Suites.ApplicationId = Applications.ApplicationID
INNER JOIN Environments
	ON Suites.EnvironmentId = Environments.EnvironmentID

--DELETE Suites
--WHERE SuiteID IN (38, 39)