SET NOCOUNT ON;
SET XACT_ABORT ON;
DECLARE @scriptName VARCHAR(255)= 'UpdateDateColumnstoUTC';
DECLARE @TicketNo VARCHAR(32)= 'TLWP-679';

IF NOT EXISTS (SELECT 1 FROM [dbo].[DBProjDeployLog] WHERE [Name] = @scriptName )
BEGIN

 UPDATE BackgroundProcessHistory
 SET [CreatedOn] = CASE WHEN [CreatedOn] < '2019-03-31' THEN [CreatedOn] ELSE DATEADD(hh, -1, [CreatedOn]) END,
    [ModifiedOn] = CASE WHEN [ModifiedOn] < '2019-03-31' THEN [ModifiedOn] ELSE DATEADD(hh, -1, [ModifiedOn]) END

 UPDATE EmailHistory
 SET [CreatedOn] = CASE WHEN [CreatedOn] < '2019-03-31' THEN [CreatedOn] ELSE DATEADD(hh, -1, [CreatedOn]) END,
    [ModifiedOn] = CASE WHEN [ModifiedOn] < '2019-03-31' THEN [ModifiedOn] ELSE DATEADD(hh, -1, [ModifiedOn]) END

 UPDATE EmailPlaceholder
 SET [CreatedOn] = CASE WHEN [CreatedOn] < '2019-03-31' THEN [CreatedOn] ELSE DATEADD(hh, -1, [CreatedOn]) END,
    [ModifiedOn] = CASE WHEN [ModifiedOn] < '2019-03-31' THEN [ModifiedOn] ELSE DATEADD(hh, -1, [ModifiedOn]) END

 UPDATE EmailTemplate
 SET [CreatedOn] = CASE WHEN [CreatedOn] < '2019-03-31' THEN [CreatedOn] ELSE DATEADD(hh, -1, [CreatedOn]) END,
    [ModifiedOn] = CASE WHEN [ModifiedOn] < '2019-03-31' THEN [ModifiedOn] ELSE DATEADD(hh, -1, [ModifiedOn]) END

 UPDATE Employer
 SET [CreatedOn] = CASE WHEN [CreatedOn] < '2019-03-31' THEN [CreatedOn] ELSE DATEADD(hh, -1, [CreatedOn]) END,
    [ModifiedOn] = CASE WHEN [ModifiedOn] < '2019-03-31' THEN [ModifiedOn] ELSE DATEADD(hh, -1, [ModifiedOn]) END

 UPDATE FunctionLog
 SET [CreatedOn] = CASE WHEN [CreatedOn] < '2019-03-31' THEN [CreatedOn] ELSE DATEADD(hh, -1, [CreatedOn]) END,
    [ModifiedOn] = CASE WHEN [ModifiedOn] < '2019-03-31' THEN [ModifiedOn] ELSE DATEADD(hh, -1, [ModifiedOn]) END

 UPDATE LearningAimReference
 SET [CreatedOn] = CASE WHEN [CreatedOn] < '2019-03-31' THEN [CreatedOn] ELSE DATEADD(hh, -1, [CreatedOn]) END,
    [ModifiedOn] = CASE WHEN [ModifiedOn] < '2019-03-31' THEN [ModifiedOn] ELSE DATEADD(hh, -1, [ModifiedOn]) END

 UPDATE [Path]
 SET [CreatedOn] = CASE WHEN [CreatedOn] < '2019-03-31' THEN [CreatedOn] ELSE DATEADD(hh, -1, [CreatedOn]) END,
    [ModifiedOn] = CASE WHEN [ModifiedOn] < '2019-03-31' THEN [ModifiedOn] ELSE DATEADD(hh, -1, [ModifiedOn]) END

 UPDATE [Provider]
 SET [CreatedOn] = CASE WHEN [CreatedOn] < '2019-03-31' THEN [CreatedOn] ELSE DATEADD(hh, -1, [CreatedOn]) END,
    [ModifiedOn] = CASE WHEN [ModifiedOn] < '2019-03-31' THEN [ModifiedOn] ELSE DATEADD(hh, -1, [ModifiedOn]) END

 UPDATE ProviderQualification
 SET [CreatedOn] = CASE WHEN [CreatedOn] < '2019-03-31' THEN [CreatedOn] ELSE DATEADD(hh, -1, [CreatedOn]) END,
    [ModifiedOn] = CASE WHEN [ModifiedOn] < '2019-03-31' THEN [ModifiedOn] ELSE DATEADD(hh, -1, [ModifiedOn]) END

 UPDATE ProviderReference
 SET [CreatedOn] = CASE WHEN [CreatedOn] < '2019-03-31' THEN [CreatedOn] ELSE DATEADD(hh, -1, [CreatedOn]) END,
    [ModifiedOn] = CASE WHEN [ModifiedOn] < '2019-03-31' THEN [ModifiedOn] ELSE DATEADD(hh, -1, [ModifiedOn]) END

 UPDATE ProviderVenue
 SET [CreatedOn] = CASE WHEN [CreatedOn] < '2019-03-31' THEN [CreatedOn] ELSE DATEADD(hh, -1, [CreatedOn]) END,
    [ModifiedOn] = CASE WHEN [ModifiedOn] < '2019-03-31' THEN [ModifiedOn] ELSE DATEADD(hh, -1, [ModifiedOn]) END

 UPDATE ProvisionGap
 SET [CreatedOn] = CASE WHEN [CreatedOn] < '2019-03-31' THEN [CreatedOn] ELSE DATEADD(hh, -1, [CreatedOn]) END,
    [ModifiedOn] = CASE WHEN [ModifiedOn] < '2019-03-31' THEN [ModifiedOn] ELSE DATEADD(hh, -1, [ModifiedOn]) END

 UPDATE Qualification
 SET [CreatedOn] = CASE WHEN [CreatedOn] < '2019-03-31' THEN [CreatedOn] ELSE DATEADD(hh, -1, [CreatedOn]) END,
    [ModifiedOn] = CASE WHEN [ModifiedOn] < '2019-03-31' THEN [ModifiedOn] ELSE DATEADD(hh, -1, [ModifiedOn]) END

 UPDATE QualificationRoutePathMapping
 SET [CreatedOn] = CASE WHEN [CreatedOn] < '2019-03-31' THEN [CreatedOn] ELSE DATEADD(hh, -1, [CreatedOn]) END,
    [ModifiedOn] = CASE WHEN [ModifiedOn] < '2019-03-31' THEN [ModifiedOn] ELSE DATEADD(hh, -1, [ModifiedOn]) END

 UPDATE Referral
 SET [CreatedOn] = CASE WHEN [CreatedOn] < '2019-03-31' THEN [CreatedOn] ELSE DATEADD(hh, -1, [CreatedOn]) END,
    [ModifiedOn] = CASE WHEN [ModifiedOn] < '2019-03-31' THEN [ModifiedOn] ELSE DATEADD(hh, -1, [ModifiedOn]) END

 UPDATE [Route]
 SET [CreatedOn] = CASE WHEN [CreatedOn] < '2019-03-31' THEN [CreatedOn] ELSE DATEADD(hh, -1, [CreatedOn]) END,
    [ModifiedOn] = CASE WHEN [ModifiedOn] < '2019-03-31' THEN [ModifiedOn] ELSE DATEADD(hh, -1, [ModifiedOn]) END

	INSERT INTO [dbo].[DBProjDeployLog]( [Date], [Name], [MD5], [Revision] )
	VALUES( GETUTCDATE(), @scriptName, @TicketNo, 1 );

END;