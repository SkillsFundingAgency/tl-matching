/*
*  One-off script to be run before deploying Sprint 9 release
*
*  The RoutePathMapping table currently has PathId as Foreign Key. 
*  We need to remove this column and remap each qualification to RouteId.
*/

/*** Sprint 8 changes need to be run first ***/

--Provider Table Change

--Rename IsEnabledForSearch to IsCDFProvider
--IsEnabledForSearch = IsFundedForNextYear = New Column => IsCDFProvider

--Make Secondary Contact details Optional (Will be done as a change on the table in the DB project)

if exists (select 1 from sys.columns where name = 'IsEnabledForSearch' and object_name(object_id) = 'Provider') AND
not exists (select 1 from sys.columns where name = 'IsCDFProvider' and object_name(object_id) = 'Provider')
	EXEC sp_rename 'Provider.IsEnabledForSearch', 'IsCDFProvider', 'COLUMN';
GO

--Copy IsFundedForNextYear to IsCDFProvider and IsEnabledForReferral
if exists (select 1 from sys.columns where name = 'IsFundedForNextYear' and object_name(object_id) = 'Provider')
	EXEC('UPDATE [dbo].[Provider]
		 SET [IsCDFProvider] = [IsFundedForNextYear],
			[IsEnabledForReferral] = [IsFundedForNextYear]')
GO

if exists (select 1 from sys.columns where name = 'IsFundedForNextYear' and object_name(object_id) = 'Provider')
	ALTER TABLE [dbo].[Provider] DROP COLUMN [IsFundedForNextYear]
GO
if exists (select 1 from sys.columns where name = 'Status' and object_name(object_id) = 'Provider')
	ALTER TABLE [dbo].[Provider] DROP COLUMN [Status]
GO
if exists (select 1 from sys.columns where name = 'StatusReason' and object_name(object_id) = 'Provider')
	ALTER TABLE [dbo].[Provider] DROP COLUMN [StatusReason]
GO

--Venue Table Changes

--Rename IsEnabledForSearch to IsRemoved
--New Column => IsEnabledForReferral

if  exists (select 1 from sys.columns where name = 'IsEnabledForSearch' and object_name(object_id) = 'ProviderVenue') AND
not exists (select 1 from sys.columns where name = 'IsRemoved' and object_name(object_id) = 'ProviderVenue')
	EXEC sp_rename 'ProviderVenue.IsEnabledForSearch', 'IsRemoved', 'COLUMN';
GO

if not exists (select 1 from sys.columns where name = 'IsEnabledForReferral' and object_name(object_id) = 'ProviderVenue')
	ALTER TABLE [dbo].[ProviderVenue] ADD [IsEnabledForReferral] BIT NULL
GO

--Reverse IsRemoved - make sure this is only run once!
if exists (select 1 from [dbo].[ProviderVenue] where [IsEnabledForReferral] IS NULL) 
BEGIN
	UPDATE [dbo].[ProviderVenue]
	SET [IsRemoved] = CASE WHEN [IsRemoved] = 0 THEN 1 ELSE 0 END

	UPDATE [dbo].[ProviderVenue]
	SET [IsEnabledForReferral] = CASE WHEN [IsRemoved] = 0 THEN 1 ELSE 0 END
	WHERE [IsEnabledForReferral] IS NULL
END

if exists (select 1 from sys.columns where name = 'IsEnabledForReferral' and is_nullable = 1 and object_name(object_id) = 'ProviderVenue')
	ALTER TABLE [dbo].[ProviderVenue] 
	ALTER COLUMN [IsEnabledForReferral] BIT NOT NULL
GO

if exists (select 1 from sys.columns where name = 'SecondaryContact' and is_nullable = 0 and object_name(object_id) = 'Provider')
	ALTER TABLE Provider
	ALTER COLUMN SecondaryContact nvarchar(100) NULL
GO

if exists (select 1 from sys.columns where name = 'SecondaryContactEmail' and is_nullable = 0 and object_name(object_id) = 'Provider')
	ALTER TABLE Provider
	ALTER COLUMN SecondaryContactEmail varchar(320) NULL
GO

--Change Status column in ProviderFeedbackRequestHistory to string
if exists (select 1 from sys.columns where name = 'Status' and system_type_id = 52 and object_name(object_id) = 'ProviderFeedbackRequestHistory') AND
   not exists (select 1 from sys.columns where name = 'Status_String' and object_name(object_id) = 'ProviderFeedbackRequestHistory')
	ALTER TABLE [ProviderFeedbackRequestHistory]
	ADD	[Status_String] VARCHAR(10) NULL
GO

if exists (select 1 from sys.columns where name = 'Status' and system_type_id = 52 and object_name(object_id) = 'ProviderFeedbackRequestHistory') AND
   exists (select 1 from sys.columns where name = 'Status_String' and object_name(object_id) = 'ProviderFeedbackRequestHistory')
   
	EXEC('UPDATE [dbo].[ProviderFeedbackRequestHistory]
	SET [Status_String] = 
		CASE
			WHEN [Status] = 1 THEN ''Pending''
			WHEN [Status] = 2 THEN ''Processing''
			WHEN [Status] = 3 THEN ''Complete''
			WHEN [Status] = 4 THEN ''Error''
		END')
GO

if exists (select 1 from sys.columns where name = 'Status_String' and object_name(object_id) = 'ProviderFeedbackRequestHistory')
	ALTER TABLE [ProviderFeedbackRequestHistory]
	ALTER COLUMN [Status_String] VARCHAR(10) NOT NULL
GO

if exists (select 1 from sys.columns where name = 'Status' and system_type_id = 52 and object_name(object_id) = 'ProviderFeedbackRequestHistory') AND
   exists (select 1 from sys.columns where name = 'Status_String' and object_name(object_id) = 'ProviderFeedbackRequestHistory')ALTER TABLE [ProviderFeedbackRequestHistory]
	DROP COLUMN [Status]
GO

if not exists (select 1 from sys.columns where name = 'Status' and object_name(object_id) = 'ProviderFeedbackRequestHistory') AND
   exists (select 1 from sys.columns where name = 'Status_String' and object_name(object_id) = 'ProviderFeedbackRequestHistory')
	EXEC sp_rename 'ProviderFeedbackRequestHistory.Status_String', 'Status', 'COLUMN';	
GO


/*** Sprint 9 changes below here ***/

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

IF EXISTS (select 1 from sys.key_constraints where name = 'PK_ProviderFeedbackRequestHistory' and object_name(parent_object_id) = 'BackgroundProcessHistory') 
	EXEC sp_rename @objname = N'PK_ProviderFeedbackRequestHistory', @newname = N'PK_BackgroundProcessHistory'
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'ProviderCount' AND object_name(object_id) = 'BackgroundProcessHistory')
	EXEC sp_rename 'BackgroundProcessHistory.ProviderCount', 'RecordCount', 'COLUMN';
GO

if not exists (select 1 from sys.columns where name = 'ProcessType' and object_name(object_id) = 'BackgroundProcessHistory')
	ALTER TABLE [dbo].[BackgroundProcessHistory]
		ADD [ProcessType] VARCHAR(50) NULL
GO

if exists(select 1 from sys.columns where name = 'ProcessType' and object_name(object_id) = 'BackgroundProcessHistory')
	EXEC('UPDATE BackgroundProcessHistory
	      SET ProcessType = ''ProviderFeedbackRequest''
          WHERE ProcessType IS NULL')
GO

if exists (select 1 from sys.columns where name = 'ProcessType' and is_nullable = 1 and object_name(object_id) = 'BackgroundProcessHistory')
	ALTER TABLE [dbo].[BackgroundProcessHistory]
		ALTER COLUMN [ProcessType] VARCHAR(50) NOT NULL
GO