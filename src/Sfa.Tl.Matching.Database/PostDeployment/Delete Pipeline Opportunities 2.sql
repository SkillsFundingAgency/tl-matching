--TLWP-1352 - Delete pipeline opportunities for specific users

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET @scriptName = 'DeletePipelineOpportunities2';
SET @TicketNo  = 'TLWP-1352-b';

IF NOT EXISTS (SELECT 1 FROM [dbo].[DBProjDeployLog] WHERE [Name] = @scriptName )
BEGIN

	DECLARE @tlwp1352b_updatedByUser NVARCHAR(50) = 'Update Script TLWP-1352'

	DECLARE @tlwp1352b_opportunityItemsToRemove TABLE (
	  opportunityItemId INT NOT NULL
	)

	DECLARE @tlwp1352b_users TABLE (
	  username NVARCHAR(50) NOT NULL
	)

	INSERT INTO @tlwp1352b_users 
	VALUES	('Steven Nicolson')

	INSERT INTO @tlwp1352b_opportunityItemsToRemove
	SELECT DISTINCT oi.Id as OpportunityItemId
	FROM Opportunity AS o 
	INNER JOIN OpportunityItem AS OI ON o.Id = oi.OpportunityId
	INNER JOIN @tlwp1352b_users u on o.CreatedBy = u.username
	WHERE oi.IsSaved = 1 
		AND oi.IsCompleted = 0
		AND oi.IsDeleted = 0

	UPDATE OpportunityItem
	SET IsDeleted = 1,
	ModifiedOn = getutcdate(),
	ModifiedBy = @tlwp1352b_updatedByUser
	WHERE Id IN (SELECT OpportunityItemId 
				 FROM @tlwp1352b_opportunityItemsToRemove)

	--Update deployment log
    INSERT INTO [dbo].[DBProjDeployLog]( [Date], [Name], [MD5], [Revision] )
	VALUES( GETUTCDATE(), @scriptName, @TicketNo, 1 );

END