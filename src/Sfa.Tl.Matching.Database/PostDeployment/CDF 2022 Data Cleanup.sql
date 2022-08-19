--TLWP-1720 - Clear out existing data

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET @scriptName = 'CDF 2022 Data Cleanup';
SET @TicketNo  = 'TLWP-1720';

IF NOT EXISTS (SELECT 1 FROM [dbo].[DBProjDeployLog] WHERE [Name] = @scriptName)
BEGIN

	UPDATE [ProviderQualification] 
	SET [IsDeleted] = 1,
		[ModifiedOn] = GETDATE(),
		[ModifiedBy] = 'System'
	WHERE [IsDeleted] = 0

	UPDATE [ProviderVenue] 
	SET [IsRemoved] = 1,
		[ModifiedOn] = GETDATE(),
		[ModifiedBy] = 'System'
	WHERE [IsRemoved] = 0

	UPDATE [Provider] 
	SET [IsCDFProvider] = 0,
		[IsEnabledForReferral] = 0,
		[ModifiedOn] = GETDATE(),
		[ModifiedBy] = 'System'
	WHERE [IsCDFProvider] = 1
	   OR [IsEnabledForReferral] = 1

	DELETE FROM [QualificationRouteMapping]
	UPDATE [Qualification] 
	SET [IsDeleted] = 1,
		[ModifiedOn] = GETDATE(),
		[ModifiedBy] = 'System'
	WHERE [IsDeleted] = 0;

	--Update deployment log
    INSERT INTO [dbo].[DBProjDeployLog]([Date], [Name], [MD5], [Revision])
	VALUES(GETUTCDATE(), @scriptName, @TicketNo, 1);
END