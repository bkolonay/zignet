--DROP TABLE Applications

--SELECT *
--INTO dbo.Applications
--FROM [ZigNet].[US\bkolonay].[Applications]

DROP TABLE Applications

CREATE TABLE dbo.Applications (
	ApplicationID int NOT NULL PRIMARY KEY IDENTITY(1,1),
	ApplicationName NVARCHAR(MAX) NOT NULL,
	ApplicationNameAbbreviation NVARCHAR(MAX) NOT NULL,
)

INSERT INTO Applications
VALUES ('LoopNet', 'LN')

INSERT INTO Applications
VALUES ('Listing Manager Mobile', 'LM Mobile')

INSERT INTO Applications
VALUES ('CityFeet', 'CF')

SELECT * FROM Applications