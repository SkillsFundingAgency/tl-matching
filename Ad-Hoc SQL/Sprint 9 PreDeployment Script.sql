/*
*  One-off script to be run before deploying Sprint 9 release
*
*  The RoutePathMapping table currently has PathId as Foreign Key. 
*  We need to remove this column and remap each qualification to RouteId.
*/

/* Before:
select qrpm.[Id]
      ,qrpm.[PathId]
	  ,p.[Name]
      ,qrpm.[QualificationId]
      ,qrpm.[Source]
from [dbo].[QualificationRoutePathMapping] qrpm
inner join [dbo].[Path] p
on p.[Id] = qrpm.[PathId]
order by qrpm.[Id]
*/

--Add RouteId column
if not exists (select 1 from sys.columns where name = 'RouteId' and object_name(object_id) = 'QualificationRoutePathMapping')
	ALTER TABLE [dbo].[QualificationRoutePathMapping]
		ADD [RouteId] INT NULL
GO

if exists(select 1 from sys.columns where name = 'PathId' and object_name(object_id) = 'QualificationRoutePathMapping')
	EXEC('update  [dbo].[QualificationRoutePathMapping] 
		  set [RouteId] = p.[RouteId]
		  from [dbo].[QualificationRoutePathMapping] qrpm
		  inner join [dbo].[Path] p
		  on p.[Id] = qrpm.[PathId]')
GO

--Collapse duplicate records 
;
WITH CTE (Id, RouteId, QualificationId, duplicateRecCount)
AS
(
SELECT	[Id], 
		[RouteId], 
		[QualificationId], 
		ROW_NUMBER() 
			OVER(
				PARTITION by [RouteId], [QualificationId] ORDER BY [Id]
				) AS duplicateRecCount
FROM [dbo].[QualificationRoutePathMapping]
)
--Now Delete Duplicate Rows
--SELECT * FROM CTE
DELETE FROM CTE
WHERE duplicateRecCount > 1 
--ORDER BY Id
GO

--Drop constraint FK_QualificationRoutePathMapping_Path
if exists (select 1 from sys.foreign_keys where name = 'FK_QualificationRoutePathMapping_Path' and object_name(parent_object_id) = 'QualificationRoutePathMapping')
	ALTER TABLE [dbo].[QualificationRoutePathMapping] DROP CONSTRAINT [FK_QualificationRoutePathMapping_Path]
GO

--Drop old PathId
if exists (select 1 from sys.columns where name = 'PathId' and object_name(object_id) = 'QualificationRoutePathMapping')
	ALTER TABLE [dbo].[QualificationRoutePathMapping] DROP COLUMN [PathId]
GO

--Make RouteId NOT NULL
if exists (select 1 from sys.columns where name = 'RouteId' and is_nullable = 1 and object_name(object_id) = 'QualificationRoutePathMapping')
	ALTER TABLE [dbo].[QualificationRoutePathMapping]
		ALTER COLUMN [RouteId] INT NOT NULL
GO

--Make RouteId a foreign key
if not exists (select 1 from sys.foreign_keys where name = 'FK_QualificationRoutePathMapping_Route' and object_name(parent_object_id) = 'QualificationRoutePathMapping')
BEGIN
	ALTER TABLE [dbo].[QualificationRoutePathMapping] WITH CHECK 
		ADD  CONSTRAINT [FK_QualificationRoutePathMapping_Route] FOREIGN KEY ([RouteId]) REFERENCES [Route]([Id])

	ALTER TABLE [dbo].[QualificationRoutePathMapping] CHECK CONSTRAINT [FK_QualificationRoutePathMapping_Route]
END
GO

/* After:
select qrpm.[Id]
      ,qrpm.[RouteId]
	  ,r.[Name]
      ,qrpm.[QualificationId]
      ,qrpm.[Source]
from [dbo].[QualificationRoutePathMapping] qrpm
left outer join [dbo].[Route] r
on r.[Id] = qrpm.[RouteId]
order by qrpm.[Id]
*/

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'ProviderFeedbackRequestHistory') 
	EXEC sp_rename 'ProviderFeedbackRequestHistory', 'BackgroundProcessHistory';
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'ProviderCount' AND object_name(object_id) = 'BackgroundProcessHistory')
	EXEC sp_rename 'BackgroundProcessHistory.ProviderCount', 'RecordCount', 'COLUMN';
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'Status' AND object_name(object_id) = 'BackgroundProcessHistory')
	EXEC sp_rename 'BackgroundProcessHistory.Status', 'ProcessType', 'COLUMN';
GO