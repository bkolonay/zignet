--DROP TABLE Environments

CREATE TABLE Environments (
	EnvironmentID int NOT NULL PRIMARY KEY IDENTITY(1,1),
	EnvironmentName NVARCHAR(MAX) NOT NULL,
	EnvironmentNameAbbreviation NVARCHAR(MAX) NOT NULL,
)

INSERT INTO Environments
VALUES ('DevMain', 'DVM')

INSERT INTO Environments
VALUES ('TestMain', 'TSM')

INSERT INTO Environments
VALUES ('TestRelease', 'TSR')

INSERT INTO Environments
VALUES ('Production', 'Prod')

INSERT INTO Environments
VALUES ('Development', 'Dev')

SELECT * FROM Environments