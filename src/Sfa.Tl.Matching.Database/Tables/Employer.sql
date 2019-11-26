CREATE TABLE [dbo].[Employer]
(
	[Id]				INT IDENTITY(1,1) NOT NULL,
	[CrmId]				uniqueidentifier NOT NULL,
	[CompanyName]		NVARCHAR(160) NOT NULL, 
	[AlsoKnownAs]		NVARCHAR(100) NULL, 
	[CompanyNameSearch] NVARCHAR(260) NULL,
	[Aupa]				NVARCHAR(10) NULL, 
	--TODO: Delete [CompanyType] after sprint 20 release
	[CompanyType]		NVARCHAR(100) NULL, 
	[PrimaryContact]	NVARCHAR(100) NULL, 
	[Phone]				VARCHAR(150) NULL,
	[Email]				VARCHAR(320) NULL,
	--TODO: Delete [Postcode] after sprint 20 release
	[Postcode]			VARCHAR(10) NULL,
	[Owner]				NVARCHAR(150) NOT NULL,
	[CreatedOn]			DATETIME2 NOT NULL DEFAULT getutcdate(), 
	[CreatedBy]			NVARCHAR(50) NULL, 
	[ModifiedOn]		DATETIME2 NULL, 
	[ModifiedBy]		NVARCHAR(50) NULL, 
	[ChecksumCol] AS BINARY_CHECKSUM([CrmId], [CompanyName], [AlsoKnownAs], [CompanyNameSearch], [Aupa], [PrimaryContact], [Phone], [Email], [Owner])
    CONSTRAINT [PK_Employer] PRIMARY KEY ([Id])
)