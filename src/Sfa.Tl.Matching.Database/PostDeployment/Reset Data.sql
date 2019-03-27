
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

DBCC CHECKIDENT ('[Employer]', RESEED, 0)
DBCC CHECKIDENT ('[EmailPlaceholder]', RESEED, 0)
DBCC CHECKIDENT ('[EmailHistory]', RESEED, 0)
DBCC CHECKIDENT ('[Employer]', RESEED, 0)
DBCC CHECKIDENT ('[ProviderQualification]', RESEED, 0)
DBCC CHECKIDENT ('[Referral]', RESEED, 0)
DBCC CHECKIDENT ('[ProviderVenue]', RESEED, 0)
DBCC CHECKIDENT ('[Provider]', RESEED, 0)
DBCC CHECKIDENT ('[QualificationRoutePathMapping]', RESEED, 0)
DBCC CHECKIDENT ('[Qualification]', RESEED, 0)
DBCC CHECKIDENT ('[Opportunity]', RESEED, 0)
DBCC CHECKIDENT ('[ProvisionGap]', RESEED, 0)
DBCC CHECKIDENT ('[FunctionLog]', RESEED, 0)
