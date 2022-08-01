/*
Post-Deployment Script
*/
DECLARE @scriptName VARCHAR(255)
DECLARE @TicketNo VARCHAR(32)

:r ".\Set Permissions.sql"

:r ".\Seed Routes.sql"
:r ".\Seed Paths.sql"
:r ".\Seed Email Templates.sql"
:r ".\Seed UserProfile.sql"
--:r ".\CDF 2022 Data Cleanup.sql"
--:r ".\CDF 2022 Qualifications Update.sql"
