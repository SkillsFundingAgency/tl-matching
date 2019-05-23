CREATE TABLE [dbo].[ProviderReferenceStaging]
(
	[UkPrn] BIGINT NOT NULL,
	[Name] NVARCHAR(400) NOT NULL,
	ChecksumCol AS BINARY_CHECKSUM([UkPrn], [Name])
)