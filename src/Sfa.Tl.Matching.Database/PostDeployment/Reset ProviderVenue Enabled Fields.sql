UPDATE [dbo].[ProviderVenue]
SET [IsEnabledForSearch]  = 1

ALTER TABLE [dbo].[ProviderVenue] ALTER COLUMN [IsEnabledForSearch] BIT NOT NULL;