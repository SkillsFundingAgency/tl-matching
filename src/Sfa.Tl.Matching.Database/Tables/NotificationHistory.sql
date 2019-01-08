CREATE TABLE [dbo].[NotificationHistory]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
	[EmailTemplateId] UNIQUEIDENTIFIER NOT NULL, 
	[ContactId] UNIQUEIDENTIFIER NOT NULL, 
	[Sender] NVARCHAR(50) NOT NULL, 
	[Recipients] NVARCHAR(100) NOT NULL, 
	[Subject] NVARCHAR(100) NOT NULL, 
	[Body] NVARCHAR(500) NOT NULL, 
	[Status] INT NOT NULL, 
	[CreatedUserName] NVARCHAR(100) NOT NULL, 
	[CreatedUserEmail] NVARCHAR(100) NOT NULL, 
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	CONSTRAINT [FK_Notification_Contact] FOREIGN KEY ([ContactId]) REFERENCES [Contact]([Id]),
	CONSTRAINT [FK_Notification_EmailTemplate] FOREIGN KEY ([EmailTemplateId]) REFERENCES [EmailTemplate]([Id])
)
