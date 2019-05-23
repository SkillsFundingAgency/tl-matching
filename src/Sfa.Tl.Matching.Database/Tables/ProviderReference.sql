CREATE TABLE [dbo].[ProviderReference]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UkPrn] BIGINT NOT NULL,
	[Name] NVARCHAR(400) NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetUTCDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL,
	ChecksumCol AS BINARY_CHECKSUM([UkPrn], [Name])
)