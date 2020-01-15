CREATE NONCLUSTERED INDEX [IX_OpportunityBasketItem]
ON [dbo].[OpportunityItem] ([OpportunityId],[IsSaved],[IsCompleted])
INCLUDE ([Id],[OpportunityType],[Town],[Postcode],[JobRole],[PlacementsKnown],[Placements])
