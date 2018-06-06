DELETE FROM Suites_Tests

-- dev, web services
INSERT INTO Suites_Tests
VALUES (1, 1)
INSERT INTO Suites_Tests
VALUES (1, 3)
INSERT INTO Suites_Tests
VALUES (1, 5)

-- test, web services
INSERT INTO Suites_Tests
VALUES (2, 1)
INSERT INTO Suites_Tests
VALUES (2, 3)
INSERT INTO Suites_Tests
VALUES (2, 5)

-- prod, web services
INSERT INTO Suites_Tests
VALUES (3, 1)
INSERT INTO Suites_Tests
VALUES (3, 3)
INSERT INTO Suites_Tests
VALUES (3, 5)

-- dev, ui
INSERT INTO Suites_Tests
VALUES (4, 2)
INSERT INTO Suites_Tests
VALUES (4, 4)
INSERT INTO Suites_Tests
VALUES (4, 6)

-- test, ui
INSERT INTO Suites_Tests
VALUES (5, 2)
INSERT INTO Suites_Tests
VALUES (5, 4)
INSERT INTO Suites_Tests
VALUES (5, 6)

-- prod, ui
INSERT INTO Suites_Tests
VALUES (6, 2)
INSERT INTO Suites_Tests
VALUES (6, 4)
INSERT INTO Suites_Tests
VALUES (6, 6)

SELECT Suites.SuiteID, Suites.SuiteName, Tests.TestID, Tests.TestName
FROM Suites_Tests
JOIN Suites
	ON Suites_Tests.SuiteId = Suites.SuiteID
JOIN Tests
	ON Suites_Tests.TestId = Tests.TestID