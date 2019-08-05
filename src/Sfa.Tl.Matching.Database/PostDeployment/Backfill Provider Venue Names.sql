SET NOCOUNT ON;
SET XACT_ABORT ON;
SET @scriptName = 'BackfillProviderVenueNames';
SET  @TicketNo  = 'TLWP-832';

IF NOT EXISTS (SELECT 1 FROM [dbo].[DBProjDeployLog] WHERE [Name] = @scriptName )
BEGIN

	UPDATE ProviderVenue
	SET [Name] = [Postcode]

	INSERT INTO [dbo].[DBProjDeployLog]( [Date], [Name], [MD5], [Revision] )
	VALUES( GETUTCDATE(), @scriptName, @TicketNo, 1 );

END;