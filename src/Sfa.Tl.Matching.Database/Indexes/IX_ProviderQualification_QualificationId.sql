CREATE NONCLUSTERED INDEX [IX_ProviderQualification_QualificationId]
ON [dbo].[ProviderQualification] ([QualificationId])
INCLUDE ([ProviderVenueId])
