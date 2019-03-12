CREATE TABLE [dbo].[Referral]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[OpportunityId] INT NOT NULL,
	[ProviderVenueId] INT NOT NULL,
	[DistanceFromEmployer] DECIMAL(18, 2),
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL
	CONSTRAINT [PK_Referral] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Referral_Opportunity] FOREIGN KEY ([OpportunityId]) REFERENCES [Opportunity]([Id]),
	CONSTRAINT [FK_Referral_ProviderVenue] FOREIGN KEY ([ProviderVenueId]) REFERENCES [ProviderVenue]([Id])
)