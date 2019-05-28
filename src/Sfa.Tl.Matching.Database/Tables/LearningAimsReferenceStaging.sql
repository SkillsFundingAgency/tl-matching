CREATE TABLE [dbo].[LearningAimsReferenceStaging]
(
	[LarId] VARCHAR(15) NOT NULL,
	[Title] VARCHAR(400) NOT NULL,
	[AwardOrgLarId] VARCHAR(150) NULL,
	[SourceCreatedOn] DATETIME2 NOT NULL,
	[SourceModifiedOn] DATETIME2 NOT NULL,
	[CreatedBy] NVARCHAR(50) NULL, 
	[ChecksumCol] AS BINARY_CHECKSUM([LarId], [Title], [AwardOrgLarId], [SourceCreatedOn], [SourceModifiedOn])
)
