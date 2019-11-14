/*
Post-Deployment Script
*/
DECLARE @scriptName VARCHAR(255)
DECLARE @TicketNo VARCHAR(32)

:r ".\Seed Routes.sql"
:r ".\Seed Paths.sql"
:r ".\Seed Email Templates.sql"
:r ".\Remove Obsolete Tables.sql"
:r ".\Seed UserProfile.sql"

:r ".\Remove EmployerId from Opportunity.sql"
:r ".\Remove Postcode and CompanyType from Employer.sql"
