CREATE TABLE [dbo].[ProviderReferenceStaging]
(
	[UkPrn] BIGINT NOT NULL,
	[Name] NVARCHAR(400) NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetUTCDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL,
	[ChecksumCol] AS BINARY_CHECKSUM([UkPrn], [Name])
	CONSTRAINT [PK_ProviderReferenceStaging] PRIMARY KEY ([UkPrn]),
)