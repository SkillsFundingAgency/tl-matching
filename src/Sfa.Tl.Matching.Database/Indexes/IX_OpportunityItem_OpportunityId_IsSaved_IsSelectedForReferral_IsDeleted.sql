CREATE NONCLUSTERED INDEX [IX_OpportunityItem_OpportunityId_IsSaved_IsSelectedForReferral_IsDeleted]
ON [dbo].[OpportunityItem] ([OpportunityId],[IsSaved],[IsSelectedForReferral],[IsDeleted])
