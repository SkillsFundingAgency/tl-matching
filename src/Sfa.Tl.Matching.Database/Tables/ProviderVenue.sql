CREATE TABLE [dbo].[ProviderVenue]
(
	[Id] int IDENTITY(1,1) NOT NULL, 
	[ProviderId] INT NOT NULL, 
	[Name] VARCHAR(400) NULL, 
	[Town] VARCHAR(100) NULL, 
	[County] NVARCHAR(50) NULL, 
	[Postcode] VARCHAR(10) NOT NULL,
	[Latitude] [DECIMAL](9, 6) NULL,
	[Longitude] [DECIMAL](9, 6) NULL,
	[Location] GEOGRAPHY NULL,
	[Source] VARCHAR(50) NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL
	CONSTRAINT [PK_ProviderVenue] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_ProviderVenue_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [Provider]([Id]),
)
