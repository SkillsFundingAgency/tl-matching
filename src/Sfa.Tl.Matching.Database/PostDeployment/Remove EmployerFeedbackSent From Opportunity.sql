IF EXISTS (SELECT 1 FROM sys.columns WHERE [Name] = 'EmployerFeedbackSent' AND  OBJECT_ID = OBJECT_ID('[dbo].[OpportunityItem]'))
BEGIN
	DECLARE @sql NVARCHAR(MAX)
	
	IF EXISTS (SELECT 1 FROM sys.default_constraints dc
					JOIN sys.columns c
					ON c.default_object_id = dc.object_id
					WHERE dc.parent_object_id = OBJECT_ID('[OpportunityItem]') AND c.name = N'EmployerFeedbackSent')
    BEGIN
		
		SELECT @sql = N'alter table [OpportunityItem] drop constraint ['+dc.name+N']'
								FROM sys.default_constraints dc
								JOIN sys.columns c
								ON c.default_object_id = dc.object_id
								WHERE dc.parent_object_id = OBJECT_ID('[OpportunityItem]') AND c.name = N'EmployerFeedbackSent'

		EXEC (@sql)
	END
		
	ALTER TABLE OpportunityItem DROP COLUMN ProviderFeedbackSent;

END

