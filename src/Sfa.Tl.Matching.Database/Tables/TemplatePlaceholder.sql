CREATE TABLE [dbo].[TemplatePlaceholder]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
	[EmailTemplateId] UNIQUEIDENTIFIER NOT NULL, 
	[Placeholder] NVARCHAR(100) NOT NULL,
	[CreatedOn] DATETIME2 NULL DEFAULT GetDate(), 
	[ModifiedOn] DATETIME2 NULL, 
	CONSTRAINT [FK_TemplatePlaceholder_EmailTemplate] FOREIGN KEY ([EmailTemplateId]) REFERENCES [EmailTemplate]([Id]), 
)
