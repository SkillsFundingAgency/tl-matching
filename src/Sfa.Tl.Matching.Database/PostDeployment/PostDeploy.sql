/*
Post-Deployment Script
*/
DECLARE @scriptName VARCHAR(255)
DECLARE @TicketNo VARCHAR(32)

:r ".\Seed Routes.sql"
:r ".\Seed Paths.sql"
:r ".\Seed Email Templates.sql"
:r ".\Migrate Opportunity Data.sql"
:r ".\Update Date Columns to UTC.sql"

