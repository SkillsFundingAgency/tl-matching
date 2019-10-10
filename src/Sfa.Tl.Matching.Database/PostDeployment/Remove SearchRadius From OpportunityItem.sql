IF EXISTS (SELECT 1 FROM sys.columns WHERE [Name] = 'SearchRadius' AND  OBJECT_ID = OBJECT_ID('[dbo].[OpportunityItem]'))
BEGIN
	ALTER TABLE OpportunityItem DROP COLUMN SearchRadius;
END
