CREATE TABLE [dbo].[Referral]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[OpportunityItemId] INT NOT NULL,
	[ProviderVenueId] INT NOT NULL,
	[DistanceFromEmployer] DECIMAL(18, 2) NOT NULL,
	[JourneyTimeByCar] [int] NULL,
	[JourneyTimeByPublicTransport] [int] NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL
	CONSTRAINT [PK_Referral] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Referral_OpportunityItem] FOREIGN KEY ([OpportunityItemId]) REFERENCES [OpportunityItem]([Id]),
	CONSTRAINT [FK_Referral_ProviderVenue] FOREIGN KEY ([ProviderVenueId]) REFERENCES [ProviderVenue]([Id])
)