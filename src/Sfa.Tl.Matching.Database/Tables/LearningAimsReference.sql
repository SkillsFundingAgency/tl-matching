CREATE TABLE [dbo].[LearningAimsReference]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LarId] VARCHAR(15) NOT NULL,
	[Title] VARCHAR(400) NOT NULL,
	[AwardOrgLarId] VARCHAR(50) NULL,
	[SourceCreatedOn] DATETIME2 NOT NULL,
	[SourceModifiedOn] DATETIME2 NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GetUTCDate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL,
	[ChecksumCol] AS BINARY_CHECKSUM([LarId], [Title], [AwardOrgLarId], [SourceCreatedOn], [SourceModifiedOn])
)
