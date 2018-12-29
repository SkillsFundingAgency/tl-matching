CREATE TABLE [dbo].[NotificationHistory]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
	[EmailTemplateId] UNIQUEIDENTIFIER NOT NULL, 
	[Sender] NVARCHAR(50) NOT NULL, 
	[Recipients] NVARCHAR(50) NOT NULL, 
	[Subject] NVARCHAR(50) NOT NULL, 
	[Body] NVARCHAR(50) NOT NULL, 
	[Status] INT NOT NULL, 
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
    CONSTRAINT [FK_Notification_EmailTemplate] FOREIGN KEY ([EmailTemplateId]) REFERENCES [EmailTemplate]([Id])
)
