UPDATE [dbo].[ProviderVenue]
SET [IsEnabledForSearch]  = 1,
    [IsEnabledForReferral] = 1

ALTER TABLE [dbo].[ProviderVenue] ALTER COLUMN [IsEnabledForSearch] BIT NOT NULL;
ALTER TABLE [dbo].[ProviderVenue] ALTER COLUMN [IsEnabledForReferral] BIT NOT NULL;