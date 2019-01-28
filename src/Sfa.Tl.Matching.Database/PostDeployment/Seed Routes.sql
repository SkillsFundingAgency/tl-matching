/*
Insert initial data for Routes and Pathways
*/

SET IDENTITY_INSERT [dbo].[Route] ON

MERGE INTO [dbo].[Route] AS Target 
USING (VALUES 
  (1, N'Agriculture, Environmental and Animal Care'),
  (2, N'Business and Administration'),
  (3, N'Catering and Hospitality'),
  (4, N'Construction'),
  (5, N'Creative and Design'),
  (6, N'Digital'),
  (7, N'Education & Childcare'),
  (8, N'Engineering and Manufacturing'),
  (9, N'Hair and Beauty'),
  (10, N'Health and Science'),
  (11, N'Legal, Financial and Accounting')
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

