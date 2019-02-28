CREATE TABLE [dbo].[ProvisionGap]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[OpportunityId] INT NOT NULL,
	[ConfirmationSelected] BIT Null,
	CONSTRAINT [PK_ProvisionGap] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_ProvisionGap_Opportunity] FOREIGN KEY ([OpportunityId]) REFERENCES [Opportunity]([Id])
)
