UPDATE [dbo].[Provider]
SET [IsEnabledForSearch]  = [Status],
    [IsEnabledForReferral] = [Status]

ALTER TABLE [dbo].[Provider] ALTER COLUMN [IsEnabledForSearch] BIT NOT NULL;
ALTER TABLE [dbo].[Provider] ALTER COLUMN [IsEnabledForReferral] BIT NOT NULL;

UPDATE [dbo].[ProviderVenue]
SET [IsEnabledForSearch]  = 1

ALTER TABLE [dbo].[ProviderVenue] ALTER COLUMN [IsEnabledForSearch] BIT NOT NULL;
