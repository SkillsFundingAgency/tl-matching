/*
Insert initial data for Routes and Pathways
*/

SET IDENTITY_INSERT [dbo].[Path] ON

MERGE INTO [dbo].[Path] AS Target 
USING (VALUES 
  (1, 1, N'Agriculture, Land Management and Production'),
  (2, 1, N'Animal are and Management'),
  (3, 2, N'Human Resources'),
  (4, 2, N'Management and Administration'),
  (5, 3,  N'Catering'),
  (6, 4, N'Building Services Engineering'),
  (7, 4, N'Design, Surveying & Planning'),
  (8, 4, N'Onsite Construction'),
  (9, 5, N'Craft and Design'),
  (10, 5, N'Cultural Heritage and Visitor Attractions'),
  (11, 5, N'Music, Broadcast and Production'),
  (12, 6, N'Data and Digital Business Services'),
  (13, 6, N'IT Support and Services'),
  (14, 6, N'Software and Applications Design and Development'),
  (15, 7, N'Education'),
  (16, 8, N'Design, Development and Control'),
  (17, 8, N'Maintenance, Installation and Repair'),
  (18, 8, N'Manufacturing and Process'),
  (19, 9, N'Hair, Beauty and Aesthetics'),
  (20, 10, N'Health'),
  (21, 10, N'Healthcare Science'),
  (22, 10, N'Science'),
  (23, 11, N'Accounting'),
  (24, 11, N'Financial'),
  (25, 11, N'Legal')
  )
  AS Source ([Id], [RouteId], [Name]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND (Target.[Name] <> Source.[Name] 
	 OR Target.[RouteId] <> Source.[RouteId]) 
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

