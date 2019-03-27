
DELETE FROM [EmailPlaceholder]
DELETE FROM [EmailHistory]
DELETE FROM [Employer]
DELETE FROM [ProviderQualification]
DELETE FROM [Referral]
DELETE FROM [ProviderVenue]
DELETE FROM [Provider]
DELETE FROM [QualificationRoutePathMapping]
DELETE FROM [Qualification]
DELETE FROM [ProvisionGap]
DELETE FROM [Opportunity]
DELETE FROM [FunctionLog]

DBCC CHECKIDENT ('[Employer]', RESEED, 1)
DBCC CHECKIDENT ('[EmailPlaceholder]', RESEED, 1)
DBCC CHECKIDENT ('[EmailHistory]', RESEED, 1)
DBCC CHECKIDENT ('[Employer]', RESEED, 1)
DBCC CHECKIDENT ('[ProviderQualification]', RESEED, 1)
DBCC CHECKIDENT ('[Referral]', RESEED, 1)
DBCC CHECKIDENT ('[ProviderVenue]', RESEED, 1)
DBCC CHECKIDENT ('[Provider]', RESEED, 1)
DBCC CHECKIDENT ('[QualificationRoutePathMapping]', RESEED, 1)
DBCC CHECKIDENT ('[Qualification]', RESEED, 1)
DBCC CHECKIDENT ('[Opportunity]', RESEED, 1)
DBCC CHECKIDENT ('[ProvisionGap]', RESEED, 1)
DBCC CHECKIDENT ('[FunctionLog]', RESEED, 1)
