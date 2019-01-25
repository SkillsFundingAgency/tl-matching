﻿CREATE TABLE [dbo].[Path]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
	[RouteId] INT NOT NULL, 
	[Name] NVARCHAR(50) NOT NULL, 
	[Keywords] NVARCHAR(500) NULL, 
	[Summary] NVARCHAR(500) NULL, 
	[CreatedOn] DATETIME2 NULL DEFAULT GetDate(), 
	[CreatedBy] NVARCHAR(50) NOT NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL,
	
	CONSTRAINT [FK_Path_Route] FOREIGN KEY ([RouteId]) REFERENCES [Route]([Id])
)
