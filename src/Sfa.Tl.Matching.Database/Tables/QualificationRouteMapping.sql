CREATE TABLE [dbo].[QualificationRouteMapping]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[RouteId] INT NOT NULL, 
	[QualificationId] INT NOT NULL, 
	[Source] VARCHAR(50) NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL
	CONSTRAINT [PK_QualificationRouteMapping] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_QualificationRouteMapping_Route] FOREIGN KEY ([RouteId]) REFERENCES [Route]([Id]),
	CONSTRAINT [FK_QualificationRouteMapping_Qualification] FOREIGN KEY ([QualificationId]) REFERENCES [Qualification]([Id])
)
