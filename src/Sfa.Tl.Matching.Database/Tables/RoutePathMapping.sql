﻿CREATE TABLE [dbo].[RoutePathMapping]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[LarsId] NVARCHAR(8) NOT NULL,
	[Title] NVARCHAR(250) NOT NULL, 
	[ShortTitle] NVARCHAR(50) NULL, 
	[PathId] INT NOT NULL, 
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[ModifiedOn] DATETIME2 NULL,

	CONSTRAINT [FK_RoutePathMapping_Path] FOREIGN KEY ([PathId]) REFERENCES [Path]([Id])
)
