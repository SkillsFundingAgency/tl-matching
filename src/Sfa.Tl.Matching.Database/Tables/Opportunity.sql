CREATE TABLE [dbo].[Opportunity]
(
	[Id]					INT IDENTITY(1,1) NOT NULL, 
	[EmployerCrmId]			uniqueidentifier NULL,
	[EmployerContact]		NVARCHAR(100) NULL,
	[EmployerContactEmail]	VARCHAR(320) NULL,
	[EmployerContactPhone]	VARCHAR(150) NULL,
	[CreatedOn]				DATETIME2 NOT NULL DEFAULT getutcdate(), 
	[CreatedBy]				NVARCHAR(50) NULL, 
	[ModifiedOn]			DATETIME2 NULL, 
	[ModifiedBy]			NVARCHAR(50) NULL
	CONSTRAINT [PK_Opportunity] PRIMARY KEY ([Id]), 
    [IsDeleted] BIT NOT NULL DEFAULT 0,
)