IF EXISTS (SELECT 1 FROM sys.columns WHERE [Name] = 'EmployerFeedbackSent' AND  OBJECT_ID = OBJECT_ID('[dbo].[OpportunityItem]'))
	ALTER TABLE [dbo].[OpportunityItem] DROP COLUMN [EmployerFeedbackSent]
