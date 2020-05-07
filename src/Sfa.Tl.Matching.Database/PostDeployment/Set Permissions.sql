--Create role for users that import data
IF DATABASE_PRINCIPAL_ID('DataImporter') IS NULL
BEGIN
	CREATE ROLE [DataImporter]
END

--Allow the DataImporter to alter staging tables so they can trancate data
GRANT ALTER, INSERT, UPDATE, DELETE ON OBJECT::[dbo].[EmployerStaging] TO DataImporter;
GRANT ALTER, INSERT, UPDATE, DELETE ON OBJECT::[dbo].[LearningAimReferenceStaging] TO DataImporter;
GRANT ALTER, INSERT, UPDATE, DELETE ON OBJECT::[dbo].[LocalEnterprisePartnershipStaging] TO DataImporter;
GRANT ALTER, INSERT, UPDATE, DELETE ON OBJECT::[dbo].[PostcodeLookupStaging] TO DataImporter;
GRANT ALTER, INSERT, UPDATE, DELETE ON OBJECT::[dbo].[ProviderReferenceStaging] TO DataImporter;
