CREATE TABLE [dbo].[Provider]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UkPrn] BIGINT NOT NULL,
	[Name] NVARCHAR(400) NOT NULL,
	[DisplayName] NVARCHAR(400) NULL,
	[OfstedRating] INT NOT NULL,
	[PrimaryContact] NVARCHAR(100) NOT NULL,
	[PrimaryContactEmail] VARCHAR(320) NOT NULL,
	[PrimaryContactPhone] VARCHAR(150) NULL,
	[SecondaryContact] NVARCHAR(100) NULL,
	[SecondaryContactEmail] VARCHAR(320) NULL,
	[SecondaryContactPhone] VARCHAR(150) NULL,
	[IsCDFProvider] BIT NOT NULL,
	[IsEnabledForReferral] BIT NOT NULL,
	[IsTLevelProvider] BIT NOT NULL DEFAULT 0,
	[Source] VARCHAR(50) NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL
	CONSTRAINT [PK_Provider] PRIMARY KEY ([Id]),
)