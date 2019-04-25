/*
Insert initial data for Email Templates
*/

MERGE INTO [dbo].[EmailTemplate] AS Target 
USING (VALUES 
	(N'EmployerReferral', N'4918d3d5-6694-4f11-975f-e91c255dd583'),
	(N'ProviderReferral', N'5740b7d4-b421-4497-8649-81cd57dbc0b0'),
	(N'ProviderQuarterlyUpdate', N'714e5adb-8f08-4b25-9be8-cb2f3fc66ed6')
  )
  AS Source ([TemplateName], [TemplateId]) 
ON Target.[TemplateName] = Source.[TemplateName] 
-- Update from Source when TemplateName is Matched
WHEN MATCHED 
	 AND (Target.[TemplateId] <> Source.[TemplateId]) 
THEN 
UPDATE SET 
	[TemplateName] = Source.[TemplateName],
	[TemplateId] = Source.[TemplateId],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([TemplateName], [TemplateId], [CreatedBy]) 
	VALUES ([TemplateName], [TemplateId], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;
