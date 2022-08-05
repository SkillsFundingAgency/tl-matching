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
	)

	--Get ids from opportunity baskets
	INSERT INTO @ids
	SELECT   DISTINCT o.Id AS OpportunityId
	FROM Opportunity AS o 
		INNER JOIN OpportunityItem AS OI ON o.Id = oi.OpportunityId
	WHERE
		oi.IsSaved = 1 
		AND 
		oi.IsCompleted = 0
		AND oi.IsDeleted = 0

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
	--Workaround for issue in test environment - has an email that has no copmpleted opportunity item
	AND NOT EXISTS (SELECT * FROM EmailHistory eh
					  WHERE eh.OpportunityId = o.Id)

	--Update deployment log
    INSERT INTO [dbo].[DBProjDeployLog]([Date], [Name], [MD5], [Revision])
	VALUES(GETUTCDATE(), @scriptName, @TicketNo, 1);
END
