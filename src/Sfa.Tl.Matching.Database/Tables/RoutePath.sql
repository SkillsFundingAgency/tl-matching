﻿CREATE TABLE [dbo].[RoutePath]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
	[CourseId] UNIQUEIDENTIFIER NOT NULL, 
	[RoutePathLookupId] INT NOT NULL, 
	[Summary] NVARCHAR(50) NULL, 
	[Keywords] NVARCHAR(50) NULL, 
	[CreatedOn] DATETIME2 NULL DEFAULT GetDate(), 
	[ModifiedOn] DATETIME2 NULL, 
	CONSTRAINT [FK_RoutePath_Course] FOREIGN KEY ([CourseId]) REFERENCES [Course]([Id]), 
	CONSTRAINT [FK_RoutePath_RoutePathLookup] FOREIGN KEY ([RoutePathLookupId]) REFERENCES [RoutePathLookup]([Id])
)
