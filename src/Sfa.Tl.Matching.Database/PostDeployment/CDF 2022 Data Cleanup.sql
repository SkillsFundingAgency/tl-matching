--TLWP-1720 - Clear out existing data

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET @scriptName = 'CDF 2022 Data Cleanup';
SET @TicketNo  = 'TLWP-1720';

IF NOT EXISTS (SELECT 1 FROM [dbo].[DBProjDeployLog] WHERE [Name] = @scriptName)
BEGIN

	--TODO: Add script
	SELECT * FROM [dbo].[Providers];

	--Update deployment log
    --INSERT INTO [dbo].[DBProjDeployLog]( [Date], [Name], [MD5], [Revision] )
	--VALUES( GETUTCDATE(), @scriptName, @TicketNo, 1 );
END