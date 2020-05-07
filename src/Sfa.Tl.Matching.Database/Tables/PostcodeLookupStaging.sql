CREATE TABLE [dbo].[PostcodeLookupStaging]
(
	[Postcode] VARCHAR(10) NOT NULL,
	[PrimaryLepCode] VARCHAR(10) NULL,
	[SecondaryLepCode] VARCHAR(10) NULL,
	[CreatedBy] NVARCHAR(50) NULL, 
	[ChecksumCol] AS BINARY_CHECKSUM([Postcode], [PrimaryLepCode], [SecondaryLepCode])
)
