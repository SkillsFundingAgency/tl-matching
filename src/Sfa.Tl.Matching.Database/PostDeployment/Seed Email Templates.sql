/*
Insert initial data for Email Templates
*/

MERGE INTO [dbo].[EmailTemplate] AS Target 
USING (VALUES 
	(N'EmployerReferral', N'4918d3d5-6694-4f11-975f-e91c255dd583'),
	(N'ProviderReferral', N'5740b7d4-b421-4497-8649-81cd57dbc0b0'),
	(N'ProviderQuarterlyUpdate', N'ad67589c-5396-476f-bc7b-20b2dc42b600')
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
