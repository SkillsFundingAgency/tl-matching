CREATE TABLE [dbo].[ProviderFeedbackRequestHistory]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[ProviderCount] INT NOT NULL,
	[Status] SMALLINT NOT NULL,
	[StatusMessage] VARCHAR(4000) NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_ProviderFeedbackRequestHistory] PRIMARY KEY ([Id]),
)
