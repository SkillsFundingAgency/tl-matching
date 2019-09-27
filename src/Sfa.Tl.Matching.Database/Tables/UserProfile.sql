CREATE TABLE [dbo].[UserProfile]
(
	[Id]					INT IDENTITY(1,1) NOT NULL,
	[Username]				VARCHAR(50) NULL,
	[Team]					VARCHAR(50) NULL,
	[Region]				VARCHAR(50) NULL,
	[CreatedOn]				DATETIME2 NOT NULL DEFAULT getutcdate(), 
	[CreatedBy]				NVARCHAR(50) NULL, 
	[ModifiedOn]			DATETIME2 NULL, 
	[ModifiedBy]			NVARCHAR(50) NULL
    CONSTRAINT [PK_UserProfile] PRIMARY KEY ([Id])
)
