CREATE TABLE [dbo].[PostcodeLookup]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[Postcode] VARCHAR(10) NOT NULL,
	[LepCode] VARCHAR(10) NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_PostcodeLookup] PRIMARY KEY ([Postcode]),
)
