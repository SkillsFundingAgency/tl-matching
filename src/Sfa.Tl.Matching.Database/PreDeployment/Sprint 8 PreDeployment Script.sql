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

if exists (select 1 from sys.columns where name = 'IsFundedForNextYear' and object_name(object_id) = 'Provider')
	ALTER TABLE [dbo].[Provider] DROP COLUMN [IsFundedForNextYear]
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

UPDATE [dbo].[ProviderVenue]
SET [IsEnabledForReferral] = [IsRemoved]
WHERE [IsEnabledForReferral] IS NULL

GO
ALTER TABLE [dbo].[ProviderVenue] ALTER COLUMN [IsEnabledForReferral] BIT NOT NULL
GO

ALTER TABLE Provider
ALTER COLUMN SecondaryContact nvarchar(100) NULL
GO

ALTER TABLE Provider
ALTER COLUMN SecondaryContactEmail varchar(320) NULL
GO