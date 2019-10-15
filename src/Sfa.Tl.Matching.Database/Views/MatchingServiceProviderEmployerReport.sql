CREATE VIEW [dbo].[MatchingServiceProviderEmployerReport]
	AS 
	
	SELECT 
		--oi.OpportunityId,
		oi.Id AS OpportunityItemId,
		p.Name AS ProviderName,
		pv.Name AS ProviderVenueName,
		e.CompanyName AS EmployerName, 
		oi.Postcode  as EmployerPostCode, 
		rt.[Name] as RouteName, 
		oi.JobRole, 
		oi.Placements,
		--oi.IsSaved,
		--oi.IsCompleted,
		--e.[Owner],
		oi.CreatedBy, 
		oi.CreatedOn,
		oi.ModifiedOn
	FROM Opportunity as o
	LEFT JOIN Employer as e on o.EmployerCrmId = e.CrmId
	INNER JOIN OpportunityItem as oi on o.Id = oi.OpportunityId
	INNER JOIN [Route] as rt on oi.RouteId = rt.Id 
	INNER JOIN Referral as r on oi.Id = r.OpportunityItemId
	INNER JOIN ProviderVenue as pv on pv.Id = r.ProviderVenueId
	INNER JOIN [Provider] as P on pv.ProviderId = p.Id
	where 
		oi.IsSaved = 1 
	AND oi.IsCompleted = 1
	AND oi.OpportunityType = 'Referral'
