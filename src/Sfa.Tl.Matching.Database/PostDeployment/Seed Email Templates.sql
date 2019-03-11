/*
Insert initial data for Email Templates
*/

/*
MERGE INTO [dbo].[EmailTemplate] AS Target 
USING (VALUES 
	(N'ProviderReferral', N'5740b7d4-b421-4497-8649-81cd57dbc0b0'),
	(N'EmployerReferral', N'4918d3d5-6694-4f11-975f-e91c255dd583'),
	(N'ProvisionGapReport', N'991bc14d-2d3e-4bd9-b670-304e6fbd85f9')
  )
  AS Source ([TemplateName], [TemplateId]) 
ON Target.[TemplateId] = Source.[TemplateId] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND (Target.[TemplateName] <> Source.[TemplateName]) 
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
*/