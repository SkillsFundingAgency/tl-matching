CREATE TABLE [dbo].[LocalEnterprisePartnershipStaging]
(
	[Code] VARCHAR(10) NOT NULL,
	[Name] VARCHAR(100) NOT NULL,
	[CreatedBy] NVARCHAR(50) NULL, 
	[ChecksumCol] AS BINARY_CHECKSUM([Code], [Name])
)
