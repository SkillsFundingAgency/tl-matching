/*
Insert initial data for Routes and Pathways
*/

SET IDENTITY_INSERT [dbo].[Route] ON

MERGE INTO [dbo].[Route] AS Target 
USING (VALUES 
  (1, N'Agriculture, environmental and animal care', 'Includes: animal care and management; agriculture, land management and production', 0),
  (2, N'Business and administration', 'Includes: management and administration; human resources (HR)', 0),
  (3, N'Catering and hospitality', 'Includes: preparing and serving food and drink; conferences and events', 0),
  (4, N'Construction', 'Includes: design, surveying and planning; onsite construction; building services engineering', 0),
  (5, N'Creative and design', 'Includes: craft and design; media, broadcast and production; cultural heritage and visitor attractions', 0),
  (6, N'Digital', 'Includes: digital support and services; digital production design and development; digital business services', 0),
  (7, N'Education and childcare', 'Includes: early education and childcare work; teaching', 0),
  (8, N'Engineering and manufacturing', 'Includes: engineering design, development and control; engineering, manufacturing and process; maintenance, installation and repair',0),
  (9, N'Hair and beauty', 'Includes: hair, beauty and aesthetics', 0),
  (10, N'Health and science', 'Includes: health and healthcare science', 0),
  (11, N'Legal, financial and accounting', 'Includes: legal, financial services and accountancy', 0),
  (12, N'Care services', 'Includes: support work for children, young people and families; adult care work', 1),
  (13, N'Protective services', 'Includes: emergency services; health and safety; security personnel; armed forces', 1),
  (14, N'Sales, marketing and procurement', 'Includes: customer service; marketing; procurement; retail', 1),
  (15, N'Transport and logistics', 'Includes: transport, supply chain; logistics', 1)
  )
  AS Source ([Id], [Name], [Summary],[IsDeleted]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
	   OR (Target.[Summary] IS NULL AND Source.[Summary] IS NOT NULL)
	   OR (Target.[Summary] <> Source.[Summary] COLLATE Latin1_General_CS_AS) 
	   OR (Target.[IsDeleted] <> Source.[IsDeleted])) 
THEN 
UPDATE SET 
	[Name] = Source.[Name],
	[Summary] = Source.[Summary],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System',
    [IsDeleted] = Source.[IsDeleted]
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [Name], [Summary], [CreatedBy], [IsDeleted]) 
	VALUES ([Id], [Name], [Summary], 'System', [IsDeleted]) 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[Route] OFF
