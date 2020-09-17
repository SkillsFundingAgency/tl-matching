--SET NOCOUNT ON;
--SET XACT_ABORT ON;
--SET @scriptName = 'DeduplicateQualifications';
--SET @TicketNo  = 'TLWP-1221';

--IF NOT EXISTS (SELECT 1 FROM [dbo].[DBProjDeployLog] WHERE [Name] = @scriptName )
--BEGIN

    --TODO: Implement the deduplication script

    --INSERT INTO [dbo].[DBProjDeployLog]( [Date], [Name], [MD5], [Revision] )
	--VALUES( GETUTCDATE(), @scriptName, @TicketNo, 1 );

--END