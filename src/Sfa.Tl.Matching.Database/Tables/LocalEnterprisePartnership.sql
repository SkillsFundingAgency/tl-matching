CREATE TABLE [dbo].[LocalEnterprisePartnership]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] VARCHAR(10) NOT NULL,
	[Name] VARCHAR(100) NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL,
	[ChecksumCol] AS BINARY_CHECKSUM([Code], [Name])
	CONSTRAINT [PK_LocalEnterprisePartnership] PRIMARY KEY ([Id])
)
