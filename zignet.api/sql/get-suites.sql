SELECT ApplicationNameAbbreviation AS AppAbbr, SuiteName, EnvironmentNameAbbreviation AS EnvAbbr
FROM Suites
INNER JOIN Applications
	ON Suites.ApplicationId = Applications.ApplicationID
INNER JOIN Environments
	ON Suites.EnvironmentId = Environments.EnvironmentID