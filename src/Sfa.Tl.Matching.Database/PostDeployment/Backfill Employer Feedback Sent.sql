SET NOCOUNT ON;
SET XACT_ABORT ON;
SET @scriptName = 'BackfillEmployerFeedbackSent';
SET @TicketNo  = 'TLWP-840';

IF NOT EXISTS (SELECT 1 FROM [dbo].[DBProjDeployLog] WHERE [Name] = @scriptName )
BEGIN

	UPDATE [OpportunityItem]
	SET [EmployerFeedbackSent] = 1
	WHERE [ModifiedOn] < GETUTCDATE()

	INSERT INTO [dbo].[DBProjDeployLog]( [Date], [Name], [MD5], [Revision] )
	VALUES( GETUTCDATE(), @scriptName, @TicketNo, 1 );

END;