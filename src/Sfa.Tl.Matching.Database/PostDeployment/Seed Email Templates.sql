﻿/*
Insert initial data for Email Templates
*/

MERGE INTO [dbo].[EmailTemplate] AS Target 
USING (VALUES 
	(N'EmployerReferral', N'4918d3d5-6694-4f11-975f-e91c255dd583'),
	(N'EmployerFeedback', N'deb927d5-1fd3-43d3-9aae-fb826f2e77f8'),
	(N'EmployerFeedbackV2', N'b0e07462-53a7-4b61-8b17-e3f3f69f13be'),
	(N'EmployerFeedbackV3', N'e6f04398-a0a2-46b7-ba7d-b265d41fb45e'),
	(N'ProviderReferral', N'5740b7d4-b421-4497-8649-81cd57dbc0b0'),
	(N'ProviderQuarterlyUpdate', N'714e5adb-8f08-4b25-9be8-cb2f3fc66ed6'),
	(N'EmployerReferralComplex', N'fed633ff-cf62-4060-9816-df036c89ba03'),
	(N'ProviderReferralComplex', N'cb4aea6c-5293-43a2-b8ad-f183f8bd7cea'),
	(N'ProviderReferralV3', N'0868baf1-0ef0-4976-8d77-da19e3d79761'),
	(N'EmployerReferralV3', N'66fffc15-2a48-4143-a729-b484f5c26980'),
	(N'ProviderReferralV4', N'b5de544b-60f7-42c2-9859-73c5db3dee5d'),
	(N'EmployerReferralV4', N'1a69302f-de7d-45d4-89f3-68be12f7fe08'),
	(N'ProviderReferralV5', N'33b65be0-b98c-41ae-94bb-e0723f8fbd24'),
	(N'EmployerReferralV5', N'7f34b79c-ca08-4132-99e2-5727e6a9e9e3'),
	(N'ProviderReferralV6', N'3a8f8852-799e-4f15-af30-00cc4389b76d'),
	(N'EmployerReferralV6', N'11d991db-8dcb-4a80-8205-ede9d471013a'),
	(N'ProviderFeedback', N'8249a0b6-40a5-46dd-a370-13c6a9651d2c'),
	(N'ProviderFeedbackV2', N'bf90646f-8c95-4c8e-a770-567af1213a4d'),
	(N'ProviderFeedbackV3', N'48358bd6-248a-4846-9754-df579f695584'),
	(N'EmployerAupaBlank', N'18c53f52-25b3-4f77-89fc-582dc545d0ed'),
	(N'FailedEmail', N'1489537d-d026-4da5-9d7d-cc979e9f0a28'),
	(N'FailedEmailV2', N'374d9de8-84e6-44d2-b844-56e21fc1d457'),
	(N'EmailDeliveryStatus', N'338154c4-7306-4cbf-97f9-1b9ded6a579d')	
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
