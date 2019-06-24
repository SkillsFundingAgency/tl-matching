SET NOCOUNT ON;
SET XACT_ABORT ON;
SET @scriptName = 'MigrateOpportunityData';
SET  @TicketNo  = 'TLWP-749';

IF NOT EXISTS (SELECT 1 FROM [dbo].[DBProjDeployLog] WHERE [Name] = @scriptName )
BEGIN

	SET IDENTITY_INSERT dbo.Opportunity ON;  

	INSERT INTO Opportunity
	([Id], [EmployerId], [EmployerContact], [EmployerContactEmail], [EmployerContactPhone], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
	SELECT 
	[Id], [EmployerId], [EmployerContact], [EmployerContactEmail], [EmployerContactPhone],	case when [CreatedOn] < '2019-03-31' then [CreatedOn] else DATEADD(hh, -1, [CreatedOn]) end, [CreatedBy], case when [CreatedOn] < '2019-03-31' then [ModifiedOn] else DATEADD(hh, -1, [ModifiedOn]) end, [ModifiedBy]
	FROM OLDOpportunity

	SET IDENTITY_INSERT dbo.Opportunity OFF;  
	SET IDENTITY_INSERT dbo.OpportunityItem ON;  

	INSERT INTO OpportunityItem
	([Id], [OpportunityId], [RouteId], [OpportunityType], [Postcode], [SearchRadius], [SearchResultProviderCount], [JobTitle], [PlacementsKnown], [Placements], [IsSaved],				 [IsSelectedForReferral], [IsCompleted], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])	
	SELECT 
	[Id],  [Id],            [RouteId], 'Referral',        [Postcode], [SearchRadius], [SearchResultProviderCount], [JobTitle], [PlacementsKnown], [Placements], case [ConfirmationSelected] when 1 then 1 else 0 end, case [ConfirmationSelected] when 1 then 1 else 0 end,  case [ConfirmationSelected] when 1 then 1 else 0 end, case when [CreatedOn] < '2019-03-31' then [CreatedOn] else DATEADD(hh, -1, [CreatedOn]) end, [CreatedBy], case when [CreatedOn] < '2019-03-31' then [ModifiedOn] else DATEADD(hh, -1, [ModifiedOn]) end, [ModifiedBy]
	FROM OLDOpportunity

	SET IDENTITY_INSERT dbo.OpportunityItem OFF;  
	
	UPDATE OpportunityItem
	SET 
		[OpportunityType] = 'ProvisionGap',
		[IsSaved] = 1,
		[IsSelectedForReferral] = 0,
		[IsCompleted] = 1
	WHERE
		Id IN (SELECT DISTINCT OpportunityItemId FROM ProvisionGap)

	INSERT INTO [dbo].[DBProjDeployLog]( [Date], [Name], [MD5], [Revision] )
	VALUES( GETUTCDATE(), @scriptName, @TicketNo, 1 );
END;