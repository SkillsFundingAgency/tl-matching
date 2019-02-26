CREATE TABLE [dbo].[QualificationRoutePathMapping]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[PathId] INT NOT NULL, 
	[QualificationId] INT NOT NULL, 
	[Source] VARCHAR(50) NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL
	CONSTRAINT [PK_RoutePathMapping] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_RoutePathMapping_Path] FOREIGN KEY ([PathId]) REFERENCES [Path]([Id]),
	CONSTRAINT [FK_RoutePathMapping_Qualification] FOREIGN KEY ([QualificationId]) REFERENCES [Qualification]([Id])
)
