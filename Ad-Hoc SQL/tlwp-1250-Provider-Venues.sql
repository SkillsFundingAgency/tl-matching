/*
Some of the venues were incorrectly updated as part of the 20/21 CDF provider data collection exercise.

The purpose of this ticket is to extract the following items of data from the Matching Service:

	Provider UK PRN
	Provider name
	Provider display name
	Is the provider receiving capacity and delivery funding (CDF)?
	Does the provider want our help finding placements for students?
	Venue Name
	Venue Postcode
	Does the provider want our help finding placements for students at this venue?
	When was the venue last modified, and by who?

*/

SELECT		p.[UkPrn] AS [UKPRN], 
			p.[Name] AS [Provider Name],
			p.[DisplayName] AS [Provider Display Name],
			--p.[IsCDFProvider],
			CASE 
				WHEN (p.[IsCDFProvider] = 1) THEN 'TRUE'
				ELSE 'FALSE'
			END AS [Provider Receiving CDF],
			--p.[IsEnabledForReferral],
			CASE 
				WHEN (p.[IsEnabledForReferral] = 1) THEN 'TRUE'
				ELSE 'FALSE'
			END AS [Provider Enabled for Referral],
			pv.[Postcode],
			pv.[Name] AS [Venue Name],
			--pv.[IsEnabledForReferral],
			CASE 
				WHEN (pv.[IsEnabledForReferral] = 1) THEN 'TRUE'
				ELSE 'FALSE'
			END AS [Venue Enabled for Referral],
			--pv.[CreatedOn] AS [Created], 
			--pv.[CreatedBy] AS [Created By], 
			pv.[ModifiedOn] AS [Last Changed], 
			pv.[ModifiedBy] AS [Last Changed By]
FROM		[Provider] p
LEFT JOIN	[ProviderVenue] pv
ON			pv.[ProviderId] = p.id
WHERE		pv.[IsRemoved] = 0
ORDER BY	p.[UkPrn],
			pv.[Name]
