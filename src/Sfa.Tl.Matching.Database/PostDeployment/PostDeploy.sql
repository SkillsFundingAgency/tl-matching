/*
Post-Deployment Script
*/
DECLARE @scriptName VARCHAR(255)
DECLARE @TicketNo VARCHAR(32)

:r ".\Seed Routes.sql"
:r ".\Seed Paths.sql"
:r ".\Seed Email Templates.sql"
:r ".\Backfill Provider Venue Names.sql"
:r ".\Remove Obsolete Tables.sql"
:r ".\Update Qualification Title length.sql"
:r ".\Backfill Employer Feedback Sent.sql"
:r ".\Update Opportunity Table EmployerCrmIds.sql"
:r ".\Update Opportunity Table EmployerFeedbackSentOn.sql"
:r ".\Remove EmployerFeedbackSent From Opportunity.sql"