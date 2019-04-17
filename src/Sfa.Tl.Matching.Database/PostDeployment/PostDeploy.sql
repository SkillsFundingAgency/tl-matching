/*
Post-Deployment Script
*/

:r ".\Seed Routes.sql"
:r ".\Seed Paths.sql"
:r ".\Seed Email Templates.sql"

--TODO: Remove this file and set fields to NOT NULL in Provider and ProviderVenue (after deployed to live)
:r ".\ResetProviderEnabledFields.sql"
