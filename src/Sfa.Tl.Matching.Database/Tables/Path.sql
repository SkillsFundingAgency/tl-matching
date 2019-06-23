CREATE TABLE [dbo].[Path]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
	[RouteId] INT NOT NULL, 
	[Name] NVARCHAR(50) NOT NULL, 
	[Keywords] NVARCHAR(500) NULL, 
	[Summary] NVARCHAR(500) NULL, 
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_Path] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Path_Route] FOREIGN KEY ([RouteId]) REFERENCES [Route]([Id])
)
