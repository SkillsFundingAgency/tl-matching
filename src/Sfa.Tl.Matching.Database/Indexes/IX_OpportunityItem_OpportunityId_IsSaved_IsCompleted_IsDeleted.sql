CREATE NONCLUSTERED INDEX [IX_OpportunityItem_OpportunityId_IsSaved_IsCompleted_IsDeleted]
ON [dbo].[OpportunityItem] ([OpportunityId],[IsSaved],[IsCompleted],[IsDeleted])
