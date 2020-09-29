SET NOCOUNT ON;
SET XACT_ABORT ON;
SET @scriptName = 'UpdateQualificationTitles';
SET @TicketNo  = 'TLWP-1220';

IF NOT EXISTS (SELECT 1 FROM [dbo].[DBProjDeployLog] WHERE [Name] = @scriptName )
BEGIN
    update qualification 
    set   Title = 'BTEC Level 3 National Certificate in Enterprise and Entrepreneurship',
          QualificationSearch = 'EnterpriseEntrepreneurshipenterpriseentrepreneurship',
          ModifiedBy = 'System',
          ModifiedOn = GETUTCDATE()
    where larid = '60174134'
    --and   Title = 'BTEC Level 3 National Certificatein Enterprise and Entrepreneurship'

    update qualification 
    set   Title = 'BTEC Level 3 National Extended Certificate in Business',
          QualificationSearch = 'Businessbusiness',
          ModifiedBy = 'System',
          ModifiedOn = GETUTCDATE()
    where larid = '60171595'
    --and   Title = 'BTEC Level 3 National Extended Certificatein Business'

    update qualification 
    set   Title = 'BTEC Level 3 National Certificate in Business',
          QualificationSearch = 'Businessbusiness',
          ModifiedBy = 'System',
          ModifiedOn = GETUTCDATE()
    where larid = '60171558'
    --and   Title = 'BTEC Level 3 National Certificatein Business'

    --select * from qualification where larid in ('60174134','60171595','60171558')

    INSERT INTO [dbo].[DBProjDeployLog]( [Date], [Name], [MD5], [Revision] )
	VALUES( GETUTCDATE(), @scriptName, @TicketNo, 1 );

END