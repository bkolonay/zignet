SELECT * FROM Suites
SELECT * FROM SuiteResults
SELECT * FROM SuiteResultTypes

DELETE FROM SuiteResults

INSERT INTO SuiteResults
VALUES (1, '4/28/2019 3:00pm', '4/28/2019 3:05pm', 3)

SELECT SuiteResults.SuiteResultID, SuiteResults.SuiteId, Suites.SuiteName,
	SuiteResults.SuiteResultStartDateTime AS StartTime, SuiteResults.SuiteResultEndDateTime AS EndTime,
	SuiteResultTypes.SuiteResultTypeName
FROM SuiteResults
JOIN Suites
	ON SuiteResults.SuiteId = Suites.SuiteID
JOIN SuiteResultTypes
	ON SuiteResults.SuiteResultTypeId = SuiteResultTypes.SuiteResultTypeID