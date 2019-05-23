CREATE TABLE [dbo].[ProviderReferenceStaging]
(
	[UkPrn] BIGINT NOT NULL,
	[Name] NVARCHAR(400) NOT NULL,
	ChecksumCol AS BINARY_CHECKSUM([UkPrn], [Name])
	CONSTRAINT [PK_ProviderReferenceStaging] PRIMARY KEY ([UkPrn]),
)