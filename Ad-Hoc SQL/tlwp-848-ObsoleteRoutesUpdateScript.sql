-- Below script is for Production Data only, Should be uncommented before running on Production
/*
-- Replace 'Sales, marketing and procurement' With 'Business and administration'
UPDATE [dbo].[OpportunityItem] 
SET RouteId = 2
WHERE Id IN (2226, 5890, 2477, 5666, 5683) AND RouteId = 14

Go
-- Replace 'Care services' With 'Education and childcare'
UPDATE [dbo].[OpportunityItem] 
SET RouteId = 7
WHERE Id IN (5605, 4133) AND RouteId = 12

Go
-- Replace 'Care services' With 'Health and science'
UPDATE [dbo].[OpportunityItem] 
SET RouteId = 10
WHERE Id IN (5472, 5477, 458, 851, 5559,5441, 5452, 5574, 1467, 4104,5550) AND RouteId = 12

Go

-- Replace 'Care services' With 'Hospitality and catering'
UPDATE [dbo].[OpportunityItem] 
SET RouteId = 3
WHERE Id IN (3565, 2092) AND RouteId = 12

Go
*/

--Set the status of the Obsolete routes
Update [dbo].[Route]
SET IsDeleted = 1
WHERE Id IN (12, 13, 14, 15)

Go

--Update Qualification Delete Status
CREATE TABLE #Qualification
(
	QualificationId INT
)

Go

WITH QualificationObsoleteRouteCount (
    QualificationId, 
    RouteId
)
AS (
Select QualificationId, RouteId 
FROM [dbo].[QualificationRouteMapping]
WHERE RouteId IN (SELECT Id FROM [dbo].[Route] WHERE [IsDeleted] = 1)
),
QualificationTotalObsoleteRouteCount(QualificationId, ObsoleteRouteCount)
AS (
Select QualificationId, Count(RouteId) 
FROM QualificationObsoleteRouteCount GROUP BY QualificationId
),
QualificationAllRouteCount(QualificationId, AllRouteCount)
AS (
Select QRM.QualificationId, Count(QRM.RouteId) 
FROM [dbo].[QualificationRouteMapping] QRM
INNER JOIN QualificationTotalObsoleteRouteCount QTORC ON QRM.QualificationId = QTORC.QualificationId
GROUP BY QRM.QualificationId
),
QualificationToDelete(QualificationId)
AS (SELECT Distinct QORC.QualificationId
FROM QualificationTotalObsoleteRouteCount  QORC
LEFT JOIN QualificationAllRouteCount QARC ON QORC.QualificationId = QARC.QualificationId
WHERE QORC.ObsoleteRouteCount = QARC.AllRouteCount
)

INSERT INTO #Qualification
SELECT QualificationId FROM QualificationToDelete

UPDATE [dbo].[Qualification]
SET IsDeleted = 1
WHERE [Id] IN (SELECT QualificationId FROM #Qualification)

UPDATE [dbo].[ProviderQualification]
SET IsDeleted = 1
WHERE QualificationId IN (SELECT QualificationId FROM #Qualification)

DROP TABLE #Qualification

Go

--Update OpportunityItem Delete Status

UPDATE [dbo].[OpportunityItem]
SET IsDeleted = 1
WHERE IsSaved = 1 AND IsCompleted = 0
AND RouteId IN (SELECT Id FROM [dbo].[Route] WHERE [IsDeleted] = 1)

Go