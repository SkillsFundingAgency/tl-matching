CREATE TABLE [dbo].[Opportunity]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[RouteId] INT NOT NULL, 
	[PostCode] VARCHAR(10) NOT NULL,
	[Distance] SMALLINT NOT NULL,
	[JobTitle] NVARCHAR(250) NULL,
	[PlacementsKnown] BIT NULL,
	[Placements] INT NULL,
	[EmployerName] NVARCHAR(250) NULL, 
	[EmployerContact] NVARCHAR(100) NULL,
	[EmployerContactEmail] VARCHAR(320) NULL,
	[EmployerContactPhone] VARCHAR(150) NULL,
	[UserEmail] VARCHAR(320) NULL,
	[DropOffStage] SMALLINT NULL,
	[SearchResultProviderCount] INT NULL,
	[ConfirmationSelected] BIT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL
	CONSTRAINT [PK_Opportunity] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Opportunity_Route] FOREIGN KEY ([RouteId]) REFERENCES [Route]([Id])
)