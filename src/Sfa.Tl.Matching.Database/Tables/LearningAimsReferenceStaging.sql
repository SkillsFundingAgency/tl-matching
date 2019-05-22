CREATE TABLE [dbo].[LearningAimsReferenceStaging]
(
	LearnAimRef VARCHAR(15) NOT NULL,
	LearnAimRefTitle VARCHAR(400) NOT NULL,
	AwardOrgAimRef VARCHAR(50) NULL,
	SourceCreatedOn DATETIME2 NOT NULL,
	SourceModifiedOn DATETIME2 NOT NULL,
	ChecksumCol AS BINARY_CHECKSUM([LearnAimRef], [LearnAimRefTitle], [AwardOrgAimRef], [SourceCreatedOn], [SourceModifiedOn])
)
