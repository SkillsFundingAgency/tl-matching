CREATE TABLE [dbo].[BackLinkHistory]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [Link] NVARCHAR(MAX) NOT NULL, 
    [CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL
)
