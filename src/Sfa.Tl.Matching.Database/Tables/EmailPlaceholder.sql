CREATE TABLE [dbo].[EmailPlaceholder]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[EmailHistoryId] INT NOT NULL, 
	[Key] VARCHAR(50) NOT NULL, 
	[Value] NVARCHAR(MAX) NOT NULL, 
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL, 

    CONSTRAINT [PK_EmailPlaceholder] PRIMARY KEY ([Id]),

	CONSTRAINT [FK_EmailPlaceholder_EmailHistory] FOREIGN KEY ([EmailHistoryId]) REFERENCES [EmailHistory]([Id]),
)
