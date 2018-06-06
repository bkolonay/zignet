--DROP TABLE Applications

ALTER TABLE LatestTestResults
ADD SuiteName NVARCHAR(MAX) NOT NULL
CONSTRAINT DF_LatestTestResultsDefaultSuiteName DEFAULT ''

ALTER TABLE LatestTestResults
DROP CONSTRAINT DF_LatestTestResultsDefaultSuiteName

SELECT * FROM LatestTestResults