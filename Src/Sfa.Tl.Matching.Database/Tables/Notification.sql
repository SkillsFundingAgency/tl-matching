CREATE TABLE [dbo].[Notification]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Sender] NVARCHAR(50) NOT NULL, 
    [Recipients] NVARCHAR(50) NOT NULL, 
    [Subject] NVARCHAR(50) NOT NULL, 
    [Body] NVARCHAR(50) NOT NULL, 
    [Status] INT NOT NULL, 
    [CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate()
)
