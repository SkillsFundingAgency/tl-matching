CREATE TABLE [dbo].[MaintenanceHistory]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[IsOnline] BIT NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(),
	[CreatedBy] NVARCHAR(50) NULL,
	[ModifiedOn] DATETIME2 NULL,
	[ModifiedBy] NVARCHAR(50) NULL

    CONSTRAINT [PK_MaintenanceHistory] PRIMARY KEY ([Id])
)