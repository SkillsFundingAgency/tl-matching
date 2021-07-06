--TLWP-1352 - Delete pipeline opportunities for specific users

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET @scriptName = 'CDF2021Cleanup';
SET @TicketNo  = 'TLWP-1471';

IF NOT EXISTS (SELECT 1 FROM [dbo].[DBProjDeployLog] WHERE [Name] = @scriptName )
BEGIN

	DECLARE @updatedByUser NVARCHAR(50) = 'Update Script TLWP-1471'

	DECLARE @providersToRemove TABLE (
	  providerId INT NOT NULL
	)

	DECLARE @providersToRemove TABLE (
	  providerId INT NOT NULL
	)

	declare @users TABLE (
	  username NVARCHAR(50) NOT NULL
	)

	--TODO: Remove provider

	--UPDATE ProviderVenue
	--SET IsDeleted = 1,
	--ModifiedOn = getutcdate(),
	--ModifiedBy = @updatedByUser
	--WHERE Id IN (SELECT ProviderId 
	--			 FROM @providersToRemove)


	--Update deployment log
    INSERT INTO [dbo].[DBProjDeployLog]( [Date], [Name], [MD5], [Revision] )
	VALUES( GETUTCDATE(), @scriptName, @TicketNo, 1 );

END