CREATE VIEW [dbo].[MatchingServiceOpportunityReport]
	AS 
	
	SELECT
		oi.Id AS OpportunityItemId,
		oi.OpportunityType,
		CAST(CASE WHEN oi.IsSaved = 1 and oi.IsCompleted = 0 THEN 1 ELSE 0 END AS BIT) AS PipelineOpportunity,
		e.Id AS EmployerId,
		e.CompanyName,
		e.Aupa,
		e.[Owner],
		oi.Postcode AS EmployerPostCodeEnteredInSearch,
		oi.Placements,
		oi.JobRole,
		rt.[Name] as RouteName,
		pg.NoSuitableStudent, 
		pg.HadBadExperience, 
		pg.ProvidersTooFarAway, 
		lep.[Code] as LepCode,
		lep.[Name] as LepName,
		oi.CreatedBy AS Username,
		oi.CreatedOn,
		oi.ModifiedOn
	FROM Opportunity as o
		INNER JOIN OpportunityItem as oi on o.Id = oi.OpportunityId
		INNER JOIN [Route] as rt on rt.Id = oi.RouteId
		LEFT JOIN Employer as e on  e.CrmId = o.EmployerCrmId
		LEFT JOIN ProvisionGap as pg on oi.Id = pg.OpportunityItemId
		LEFT JOIN [PostcodeLookup] as pl on pl.Postcode = oi.Postcode
		LEFT JOIN [dbo].[LocalEnterprisePartnership] as lep on lep.Code = pl.LepCode
	WHERE 
		oi.IsSaved = 1
		AND (oi.IsCompleted = 1 or oi.IsDeleted = 0)
