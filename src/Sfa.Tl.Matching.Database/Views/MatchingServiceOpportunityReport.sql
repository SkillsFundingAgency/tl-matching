CREATE VIEW [dbo].[MatchingServiceOpportunityReport]
	AS 
	
	WITH CTE AS (
		SELECT
			oi.Id AS OpportunityItemId,
			count(p.Id) AS ProviderCount
		from [Provider] AS P
			INNER JOIN ProviderVenue AS pv ON pv.ProviderId = p.Id
			INNER JOIN Referral AS rf ON rf.ProviderVenueId = pv.Id
			INNER JOIN OpportunityItem AS oi ON rf.OpportunityItemId = oi.Id
		GROUP BY oi.Id
	)
	SELECT
		oi.Id AS OpportunityItemId,
		oi.OpportunityType,
		oi.IsCompleted,
		oi.IsSaved,
		CAST(CASE WHEN oi.IsSaved = 1 and oi.IsCompleted = 0 THEN 1 ELSE 0 END AS BIT) AS PipelineOpportunity,
		e.Id AS EmployerId,
		e.CompanyName,
		e.Aupa,
		e.Owner,
		oi.Postcode AS EmployerPostCodeEnteredInSearch,
		oi.PlacementsKnown,
		oi.Placements,
		oi.JobRole,
		cte.ProviderCount,
		rt.[Name] as RouteName,
		rl.Region,
		rl.Team,
		oi.CreatedBy AS Username,
		oi.ModifiedOn,
		oi.CreatedOn,
		DATEADD(DAY, 6 - DATEPART(WEEKDAY, oi.CreatedOn), CAST(oi.CreatedOn AS  DATE)) WeekEndDate,
		LEFT(DATENAME(MONTH, oi.CreatedOn),3) + ' ' + RIGHT('00' + CAST(YEAR(oi.CreatedOn) AS VARCHAR),2) as [Date]
	FROM Opportunity as o
		INNER JOIN OpportunityItem as oi on o.Id = oi.OpportunityId
		INNER JOIN [Route] as rt on rt.Id = oi.RouteId
		LEFT JOIN CTE ON cte.OpportunityItemId = oi.Id
		LEFT JOIN UserProfile as rl on o.CreatedBy = rl.Username
		LEFT JOIN Employer as e on  e.CrmId = o.EmployerCrmId
	WHERE 
		oi.IsSaved = 1
	--ORDER BY 
	--    OpportunityType DESC,
	-- 	IsCompleted DESC,
	-- 	IsSaved DESC,
	--	OpportunityItemId