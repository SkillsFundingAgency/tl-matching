--TLWP-1721 - Update qualifications

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET @scriptName = 'CDF 2022 Qualifications Update';
SET @TicketNo  = 'TLWP-1721';

IF NOT EXISTS (SELECT 1 FROM [dbo].[DBProjDeployLog] WHERE [Name] = @scriptName)
BEGIN
	MERGE INTO [dbo].[Qualification] AS Target 
	USING (VALUES 
	  ('CDF00001', N'Agriculture, environmental and animal care', N'agriculture, environmental and animal care', N'AgricultureEnvironmentalAnimalCareagricultureenvironmentalanimalcare', N'agricultureenvironmentalanimalcare', 0),
	  ('CDF00002', N'Business and administration', N'business and administration', N'BusinessAdministrationbusinessadministration', N'businessadministration', 0),
	  ('CDF00003', N'Catering and hospitality', N'catering and hospitality', N'CateringHospitalitycateringhospitality', N'cateringhospitality', 0),
	  ('CDF00004', N'Construction', N'construction', N'Constructionconstruction', N'construction', 0),
	  ('CDF00005', N'Creative and design', N'creative and design', N'CreativeDesigncreativedesign', N'creativedesign', 0),
	  ('CDF00006', N'Digital', 'digital', 'Digitaldigital', 'digital', 0),
	  ('CDF00007', N'Education and childcare', N'education and childcare', N'EducationChildcareeducationchildcare', N'educationchildcare', 0),
	  ('CDF00008', N'Engineering and manufacturing', N'engineering and manufacturing', N'EngineeringManufacturingengineeringmanufacturing', N'engineeringmanufacturing',0),
	  ('CDF00009', N'Hair and beauty', N'hair and beauty', N'HairBeautyhairbeauty', N'hairbeauty', 0),
	  ('CDF00010', N'Health and science', N'health and science', N'HealthSciencehealthscience', N'healthscience' , 0),
	  ('CDF00011', N'Legal, financial and accounting', N'Legal, financial and accounting', N'LegalFinancialAccountinglegalfinancialaccounting', N'legalfinancialaccounting', 0)
	  )
	  AS Source ([LarId],
				 [Title],
				 [ShortTitle],
				 [QualificationSearch],
				 [ShortTitleSearch],
				 [IsDeleted]) 
	ON Target.[LarId] = Source.[LarId] 
	-- Update from Source when Id is Matched
	WHEN MATCHED 
		 AND ((Target.[Title] <> Source.[Title] COLLATE Latin1_General_CS_AS)
		   OR (Target.[ShortTitle] IS NULL AND Source.[ShortTitle] IS NOT NULL)
		   OR (Target.[ShortTitle] <> Source.[ShortTitle] COLLATE Latin1_General_CS_AS) 
		   OR (Target.[QualificationSearch] IS NULL AND Source.[QualificationSearch] IS NOT NULL)
		   OR (Target.[QualificationSearch] <> Source.[QualificationSearch] COLLATE Latin1_General_CS_AS) 
		   OR (Target.[ShortTitleSearch] IS NULL AND Source.[ShortTitleSearch] IS NOT NULL)
		   OR (Target.[ShortTitleSearch] <> Source.[ShortTitleSearch] COLLATE Latin1_General_CS_AS) 
		   OR (Target.[IsDeleted] <> Source.[IsDeleted])) 
	THEN 
	UPDATE SET 
		[Title] = Source.[Title],
		[ShortTitle] = Source.[ShortTitle],
		[QualificationSearch] = Source.[QualificationSearch],
		[ShortTitleSearch] = Source.[ShortTitleSearch],
		[ModifiedOn] = GETDATE(),
		[ModifiedBy] = 'CDF_2022',
		[IsDeleted] = Source.[IsDeleted]
	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([LarId], [Title], [ShortTitle], [QualificationSearch], [ShortTitleSearch], [CreatedBy], [IsDeleted]) 
		VALUES ([LarId], [Title], [ShortTitle], [QualificationSearch], [ShortTitleSearch], 'System', [IsDeleted]) 
	WHEN NOT MATCHED BY SOURCE THEN 
	UPDATE SET 
		[ModifiedOn] = GETDATE(),
		[IsDeleted] = 1;

	--Add links to QualificationRouteMapping
	WITH NewMappingCTE AS (
		SELECT 1 AS [RouteId], [Id] AS [QualificationId] FROM [Qualification] WHERE [LarId] = 'CDF00001'
		UNION
		SELECT 2, [Id] FROM [Qualification] WHERE [LarId] = 'CDF00002'
		UNION
		SELECT 3, [Id] FROM [Qualification] WHERE [LarId] = 'CDF00003'
		UNION
		SELECT 4, [Id] FROM [Qualification] WHERE [LarId] = 'CDF00004'
		UNION
		SELECT 5, [Id] FROM [Qualification] WHERE [LarId] = 'CDF00005'
		UNION
		SELECT 6, [Id] FROM [Qualification] WHERE [LarId] = 'CDF00006'
		UNION
		SELECT 7, [Id] FROM [Qualification] WHERE [LarId] = 'CDF00007'
		UNION
		SELECT 8, [Id] FROM [Qualification] WHERE [LarId] = 'CDF00008'
		UNION
		SELECT 9, [Id] FROM [Qualification] WHERE [LarId] = 'CDF00009'
		UNION
		SELECT 10, [Id] FROM [qualification] WHERE [LarId] = 'CDF00010'
		UNION
		SELECT 11, [Id] FROM [qualification] WHERE [LarId] = 'CDF00011'
	)
	SELECT	cte.RouteId,
			cte.QualificationId,
			'CDF_2022',
			'System'
	FROM [NewMappingCTE] cte
	LEFT JOIN [QualificationRouteMapping] qre
				 ON qre.RouteId = cte.RouteId
				   AND qre.QualificationId = cte.QualificationId
	WHERE qre.[Id] IS NULL;

	--Update deployment log
    INSERT INTO [dbo].[DBProjDeployLog]([Date], [Name], [MD5], [Revision])
	VALUES(GETUTCDATE(), @scriptName, @TicketNo, 1);
END