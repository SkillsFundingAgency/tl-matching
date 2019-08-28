SELECT 
	count(oi.Id) OpportunityItemCount, 
	R.Name AS RouteName, 
	pv.postcode AS ProviderVenuePostCode, 
	p.name AS ProviderName
FROM OpportunityItem as OI
INNER JOIN Referral as RF on OI.id = RF.OpportunityItemId
INNER JOIN ProviderVenue AS PV on pv.Id = rf.ProviderVenueId
INNER JOIN [Provider] AS P on pv.ProviderId = p.Id
INNER JOIN [Route] as R on R.Id = oi.RouteId
where 
	oi.IsSaved = 1
GROUP BY 
	oi.RouteId, pv.postcode, p.name, p.id