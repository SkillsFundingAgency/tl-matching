CREATE TABLE [dbo].[OpportunityItem]
(
	[Id]						INT IDENTITY(1,1) NOT NULL, 
	[OpportunityId]				INT NOT NULL, 
	[RouteId]					INT NOT NULL, 
	[OpportunityType]			NVARCHAR(50) NULL,
	[Town]						VARCHAR(100) NULL, 
	[Postcode]					VARCHAR(10) NOT NULL,
	[SearchRadius]				SMALLINT NOT NULL DEFAULT(0),
	[SearchResultProviderCount] INT NULL,
	[JobRole]					NVARCHAR(250) NULL,
	[PlacementsKnown]			BIT NULL,
	[Placements]				INT NULL,
	[IsSaved]					BIT NOT NULL DEFAULT 0,
	[IsSelectedForReferral]		BIT NOT NULL DEFAULT 0,
	[IsCompleted]				BIT NOT NULL DEFAULT 0,
	[EmployerFeedbackSent]		BIT NOT NULL DEFAULT 0,
	[CreatedOn]					DATETIME2 NOT NULL DEFAULT getutcdate(), 
	[CreatedBy]					NVARCHAR(50) NULL, 
	[ModifiedOn]				DATETIME2 NULL, 
	[ModifiedBy]				NVARCHAR(50) NULL
	CONSTRAINT [PK_OpportunityItem] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_OpportunityItem_Opportunity] FOREIGN KEY ([OpportunityId]) REFERENCES [Opportunity]([Id]),
	CONSTRAINT [FK_OpportunityItem_Route] FOREIGN KEY ([RouteId]) REFERENCES [Route]([Id])
)