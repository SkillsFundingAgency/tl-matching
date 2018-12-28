CREATE TABLE [dbo].[IndustryPlacement]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [RoutePathId] UNIQUEIDENTIFIER NULL, 
    [AddressId] UNIQUEIDENTIFIER NULL,
    [PlacementAvailable] INT NULL DEFAULT 0, 
    [PlacementsOffered] INT NULL, 
    [ContactedOn] DATETIME2 NULL, 
    [NextContactOn] DATETIME2 NULL, 
    [Resolution] NVARCHAR(100) NULL, 
)
