CREATE TABLE [dbo].[Employer]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CrmId] uniqueidentifier NOT NULL,
	[CompanyName] NVARCHAR(160) NOT NULL, 
	[AlsoKnownAs] NVARCHAR(100) NULL, 
	[Aupa] NVARCHAR(10) NULL, 
	[CompanyType] NVARCHAR(100) NULL, 
	[PrimaryContact] NVARCHAR(100) NULL, 
	[Phone] VARCHAR(150) NULL,
	[Email] VARCHAR(320) NULL,
	[PostCode] VARCHAR(10) NOT NULL,
	[Owner] NVARCHAR(150) NOT NULL,
)