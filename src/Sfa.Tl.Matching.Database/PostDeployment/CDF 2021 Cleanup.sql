--TLWP-1352 - Delete pipeline opportunities for specific users

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET @scriptName = 'CDF2021Cleanup';
SET @TicketNo  = 'TLWP-1471';

IF NOT EXISTS (SELECT 1 FROM [dbo].[DBProjDeployLog] WHERE [Name] = @scriptName )
BEGIN

	--Remove incorrectly added provider
	DECLARE @ukPrnToDelete BIGINT = 10064234

	DELETE ProviderQualification
	FROM		Provider p
	INNER JOIN	providervenue pv
	ON			pv.ProviderId = p.Id
	INNER JOIN	ProviderQualification pvq
	ON			pvq.ProviderVenueId = pv.Id
	WHERE Ukprn = @ukPrnToDelete

	DELETE ProviderVenue
	FROM		Provider p
	INNER JOIN	ProviderVenue pv
	ON			pv.ProviderId = p.Id
	WHERE ukprn = @ukPrnToDelete

	DELETE FROM	Provider 
	WHERE ukprn = @ukPrnToDelete

	--Update deployment log
    INSERT INTO [dbo].[DBProjDeployLog]( [Date], [Name], [MD5], [Revision] )
	VALUES( GETUTCDATE(), @scriptName, @TicketNo, 1 );

END