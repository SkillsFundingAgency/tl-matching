CREATE NONCLUSTERED INDEX [IX_QualificationRouteMapping_RouteId]
ON [dbo].[QualificationRouteMapping] ([RouteId])
INCLUDE ([QualificationId])
