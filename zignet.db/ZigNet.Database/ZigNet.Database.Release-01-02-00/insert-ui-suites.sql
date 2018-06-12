--INSERT INTO Environments (EnvironmentName, EnvironmentNameAbbreviation)
--VALUES ('Test', 'Test')
--INSERT INTO Environments (EnvironmentName, EnvironmentNameAbbreviation)
--VALUES ('Test Debug', 'Test (D)')
--INSERT INTO Environments (EnvironmentName, EnvironmentNameAbbreviation)
--VALUES ('Development Debug', 'Dev (D)')
--SELECT * FROM Environments

INSERT INTO Suites (SuiteName, EnvironmentId, ApplicationId)
VALUES	('UI (D)',
		(SELECT EnvironmentID
		 FROM Environments
		 WHERE EnvironmentNameAbbreviation = 'DVM (D)'),
		(SELECT ApplicationID
		 FROM Applications
		 WHERE ApplicationName = 'LoopNet'))

INSERT INTO Suites (SuiteName, EnvironmentId, ApplicationId)
VALUES	('UI',
		(SELECT EnvironmentID
		 FROM Environments
		 WHERE EnvironmentNameAbbreviation = 'DVM'),
		(SELECT ApplicationID
		 FROM Applications
		 WHERE ApplicationName = 'LoopNet'))

INSERT INTO Suites (SuiteName, EnvironmentId, ApplicationId)
VALUES	('UI (D)',
		(SELECT EnvironmentID
		 FROM Environments
		 WHERE EnvironmentNameAbbreviation = 'TSR (D)'),
		(SELECT ApplicationID
		 FROM Applications
		 WHERE ApplicationName = 'LoopNet'))

INSERT INTO Suites (SuiteName, EnvironmentId, ApplicationId)
VALUES	('UI',
		(SELECT EnvironmentID
		 FROM Environments
		 WHERE EnvironmentNameAbbreviation = 'TSR'),
		(SELECT ApplicationID
		 FROM Applications
		 WHERE ApplicationName = 'LoopNet'))

INSERT INTO Suites (SuiteName, EnvironmentId, ApplicationId)
VALUES	('UI (D)',
		(SELECT EnvironmentID
		 FROM Environments
		 WHERE EnvironmentNameAbbreviation = 'Prod (D)'),
		(SELECT ApplicationID
		 FROM Applications
		 WHERE ApplicationName = 'LoopNet'))

INSERT INTO Suites (SuiteName, EnvironmentId, ApplicationId)
VALUES	('UI',
		(SELECT EnvironmentID
		 FROM Environments
		 WHERE EnvironmentNameAbbreviation = 'Prod'),
		(SELECT ApplicationID
		 FROM Applications
		 WHERE ApplicationName = 'LoopNet'))


SELECT SuiteID, ApplicationName AS App, ApplicationNameAbbreviation AS AppAbbr, SuiteName, EnvironmentNameAbbreviation AS EnvAbbr, EnvironmentName As Env
FROM Suites
INNER JOIN Applications
	ON Suites.ApplicationId = Applications.ApplicationID
INNER JOIN Environments
	ON Suites.EnvironmentId = Environments.EnvironmentID

--DELETE Suites
--WHERE SuiteID IN (38, 39)