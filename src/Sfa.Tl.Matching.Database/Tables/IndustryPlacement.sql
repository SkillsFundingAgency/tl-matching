CREATE TABLE [dbo].[IndustryPlacement]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
	[RoutePathId] UNIQUEIDENTIFIER NULL, 
	[AddressId] UNIQUEIDENTIFIER NULL,
	[Placement] INT NULL DEFAULT 0, 
	[PlacementReferred] INT NULL DEFAULT 0, 
	[PlacementOffered] INT NULL DEFAULT 0, 
	[PlacementGap] Bit NULL DEFAULT 0, 
	[ContactedOn] DATETIME2 NULL, 
	[NextContactOn] DATETIME2 NULL, 
	[Resolution] NVARCHAR(100) NULL, 
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[ModifiedOn] DATETIME2 NULL, 
	CONSTRAINT [FK_IndustryPlacement_RoutePath] FOREIGN KEY ([RoutePathId]) REFERENCES [RoutePath]([Id]),
	CONSTRAINT [FK_IndustryPlacement_Address] FOREIGN KEY ([AddressId]) REFERENCES [Address]([Id]),
)
