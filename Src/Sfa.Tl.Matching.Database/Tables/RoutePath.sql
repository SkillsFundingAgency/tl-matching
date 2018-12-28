CREATE TABLE [dbo].[RoutePath]
(
	[Id] INT NOT NULL PRIMARY KEY, 
	[CourseId] UNIQUEIDENTIFIER NOT NULL, 
    [Route] NVARCHAR(50) NOT NULL, 
    [Path] NVARCHAR(10) NOT NULL, 
    [Summary] NVARCHAR(50) NULL, 
    [Keywords] NVARCHAR(50) NULL, 
    [CreatedOn] DATETIME2 NULL DEFAULT GetDate(), 
    [ModifiedOn] DATETIME2 NULL, 
)
