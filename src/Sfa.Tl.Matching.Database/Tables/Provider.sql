CREATE TABLE [dbo].[Provider]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[UkPrn] BIGINT NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	[OfstedRating] INT NOT NULL,
	[Active] bit NOT NULL,
	[ActiveReason] NVARCHAR(30) NULL,
	[PrimaryContact] NVARCHAR(100) NOT NULL,
	[PrimaryContactEmail] VARCHAR(320) NOT NULL,
	[PrimaryContactPhone] VARCHAR(150) NULL,
	[SecondaryContact] NVARCHAR(100) NOT NULL,
	[SecondaryContactEmail] VARCHAR(320) NOT NULL,
	[SecondaryContactPhone] VARCHAR(150) NULL,
	[Source] INT NOT NULL
)