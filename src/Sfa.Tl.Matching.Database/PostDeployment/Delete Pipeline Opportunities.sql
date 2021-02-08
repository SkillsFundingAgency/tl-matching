--TLWP-1352 - Delete pipeline opportunities for specific users

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET @scriptName = 'DeletePipelineOpportunities';
SET @TicketNo  = 'TLWP-1352';

IF NOT EXISTS (SELECT 1 FROM [dbo].[DBProjDeployLog] WHERE [Name] = @scriptName )
BEGIN

	DECLARE @updatedByUser NVARCHAR(50) = 'Update Script TLWP-1352'

	DECLARE @opportunityItemsToRemove TABLE (
	  opportunityItemId INT NOT NULL
	)

	declare @users TABLE (
	  username NVARCHAR(50) NOT NULL
	)

	INSERT INTO @users 
	VALUES	('Simon Peek'), 
			('Mark Coulson'), 
			('Nichola Akers'),
			--For testing on the test environment
			('2 Tmatching')

	INSERT INTO @opportunityItemsToRemove
	SELECT DISTINCT oi.Id as OpportunityItemId
	FROM Opportunity AS o 
	INNER JOIN OpportunityItem AS OI ON o.Id = oi.OpportunityId
	INNER JOIN @users u on o.CreatedBy = u.username
	WHERE oi.IsSaved = 1 
		AND oi.IsCompleted = 0
		AND oi.IsDeleted = 0

	UPDATE OpportunityItem
	SET IsDeleted = 1,
	ModifiedOn = getutcdate(),
	ModifiedBy = @updatedByUser
	WHERE Id IN (SELECT OpportunityItemId 
				 FROM @opportunityItemsToRemove)

	--Update deployment log
    INSERT INTO [dbo].[DBProjDeployLog]( [Date], [Name], [MD5], [Revision] )
	VALUES( GETUTCDATE(), @scriptName, @TicketNo, 1 );

END