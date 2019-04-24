﻿CREATE TABLE [dbo].[Provider]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UkPrn] BIGINT NOT NULL,
	[Name] NVARCHAR(400) NOT NULL,
	[DisplayName] NVARCHAR(400) NULL,
	[OfstedRating] INT NOT NULL,
	[Status] bit NOT NULL,
	[StatusReason] NVARCHAR(100) NULL,
	[PrimaryContact] NVARCHAR(100) NOT NULL,
	[PrimaryContactEmail] VARCHAR(320) NOT NULL,
	[PrimaryContactPhone] VARCHAR(150) NULL,
	[SecondaryContact] NVARCHAR(100) NOT NULL,
	[SecondaryContactEmail] VARCHAR(320) NOT NULL,
	[SecondaryContactPhone] VARCHAR(150) NULL,
	[IsEnabledForSearch] BIT NULL,
	[IsEnabledForReferral] BIT NULL,
	[IsFundedForNextYear] BIT NULL,
	[Source] VARCHAR(50) NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL
	CONSTRAINT [PK_Provider] PRIMARY KEY ([Id]),
)