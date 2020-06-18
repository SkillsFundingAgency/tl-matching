CREATE TABLE [dbo].[EmailHistory]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[NotificationId] UNIQUEIDENTIFIER NULL, 
	[EmailTemplateId] INT NOT NULL, 
	[OpportunityId] INT NULL, 
	[OpportunityItemId] INT NULL, 
	[SentTo] NVARCHAR(500) NOT NULL, 
	[Status] NVARCHAR(100) NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL, 

    CONSTRAINT [PK_EmailHistory] PRIMARY KEY ([Id]),

	CONSTRAINT [FK_EmailHistory_Opportunity] FOREIGN KEY ([OpportunityId]) REFERENCES [Opportunity]([Id]),
	CONSTRAINT [FK_EmailHistory_EmailTemplate] FOREIGN KEY ([EmailTemplateId]) REFERENCES [EmailTemplate]([Id])
)
