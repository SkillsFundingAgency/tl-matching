/*
Post-Deployment Script
*/
DECLARE @scriptName VARCHAR(255)
DECLARE @TicketNo VARCHAR(32)

:r ".\Set Permissions.sql"

:r ".\Seed Routes.sql"
:r ".\Seed Paths.sql"
:r ".\Seed Email Templates.sql"
:r ".\Remove Obsolete Tables.sql"
:r ".\Seed UserProfile.sql"
:r ".\Update Qualification Titles.sql"
:r ".\Deduplicate Qualifications.sql"
