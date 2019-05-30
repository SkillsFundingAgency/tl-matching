CREATE TABLE [dbo].[QualificationRoutePathMapping]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[RouteId] INT NOT NULL, 
	[QualificationId] INT NOT NULL, 
	[Source] VARCHAR(50) NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL
	CONSTRAINT [PK_QualificationRoutePathMapping] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_QualificationRoutePathMapping_Route] FOREIGN KEY ([RouteId]) REFERENCES [Route]([Id]),
	CONSTRAINT [FK_QualificationRoutePathMapping_Qualification] FOREIGN KEY ([QualificationId]) REFERENCES [Qualification]([Id])
)
