/*
Post-Deployment Script
*/

:r ".\Seed Routes.sql"
:r ".\Seed Paths.sql"

--Remove obsolete tables
IF EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'IndustryPlacement')
	DROP TABLE [dbo].[IndustryPlacement]
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'ProviderCourses')
	DROP TABLE [dbo].[ProviderCourses]
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'Address')
	DROP TABLE [dbo].[Address]
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'LocalAuthorityMapping')
	DROP TABLE [dbo].[LocalAuthorityMapping]
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'NotificationHistory')
	DROP TABLE [dbo].[NotificationHistory]
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'Contact')
	DROP TABLE [dbo].[Contact]
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'RoutePath')
	DROP TABLE [dbo].[RoutePath]
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'Course')
	DROP TABLE [dbo].[Course]
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'TemplatePlaceholder')
	DROP TABLE [dbo].[TemplatePlaceholder]
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'EmailTemplate')
	DROP TABLE [dbo].[EmailTemplate]
