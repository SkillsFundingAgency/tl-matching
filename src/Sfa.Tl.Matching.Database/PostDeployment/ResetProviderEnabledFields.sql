UPDATE [dbo].[Provider]
SET [IsEnabledForSearch]  = [Status],
    [IsEnabledForReferral] = [Status]
WHERE [IsEnabledForSearch] IS NULL

UPDATE [dbo].[Provider]
SET [IsFundedForNextYear] = 1
WHERE [IsFundedForNextYear] IS NULL

ALTER TABLE [dbo].[Provider] ALTER COLUMN [IsEnabledForSearch] BIT NOT NULL;
ALTER TABLE [dbo].[Provider] ALTER COLUMN [IsEnabledForReferral] BIT NOT NULL;
ALTER TABLE [dbo].[Provider] ALTER COLUMN [IsFundedForNextYear] BIT NOT NULL;

UPDATE [dbo].[ProviderVenue]
SET [IsEnabledForSearch]  = 1
WHERE [IsEnabledForSearch] IS NULL

ALTER TABLE [dbo].[ProviderVenue] ALTER COLUMN [IsEnabledForSearch] BIT NOT NULL;
