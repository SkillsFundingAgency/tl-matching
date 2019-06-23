CREATE TABLE [dbo].[ProvisionGap]
(
	[Id]					INT IDENTITY(1,1) NOT NULL, 
	[OpportunityItemId]		INT NOT NULL,
	[NoSuitableStudent]		BIT NULL,
	[HadBadExperience]		BIT NULL,
	[ProviderTooFarAway]	BIT NULL,
	[CreatedOn]				DATETIME2 NOT NULL DEFAULT getutcdate(), 
	[CreatedBy]				NVARCHAR(50) NULL, 
	[ModifiedOn]			DATETIME2 NULL, 
	[ModifiedBy]			NVARCHAR(50) NULL
	CONSTRAINT [PK_ProvisionGap] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_ProvisionGap_OpportunityItem] FOREIGN KEY ([OpportunityItemId]) REFERENCES [OpportunityItem]([Id])
)
