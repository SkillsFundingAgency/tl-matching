CREATE TABLE [dbo].[DBProjDeployLog]
(
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Date]     DATETIME       NOT NULL DEFAULT (getutcdate()),
    [Name]     NVARCHAR (255) NULL,
    [MD5]      VARCHAR (32)   NULL,
    [Revision] NVARCHAR (MAX) NULL
	CONSTRAINT [PK_DBProjDeployLog] PRIMARY KEY CLUSTERED ([Id] ASC)
)
