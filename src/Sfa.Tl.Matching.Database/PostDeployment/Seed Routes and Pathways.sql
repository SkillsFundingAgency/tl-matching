/*
Insert initial data for Routes and Pathways
*/

MERGE INTO [dbo].[RoutePathLookup] AS Target 
USING (VALUES 
  (1, N'Agriculture, Environmental and Animal Care', N'Agriculture, Land Management and Production'),
  (2, N'Agriculture, Environmental and Animal Care', N'Animal are and Management'),
  (3, N'Business and Administration', N'Human Resources'),
  (4, N'Business and Administration', N'Management and Administration'),
  (5, N'Catering and Hospitality', N'Catering'),
  (6, N'Construction', N'Building Services Engineering'),
  (7, N'Construction', N'Design, Surveying & Planning'),
  (8, N'Construction', N'Onsite Construction'),
  (9, N'Creative and Design', N'Craft and Design'),
  (10, N'Creative and Design', N'Cultural Heritage and Visitor Attractions'),
  (11, N'Creative and Design', N'Music, Broadcast and Production'),
  (12, N'Digital', N'Data and Digital Business Services'),
  (13, N'Digital', N'IT Support and Services'),
  (14, N'Digital', N'Software and Applications Design and Development'),
  (15, N'Education & Childcare', N'Education'),
  (16, N'Engineering and Manufacturing', N'Design, Development and Control'),
  (17, N'Engineering and Manufacturing', N'Maintenance, Installation and Repair'),
  (18, N'Engineering and Manufacturing', N'Manufacturing and Process'),
  (19, N'Hair and Beauty', N'Hair, Beauty and Aesthetics'),
  (20, N'Health and Science', N'Health'),
  (21, N'Health and Science', N'Healthcare Science'),
  (22, N'Health and Science', N'Science'),
  (23, N'Legal, Financial and Accounting', N'Accounting'),
  (24, N'Legal, Financial and Accounting', N'Financial'),
  (25, N'Legal, Financial and Accounting', N'Legal')
  )
  AS Source ([Id], [Route], [Path]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND (Target.[Route] <> Source.[Route] 
	      OR Target.[Path] <> Source.[Path]) 
THEN 
UPDATE SET 
	[Route] = Source.[Route]
	,[Path] = Source.[Path]
	,[ModifiedOn] = GETDATE()
	,[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [Route], [Path], [CreatedBy]) 
	VALUES ([Id], [Route], [Path], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;
