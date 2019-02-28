CREATE TABLE [dbo].[ProviderQualification]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[ProviderVenueId] INT NOT NULL,
	[QualificationId] INT NOT NULL, 
	[NumberOfPlacements] INT NULL, 
	[Source] VARCHAR(50) NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL 
	CONSTRAINT [PK_ProviderQualification] PRIMARY KEY ([Id]), 
	CONSTRAINT [FK_RProviderQualification_ProviderVenue] FOREIGN KEY ([ProviderVenueId]) REFERENCES [ProviderVenue]([Id]),
	CONSTRAINT [FK_RProviderQualification_Qualification] FOREIGN KEY ([QualificationId]) REFERENCES [Qualification]([Id])
)
