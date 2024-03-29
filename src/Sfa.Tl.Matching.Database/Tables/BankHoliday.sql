﻿CREATE TABLE [dbo].[BankHoliday]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
	[Date] DATE NOT NULL, 
	[Title] NVARCHAR(100) NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL, 
)
