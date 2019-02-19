/*
Insert initial data for Routes and Pathways
*/

SET IDENTITY_INSERT [dbo].[Route] ON

MERGE INTO [dbo].[Route] AS Target 
USING (VALUES 
  (1, N'Agriculture, environmental and animal care'),
  (2, N'Business and administration'),
  (3, N'Catering and hospitality'),
  (4, N'Construction'),
  (5, N'Creative and design'),
  (6, N'Digital'),
  (7, N'Education and childcare'),
  (8, N'Engineering and manufacturing'),
  (9, N'Hair and beauty'),
  (10, N'Health and science'),
  (11, N'Legal, financial and accounting'),
  (12, N'Care services'),
  (13, N'Protective services'),
  (14, N'Sales, marketing and procurement'),
  (15, N'Transport and logistics')
  )
  AS Source ([Id], [Name]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND (Target.[Name] <> Source.[Name]) 
THEN 
UPDATE SET 
	[Name] = Source.[Name],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [Name], [CreatedBy]) 
	VALUES ([Id], [Name], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[Route] OFF

