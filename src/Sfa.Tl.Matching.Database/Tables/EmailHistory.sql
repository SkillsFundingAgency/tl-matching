CREATE TABLE [dbo].[EmailHistory]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[ReferralId] INT NOT NULL, 
	[EmailTemplateId] INT NOT NULL, 
	[SentTo] NVARCHAR(500) NOT NULL, 
	[CopiedTo] NVARCHAR(500) NULL, 
	[BlindCopiedTo] NVARCHAR(500) NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL, 

    CONSTRAINT [PK_EmailHistory] PRIMARY KEY ([Id]),

	CONSTRAINT [FK_EmailHistory_Referral] FOREIGN KEY ([ReferralId]) REFERENCES [Referral]([Id]),
	CONSTRAINT [FK_EmailHistory_EmailTemplate] FOREIGN KEY ([EmailTemplateId]) REFERENCES [EmailTemplate]([Id])
)
