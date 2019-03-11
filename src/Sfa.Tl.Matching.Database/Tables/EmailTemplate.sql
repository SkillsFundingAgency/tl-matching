CREATE TABLE [dbo].[EmailTemplate]
(
<<<<<<< HEAD
	[Id] INT NOT NULL IDENTITY(1,1), 
=======
	[Id] INT IDENTITY(1,1) NOT NULL, 
>>>>>>> master
	[TemplateName] NVARCHAR(50) NOT NULL, 
	[TemplateId] NVARCHAR(50) NOT NULL, 
	[Recipients] [NVARCHAR](MAX) NULL, 
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL, 

    CONSTRAINT [PK_EmailTemplates] PRIMARY KEY ([Id])
)
