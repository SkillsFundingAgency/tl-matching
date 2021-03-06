﻿CREATE TABLE [dbo].[Qualification]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[LarId] NVARCHAR(8) NOT NULL,
	[Title] NVARCHAR(400) NOT NULL, 
	[ShortTitle] NVARCHAR(100) NULL,
	[QualificationSearch] NVARCHAR(350) NULL,
	[ShortTitleSearch] NVARCHAR(100) NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL
	CONSTRAINT [PK_Qualification] PRIMARY KEY ([Id]), 
    [IsDeleted] BIT NOT NULL DEFAULT 0,
)
