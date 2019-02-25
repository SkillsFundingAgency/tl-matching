CREATE TABLE [dbo].[Opportunity]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[RouteId] INT NOT NULL, 
	[PostCode] VARCHAR(10) NOT NULL,
	[Distance] SMALLINT NOT NULL,
	[JobTitle] NVARCHAR(250) NULL,
	[PlacementsKnown] BIT NULL,
	[Placements] SMALLINT NULL,
	[EmployerCrmId] UNIQUEIDENTIFIER NULL,
	[EmployerName] NVARCHAR(250) NULL, 
	[EmployerAupa] NVARCHAR(10) NULL, 
	[EmployerOwner] NVARCHAR(150) NULL, 
	[Contact] NVARCHAR(100) NULL,
	[ContactEmail] VARCHAR(320) NULL,
	[ContactPhone] VARCHAR(150) NULL,
	[UserEmail] VARCHAR(320) NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL
	CONSTRAINT [PK_Opportunity] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Opportunity_Route] FOREIGN KEY ([RouteId]) REFERENCES [Route]([Id])
)