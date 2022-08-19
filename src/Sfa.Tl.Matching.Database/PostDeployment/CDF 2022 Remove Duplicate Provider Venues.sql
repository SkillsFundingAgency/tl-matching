--TLWP-1720 - Clear out existing data

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET @scriptName = 'CDF 2022 Remove Duplicate Provider Venues';
SET @TicketNo  = 'TLWP-1720';

IF NOT EXISTS (SELECT 1 FROM [dbo].[DBProjDeployLog] WHERE [Name] = @scriptName)
BEGIN

    DECLARE @providerVenueIds TABLE 
	(
		ProviderVenueId int
	);

    WITH cte AS (
        SELECT 
            [Id],
            ROW_NUMBER() OVER (
                PARTITION BY [ProviderId], 
                             [Postcode]
                ORDER BY [ProviderId], 
                         [Postcode]
            ) row_num
         FROM 
            [ProviderVenue]
    )
    INSERT INTO @providerVenueIds
    SELECT [Id] FROM cte
    WHERE row_num > 1;

    DELETE [ProviderQualification] 
    WHERE [ProviderVenueId] IN (SELECT [ProviderVenueId] 
                                FROM @providerVenueIds)

    DELETE [ProviderVenue] 
    WHERE [Id] in (SELECT [ProviderVenueId] 
                   FROM @providerVenueIds)

	--Update deployment log
    INSERT INTO [dbo].[DBProjDeployLog]([Date], [Name], [MD5], [Revision])
	VALUES(GETUTCDATE(), @scriptName, @TicketNo, 1);
END