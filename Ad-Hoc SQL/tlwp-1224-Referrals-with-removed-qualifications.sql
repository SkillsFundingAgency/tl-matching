/*
A large number of qualifications have been removed recently, and several providers have been changed to not be CDF providers/not enabled for referrals.
Any incomplete ("in-flight") opportunities in baskets that use these removed/disabled qualifications, providers, or venues could result in emails
being sent to providers if the opportunities are completed. We need to identify which opportunities are affected and advise users to edit
those opportunities in their baskets.

This script finds all in-flight opportunities and identifies any associated with removed/disabled providers/venues. and any where
the route for the opportunity is no longer available because there is no longer any qualification offering that route at the selected location.
*/

declare @basket TABLE (
		OpportunityId INT NOT NULL,
		OpportunityItemId INT NOT NULL,
		OpportunityCreatedBy NVARCHAR(50) NOT NULL,
		OpportunityModifiedBy NVARCHAR(50) NOT NULL,
		EmployerName NVARCHAR(200) NOT NULL,
		Workplace NVARCHAR(110) NOT NULL,
		ProviderId INT NOT NULL,
		ProviderName NVARCHAR(400) NOT NULL,
		IsCDFProvider bit NOT NULL,
		IsEnabledForReferral bit NOT NULL,
		ProviderVenueId INT NOT NULL,
		Postcode NVARCHAR(10) NOT NULL,
		VenueName NVARCHAR(400) NOT NULL,
		VenueIsEnabledForReferral bit NOT NULL,
		VenueIsRemoved bit NOT NULL,
		RouteId INT NOT NULL,
		RouteName NVARCHAR(50) NOT NULL
)

declare @venueroutes TABLE (
		ProviderId INT NOT NULL,
		ProviderVenueId INT NOT NULL,
		Postcode NVARCHAR(10) NOT NULL,
		RouteId INT NOT NULL,
		RouteName NVARCHAR(50) NOT NULL,
		QualificationId INT NOT NULL,
		LarId NVARCHAR(8) NOT NULL,
		QualificationName NVARCHAR(400) NOT NULL
)

declare @results TABLE (
		OpportunityId INT NOT NULL,
		OpportunityItemId INT NOT NULL,
		EmployerName NVARCHAR(200) NOT NULL,
		OpportunityCreatedBy NVARCHAR(50) NOT NULL,
		OpportunityModifiedBy NVARCHAR(50) NOT NULL,
		IsCDFProvider bit NOT NULL,
		IsEnabledForReferral bit NOT NULL,
		ProviderId INT NOT NULL,
		ProviderName NVARCHAR(400) NOT NULL,
		ProviderVenueId INT NOT NULL,
		Postcode NVARCHAR(10) NOT NULL,
		VenueIsEnabledForReferral bit NOT NULL,
		VenueIsRemoved bit NOT NULL,
		RouteName NVARCHAR(50) NOT NULL,
		QualificationName NVARCHAR(400) NULL
)

---Items saved in the basket
INSERT INTO @basket
SELECT  o.Id AS OpportunityId,
		oi.Id AS OpportunityItemId,
		o.CreatedBy AS OpportunityCreatedBy,
		o.ModifiedBy AS OpportunityModifiedBy,
		e.CompanyName AS EmployerName,
		oi.Town + ' ' + oi.Postcode AS Workplace,
		p.Id ProviderId,
		p.[Name] AS ProviderName,
		p.IsCDFProvider,
		p.IsEnabledForReferral,
		pv.Id ProviderVenueId,
		pv.Postcode,
		pv.[Name] AS VenueName,
		pv.IsEnabledForReferral AS VenueIsEnabledForReferral,
		pv.IsRemoved AS VenueIsRemoved,
		rt.Id AS RouteId,
		rt.[Name] AS RouteName
FROM	Opportunity AS o 
		INNER JOIN OpportunityItem AS oi ON o.Id = oi.OpportunityId
		LEFT JOIN Employer AS e ON o.EmployerCrmId = e.CrmId
		LEFT JOIN Referral AS r on oi.Id = r.OpportunityItemId
		LEFT JOIN ProviderVenue AS pv on r.ProviderVenueId = pv.Id
		LEFT JOIN Provider AS p on pv.ProviderId = p.Id
		LEFT JOIN ROUTE AS rt on RT.Id = oi.RouteId
WHERE	oi.IsSaved = 1 
		AND oi.IsCompleted = 0
		AND oi.IsDeleted = 0
		AND OpportunityType = 'Referral'

--All current basket items
--SELECT * FROM @basket

--All routes for providervenues
INSERT INTO @venueroutes
SELECT	
		p.Id ProviderId,
		pv.Id VenueId,
		pv.Postcode,
		r.Id AS RouteId,
		r.[Name] AS RouteName,
		q.Id AS QualificationId,
		q.LarId,
		q.[Title] AS QualificationTitle
FROM Provider p
INNER JOIN ProviderVenue pv
ON pv.ProviderId = p.Id
INNER JOIN ProviderQualification pvq on pvq.ProviderVenueId = pv.Id
INNER JOIN Qualification q
ON q.Id = pvq.QualificationId
INNER JOIN QualificationRouteMapping qrm
ON qrm.QualificationId = q.Id
INNER JOIN Route r
ON r.Id = qrm.RouteId
WHERE
--Only select valid providers venues - the others have been shown in the queries above
		p.IsCdfProvider = 1 
		AND p.IsEnabledForReferral = 1 
		AND pv.IsRemoved = 0 
		AND pv.IsEnabledForReferral = 1 
		AND q.IsDeleted = 0 

--select * from @venueroutes

--Provider no longer a CDF provider or no longer enabled for referrals
/*
SELECT * FROM @basket b
WHERE	b.IsCDFProvider = 0
	 OR b.IsEnabledForReferral = 0 
	 OR b.VenueIsEnabledForReferral = 0
	 OR b.VenueIsRemoved = 1
ORDER BY b.OpportunityId, 
		 b.OpportunityItemId
*/

--Provider Venue is removed or no longer enabled for referrals
/*
SELECT * FROM @basket b
WHERE VenueIsEnabledForReferral = 0
OR VenueIsRemoved = 1
*/

--Route not available because qualification has been removed
/*
SELECT * 
FROM @basket b
LEFT JOIN @venueroutes pvr 
ON		b.ProviderId = pvr.ProviderId
AND		b.ProviderVenueId = pvr.ProviderVenueId
AND		b.RouteId = pvr.RouteId
WHERE 	pvr.RouteId IS NULL
ORDER BY	b.OpportunityId, 
			b.OpportunityItemId, 
			b.Postcode, 
			b.RouteName
*/

--Everything
INSERT INTO @results
SELECT	b.OpportunityId, 
		b.OpportunityItemId, 
		b.EmployerName, 
		b.OpportunityCreatedBy, 
		b.OpportunityModifiedBy, 
		b.IsCDFProvider, 
		b.IsEnabledForReferral, 
		b.ProviderId,
		b.ProviderName, 
		b.ProviderVenueId,
		b.Postcode, 
		b.VenueIsEnabledForReferral,
		b.VenueIsRemoved,
		b.RouteName,
		pvr.QualificationName
FROM @basket b
LEFT JOIN @venueroutes pvr 
ON		b.ProviderId = pvr.ProviderId
AND		b.ProviderVenueId = pvr.ProviderVenueId
AND		b.RouteId = pvr.RouteId
WHERE 	pvr.RouteId IS NULL --Covers the case of missing qualifications
	OR	(b.IsCDFProvider = 0
	  OR b.IsEnabledForReferral = 0 
	  OR b.VenueIsEnabledForReferral = 0
	  OR b.VenueIsRemoved = 1) --Covers providers or venues that have been removed or hidden

--Show detailed results
SELECT * 
FROM @results
ORDER BY	OpportunityId, 
			OpportunityItemId, 
			Postcode, 
			RouteName

--Show results with count of items in each basket
SELECT	OpportunityCreatedBy,
		EmployerName,
		count(OpportunityCreatedBy) AS [Count]
FROM	@results
GROUP BY	OpportunityCreatedBy,
			EmployerName
ORDER BY	OpportunityCreatedBy,
			EmployerName

--Check if two users could be working on same opportunity 
--SELECT * 
--FROM @results	  
--WHERE OpportunityCreatedBy <> OpportunityModifiedBy

--Show a more user-friendly version of the results
SELECT	OpportunityCreatedBy,
		EmployerName, 
		ProviderName,
		CASE 
			WHEN (IsCDFProvider = 0 OR IsEnabledForReferral = 0) THEN 'YES'
			ELSE ''
		END AS [Provider Removed],
		Postcode,	  
		CASE 
			WHEN (VenueIsRemoved = 1 OR VenueIsEnabledForReferral = 0) THEN 'YES'
			ELSE ''
		END AS [Venue Removed],
		RouteName
FROM	@results
ORDER BY	OpportunityCreatedBy, 
			EmployerName, 
			ProviderName, 
			Postcode, 
			RouteName
