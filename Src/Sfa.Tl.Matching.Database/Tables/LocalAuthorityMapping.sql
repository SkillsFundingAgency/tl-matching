CREATE TABLE [dbo].[LocalAuthorityMapping]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
	[LocalAuthorityCode] NVARCHAR(50) NOT NULL, 
	[LocalAuthority] NVARCHAR(50) NOT NULL, 
	[ LocalEnterprisePartnership] NVARCHAR(50) NOT NULL
)
