CREATE TABLE [dbo].[EmailTemplate]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[TemplateName] NVARCHAR(50) NOT NULL, 
	[TemplateId] NVARCHAR(50) NOT NULL, 
	[Recipients] [NVARCHAR](MAX) NULL, 
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL, 

    CONSTRAINT [PK_EmailTemplates] PRIMARY KEY ([Id])
)
