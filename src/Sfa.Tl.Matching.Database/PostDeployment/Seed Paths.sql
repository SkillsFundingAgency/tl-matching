/*
Insert initial data for Routes and Pathways
*/

SET IDENTITY_INSERT [dbo].[Path] ON

MERGE INTO [dbo].[Path] AS Target 
USING (VALUES 
  (1, 1, N'Agriculture, land management and production'),
  (2, 1, N'Animal care and management'),
  (3, 2, N'Human resources'),
  (4, 2, N'Management and administration'),
  (5, 3, N'Hospitality'),
  (6, 3, N'Catering'),
  (7, 4, N'Building services engineering'),
  (8, 4, N'Design, surveying and planning'),
  (9, 4, N'Onsite construction'),
  (10, 5, N'Craft and design'),
  (11, 5, N'Cultural heritage and visitor attractions'),
  (12, 5, N'Media, broadcast and production'),
  (13, 6, N'Digital production, design and development'),
  (14, 6, N'Digital support and services'),
  (15, 6, N'Digital business services'),
  (16, 7, N'Education'),
  (17, 8, N'Design, development and control'),
  (18, 8, N'Maintenance, installation and repair'),
  (19, 8, N'Manufacturing and process'),
  (20, 9, N'Hair, beauty and aesthetics'),
  (21, 10, N'Health'),
  (22, 10, N'Healthcare science'),
  (23, 10, N'Science'),
  (24, 10, N'Community exercise, fitness and health'),
  (25, 11, N'Accountancy'),
  (26, 11, N'Financial'),
  (27, 11, N'Legal'),
  (28, 12, N'Care services'),
  (29, 13, N'Protective services'),
  (30, 14, N'Customer service'),
  (31, 14, N'Marketing'),
  (32, 14, N'Procurement'),
  (33, 14, N'Retail'),
  (34, 15, N'Transport'),
  (35, 15, N'Logistics')
  )
  AS Source ([Id], [RouteId], [Name]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
	   OR (Target.[RouteId] <> Source.[RouteId])) 
THEN 
UPDATE SET 
	[Name] = Source.[Name],
	[RouteId] = Source.[RouteId],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [RouteId], [Name], [CreatedBy]) 
	VALUES ([Id], [RouteId], [Name], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[Path] OFF
