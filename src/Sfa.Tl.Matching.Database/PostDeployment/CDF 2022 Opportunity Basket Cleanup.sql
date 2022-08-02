--TLWP-1720 - Clear out existing opportunites

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET @scriptName = 'CDF 2022 Opportunity Basket Cleanup';
SET @TicketNo  = 'TLWP-1720';

IF NOT EXISTS (SELECT 1 FROM [dbo].[DBProjDeployLog] WHERE [Name] = @scriptName)
BEGIN

	DECLARE @ids TABLE 
	(
		OpportunityId int
		--OpportunityItemId int,
		--ReferralType nvarchar(12)
	)

	--Get ids from opportunity baskets
	INSERT INTO @ids
	SELECT   DISTINCT o.Id AS OpportunityId
			--,oi.Id AS OpportunityItemId
			--,oi.OpportunityType
	FROM Opportunity AS o 
		INNER JOIN OpportunityItem AS OI ON o.Id = oi.OpportunityId
		--LEFT JOIN Employer AS E ON o.EmployerCrmId = e.CrmId
		--LEFT JOIN ProvisionGap AS PG on oi.Id = PG.OpportunityItemId
		--LEFT JOIN Referral AS R on oi.Id = r.OpportunityItemId
		--LEFT JOIN ProviderVenue AS PV on r.ProviderVenueId = pv.Id
		--LEFT JOIN Provider AS P on pv.ProviderId = p.Id
	WHERE
		oi.IsSaved = 1 
		AND 
		oi.IsCompleted = 0
		AND oi.IsDeleted = 0

	--SELECT * FROM @ids

	SELECT * FROM @ids ids	
	INNER JOIN OpportunityItem oi ON oi.OpportunityId = ids.OpportunityId
	INNER JOIN Referral AS r on oi.Id = r.OpportunityItemId
	WHERE oi.IsCompleted = 0

	PRINT('DELETE Referral')

	DELETE Referral 
	FROM @ids ids	
	INNER JOIN OpportunityItem oi ON oi.OpportunityId = ids.OpportunityId
	INNER JOIN Referral AS r on oi.Id = r.OpportunityItemId
	WHERE oi.IsCompleted = 0

	SELECT * FROM @ids ids	
	INNER JOIN OpportunityItem oi ON oi.OpportunityId = ids.OpportunityId
	INNER JOIN ProvisionGap AS pg on oi.Id = pg.OpportunityItemId	
	WHERE oi.IsCompleted = 0

	PRINT('DELETE ProvisionGap')
	
	DELETE ProvisionGap 
	FROM @ids ids	
	INNER JOIN OpportunityItem oi ON oi.OpportunityId = ids.OpportunityId
	INNER JOIN ProvisionGap AS pg on oi.Id = pg.OpportunityItemId	
	WHERE oi.IsCompleted = 0
	
	SELECT * FROM @ids ids	
	INNER JOIN OpportunityItem oi
	ON oi.OpportunityId = ids.OpportunityId
	WHERE oi.IsCompleted = 0

	PRINT('DELETE OpportunityItem')

	DELETE OpportunityItem
	FROM @ids ids	
	INNER JOIN OpportunityItem oi
	ON oi.OpportunityId = ids.OpportunityId
	WHERE oi.IsCompleted = 0

	SELECT * FROM @ids ids	
	INNER JOIN Opportunity o
	ON o.Id = ids.OpportunityId
	WHERE NOT EXISTS (SELECT * FROM OpportunityItem oi
					  WHERE oi.OpportunityId = o.Id
					  AND oi.IsCompleted = 1)

	PRINT('DELETE Opportunity')

	DELETE Opportunity
	FROM @ids ids
	INNER JOIN Opportunity o
	ON o.Id = ids.OpportunityId
	WHERE NOT EXISTS (SELECT * FROM OpportunityItem oi
					  WHERE oi.OpportunityId = o.Id
					  AND oi.IsCompleted = 1)

	--Update deployment log
    INSERT INTO [dbo].[DBProjDeployLog]([Date], [Name], [MD5], [Revision])
	VALUES(GETUTCDATE(), @scriptName, @TicketNo, 1);
END
