/*
Post-Deployment Script
*/

:r ".\Seed Routes.sql"
:r ".\Seed Paths.sql"

IF EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'RoutePathLookup')
	DROP TABLE [dbo].[RoutePathLookup]
