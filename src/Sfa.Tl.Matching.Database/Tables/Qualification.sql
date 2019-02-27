CREATE TABLE [dbo].[Qualification]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[LarsId] NVARCHAR(8) NOT NULL,
	[Title] NVARCHAR(250) NOT NULL, 
	[ShortTitle] NVARCHAR(100) NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL
	CONSTRAINT [PK_Qualification] PRIMARY KEY ([Id]),
)
