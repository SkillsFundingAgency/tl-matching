CREATE TABLE [dbo].[PostcodeLookupStaging]
(
	[Postcode] VARCHAR(10) NOT NULL,
	[LepCode] VARCHAR(10) NULL,
	[CreatedBy] NVARCHAR(50) NULL, 
	[ChecksumCol] AS BINARY_CHECKSUM([Postcode], [LepCode])
)
