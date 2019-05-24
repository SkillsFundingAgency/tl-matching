CREATE TABLE [dbo].[BackgroundProcessHistory]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[RecordCount] INT NOT NULL,
	[Status] VARCHAR(10) NOT NULL,
	[StatusMessage] VARCHAR(4000) NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_BackgroundProcessHistory] PRIMARY KEY ([Id]),
)