CREATE TABLE [dbo].[ProvisionGap]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[OpportunityId] INT NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL
	CONSTRAINT [PK_ProvisionGap] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_ProvisionGap_Opportunity] FOREIGN KEY ([OpportunityId]) REFERENCES [Opportunity]([Id])
)
