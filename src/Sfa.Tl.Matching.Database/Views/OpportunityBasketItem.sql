CREATE VIEW [dbo].[OpportunityBasketItem]
	AS 
		SELECT 
			o.Id AS OpportunityId,
			e.CompanyName,
			e.AlsoKnownAs AS CompanyNameAka,
			oi.Id AS OpportunityItemId,
			oi.JobRole,
			oi.Placements,
			oi.PlacementsKnown,
			oi.Town + ' ' + oi.Postcode AS Workplace,
			oi.OpportunityType,
			pg.HadBadExperience,
			pg.NoSuitableStudent,
			pg.ProvidersTooFarAway,
			p.DisplayName,
			pv.[Name] AS VenueName,
			pv.Postcode
		FROM Opportunity AS o 
			INNER JOIN OpportunityItem AS OI ON o.Id = oi.OpportunityId
			LEFT JOIN Employer AS E ON o.EmployerCrmId = e.CrmId
			LEFT JOIN ProvisionGap AS PG on oi.Id = PG.OpportunityItemId
			LEFT JOIN Referral AS R on oi.Id = r.OpportunityItemId
			LEFT JOIN ProviderVenue AS PV on r.ProviderVenueId = pv.Id
			LEFT JOIN Provider AS P on pv.ProviderId = p.Id
		WHERE
			oi.IsSaved = 1 AND oi.IsCompleted = 0
