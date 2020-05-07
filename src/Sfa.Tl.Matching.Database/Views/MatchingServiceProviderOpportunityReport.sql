CREATE VIEW [dbo].[MatchingServiceProviderOpportunityReport]
	AS 
	SELECT 
		OpportunityItemCount, 
		RouteName, 
		ProviderVenuePostCode, 
		ProviderName,
		lep.[Code] as LepCode,
		lep.[Name] as LepName
	FROM   (SELECT 
			count(oi.Id) OpportunityItemCount, 
			R.[Name] AS RouteName, 
			pv.Postcode AS ProviderVenuePostCode, 
			p.[Name] AS ProviderName
			FROM OpportunityItem AS OI
			INNER JOIN Referral AS RF ON OI.Id = RF.OpportunityItemId
			INNER JOIN ProviderVenue AS PV ON pv.Id = rf.ProviderVenueId
			INNER JOIN [Provider] AS P ON pv.ProviderId = p.Id
			INNER JOIN [Route] AS R ON R.Id = oi.RouteId
			WHERE 
				oi.IsSaved = 1 AND oi.IsCompleted = 1
			GROUP BY 
				R.[Name], pv.[Postcode], p.[Name], p.[Id]) t
	LEFT JOIN [PostcodeLookup] as pl on pl.Postcode = t.ProviderVenuePostCode
	LEFT JOIN [dbo].[LocalEnterprisePartnership] as lep on lep.Code = pl.PrimaryLepCode