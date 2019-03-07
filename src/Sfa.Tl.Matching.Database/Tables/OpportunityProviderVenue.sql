CREATE TABLE [dbo].[OpportunityProviderVenue]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[ReferralId] INT NOT NULL, 
	[ProviderVenueId] INT NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL
	CONSTRAINT [PK_OpportunityProvider] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_OpportunityProvider_ProviderVenue] FOREIGN KEY ([ProviderVenueId]) REFERENCES [ProviderVenue]([Id]),
	CONSTRAINT [FK_OpportunityProvider_Referral] FOREIGN KEY ([ReferralId]) REFERENCES [Referral]([Id])
)