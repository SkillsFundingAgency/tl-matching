CREATE TABLE [dbo].[OpportunityEmailPlaceholder]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[OpportunityId] INT NOT NULL, 
	[EmailTemplateId] INT NOT NULL, 
	[Key] VARCHAR(50) NOT NULL, 
	[Value] NVARCHAR(500) NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL, 

    CONSTRAINT [PK_OpportunityEmailPlaceholder] PRIMARY KEY ([Id]),

	CONSTRAINT [FK_OpportunityEmailPlaceholder_Opportunity] FOREIGN KEY ([OpportunityId]) REFERENCES [Opportunity]([Id]),
	CONSTRAINT [FK_OpportunityEmailPlaceholder_EmailTemplate] FOREIGN KEY ([EmailTemplateId]) REFERENCES [EmailTemplate]([Id])
)
