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

INSERT INTO Environments
VALUES ('DevMain Debug', 'DVM (D)')

INSERT INTO Environments
VALUES ('TestMain Debug', 'TSM (D)')

INSERT INTO Environments
VALUES ('TestRelease Debug', 'TSR (D)')

INSERT INTO Environments
VALUES ('Production Debug', 'Prod (D)')

SELECT * FROM Environments