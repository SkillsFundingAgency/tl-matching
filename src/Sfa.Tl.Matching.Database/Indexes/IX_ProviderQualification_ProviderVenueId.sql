CREATE NONCLUSTERED INDEX [IX_ProviderQualification_ProviderVenueId]
ON [dbo].[ProviderQualification] ([ProviderVenueId])
INCLUDE ([QualificationId])
