CREATE TABLE [dbo].[Employer]
(
	[Id] uniqueidentifier NOT NULL PRIMARY KEY, 
	[CompanyName] NVARCHAR(160) NOT NULL, 
	[AlsoKnownAs] NVARCHAR(100) NULL, 
	[CompanyType] INT NOT NULL, 
	[AupaStatus] INT NOT NULL, 
	[Website] NVARCHAR(200) NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[ModifiedOn] DATETIME2 NULL,
)