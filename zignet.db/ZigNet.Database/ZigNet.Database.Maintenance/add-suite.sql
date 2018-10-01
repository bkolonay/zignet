USE ZigNet

DECLARE @applicationName varchar(MAX) = 'Showcase'
DECLARE @applicationNameAbbreviation varchar(MAX) = 'SC'
DECLARE @suiteName varchar(MAX) = 'Services' -- e.g. Services (D), Services, UI (D), UI
DECLARE @environmentName varchar(MAX) = 'Test' -- e.g. TestMain Debug, TestMain, Test Debug, Test

--INSERT INTO Applications (ApplicationName, ApplicationNameAbbreviation)
--VALUES (@applicationName, @applicationNameAbbreviation)

INSERT INTO Suites (SuiteName, ApplicationId, EnvironmentId)
SELECT 
	@suiteName, 
	(SELECT ApplicationID
	 FROM Applications WHERE ApplicationName = @applicationName),
	(SELECT EnvironmentID
	 FROM Environments WHERE EnvironmentName = @environmentName)

--SELECT * FROM Suites
--SELECT * FROM Environments
--SELECT * FROM Applications

SELECT SuiteID, ApplicationName AS App, ApplicationNameAbbreviation AS AppAbbr, SuiteName, EnvironmentNameAbbreviation AS EnvAbbr, EnvironmentName As Env
FROM Suites
INNER JOIN Applications
	ON Suites.ApplicationId = Applications.ApplicationID
INNER JOIN Environments
	ON Suites.EnvironmentId = Environments.EnvironmentID