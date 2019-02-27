/*
Post-Deployment Script
*/

:r ".\Seed Routes.sql"
:r ".\Seed Paths.sql"

--Remove obsolete tables
IF EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'RoutePathMapping')
	DROP TABLE [dbo].[RoutePathMapping]
