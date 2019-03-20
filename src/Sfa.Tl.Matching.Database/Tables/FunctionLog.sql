CREATE TABLE [dbo].[FunctionLog]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FunctionName] VARCHAR(20) NOT NULL,
	[RowNumber] int NOT NULL,
	[ErrorMessage] VARCHAR(4000) NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL, 
	CONSTRAINT [PK_FunctionLog] PRIMARY KEY ([Id])
)
