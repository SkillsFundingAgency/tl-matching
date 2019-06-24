﻿CREATE TABLE [dbo].[Route]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
	[Name] NVARCHAR(50) NOT NULL, 
	[Keywords] NVARCHAR(500) NULL, 
	[Summary] NVARCHAR(500) NULL, 
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL
	CONSTRAINT [PK_Route] PRIMARY KEY ([Id]),
)
