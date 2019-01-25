/*
 Pre-Deployment Script
*/

IF EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'RoutePathLookup')
	DROP TABLE [dbo].[RoutePathLookup]
