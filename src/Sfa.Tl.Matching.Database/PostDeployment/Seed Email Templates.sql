/*
Insert initial data for Email Templates
*/

MERGE INTO [dbo].[EmailTemplate] AS Target 
USING (VALUES 
	(N'ProviderReferral', N'f2a7a475-6bbb-4ca7-a010-14d83e9ed90a'),
	(N'EmployerReferral', N'6a65bb26-d4f6-482e-8ece-807c4aaf910b')
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
