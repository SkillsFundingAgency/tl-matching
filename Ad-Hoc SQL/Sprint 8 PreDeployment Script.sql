/*
*  One-off script to be run before deploying Sprint 8 release
*/

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
		 SET [IsCDFProvider] = [IsFundedForNextYear]')
GO

EXEC('UPDATE [dbo].[Provider]
      Set [IsEnabledForReferral] = [IsCDFProvider]')
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

ALTER TABLE [dbo].[ProviderVenue] ALTER COLUMN [IsEnabledForReferral] BIT NOT NULL
GO

ALTER TABLE Provider
ALTER COLUMN SecondaryContact nvarchar(100) NULL
GO

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
