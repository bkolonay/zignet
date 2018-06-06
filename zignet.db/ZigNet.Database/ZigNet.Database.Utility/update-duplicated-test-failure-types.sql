--SELECT *
--INTO TestResult_TestFailureType_COPY
--FROM TestResult_TestFailureType
--DROP TABLE TestResult_TestFailureType_COPY

SELECT * FROM TestResult_TestFailureType
SELECT * FROM TestFailureTypes

SELECT TestResult_TestFailureType.TestFailureTypeId, TestFailureTypes.TestFailureTypeName
--UPDATE TestResult_TestFailureType
--SET TestFailureTypeId = 1
FROM TestResult_TestFailureType
INNER JOIN TestFailureTypes
	ON TestResult_TestFailureType.TestFailureTypeId = TestFailureTypes.TestFailureTypeID
WHERE TestFailureTypes.TestFailureTypeName = 'Assertion'

SELECT TestResult_TestFailureType.TestFailureTypeId, TestFailureTypes.TestFailureTypeName
--UPDATE TestResult_TestFailureType
--SET TestFailureTypeId = 2
FROM TestResult_TestFailureType
INNER JOIN TestFailureTypes
	ON TestResult_TestFailureType.TestFailureTypeId = TestFailureTypes.TestFailureTypeID
WHERE TestFailureTypes.TestFailureTypeName = 'Exception'

SELECT * FROM TestFailureTypes
--DELETE TestFailureTypes
--WHERE TestFailureTypeID > 2