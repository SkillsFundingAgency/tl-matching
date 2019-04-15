UPDATE [dbo].[Provider]
SET [IsEnabledForSearch]  = 1,
	[IsEnabledForReferral] = 1

ALTER TABLE [dbo].[Provider] ALTER COLUMN [IsEnabledForSearch] BIT NOT NULL;
ALTER TABLE [dbo].[Provider] ALTER COLUMN [IsEnabledForReferral] BIT NOT NULL