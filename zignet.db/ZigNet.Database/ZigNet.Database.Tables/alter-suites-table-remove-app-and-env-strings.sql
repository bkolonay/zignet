-- update LN service suite names (non-debug)
--UPDATE Suites
--SET SuiteName = 'Services'
--WHERE SuiteName NOT LIKE '%(D)%'
--	AND SuiteName LIKE '%Services%'

SELECT SuiteName, ApplicationNameAbbreviation AS AppAbbr, EnvironmentNameAbbreviation AS EnvAbbr
FROM Suites
INNER JOIN Applications
	ON Suites.ApplicationId = Applications.ApplicationID
INNER JOIN Environments
	ON Suites.EnvironmentId = Environments.EnvironmentID
WHERE SuiteName NOT LIKE '%(D)%'
	AND SuiteName LIKE '%Services%'

-- update LN service suite names (debug)
--UPDATE Suites
--SET SuiteName = 'Services (D)'
--WHERE SuiteName LIKE '%(D)%'
--	AND SuiteName LIKE '%Services%'

SELECT SuiteName, ApplicationNameAbbreviation AS AppAbbr, EnvironmentNameAbbreviation AS EnvAbbr
FROM Suites
INNER JOIN Applications
	ON Suites.ApplicationId = Applications.ApplicationID
INNER JOIN Environments
	ON Suites.EnvironmentId = Environments.EnvironmentID
WHERE SuiteName LIKE '%(D)%'
	AND SuiteName LIKE '%Services%'

-- update remaining suites
--UPDATE Suites
--SET SuiteName = 'UI'
--WHERE SuiteName = 'LN UI - TSM'
--UPDATE Suites
--SET SuiteName = 'UI (D)'
--WHERE SuiteName = 'LN UI - TSM (D)'
--UPDATE Suites
--SET SuiteName = 'Services'
--WHERE SuiteName = 'LM Mobile - DVM'
--UPDATE Suites
--SET SuiteName = 'Services (D)'
--WHERE SuiteName = 'LM Mobile - DVM (D)'

SELECT SuiteID, ApplicationName AS App, ApplicationNameAbbreviation AS AppAbbr, SuiteName, EnvironmentNameAbbreviation AS EnvAbbr, EnvironmentName AS Env
FROM Suites
INNER JOIN Applications
	ON Suites.ApplicationId = Applications.ApplicationID
INNER JOIN Environments
	ON Suites.EnvironmentId = Environments.EnvironmentID

SELECT * FROM SuiteResults
WHERE SuiteResultID > 24840
--ORDER BY SuiteResultID DESC

SELECT * FROM Environments