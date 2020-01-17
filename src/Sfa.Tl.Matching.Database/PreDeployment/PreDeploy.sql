/*
 Pre-Deployment Script
*/

/* Update EmailHistory EmailTemplateId for SecondaryProviderReferral */
DECLARE @PrimaryEmailTemplateId int
SET @PrimaryEmailTemplateId = (SELECT Id 
							FROM EmailTemplate
							WHERE TemplateName = 'ProviderReferralV4')

DECLARE @SecondaryEmailTemplateId int
SET @SecondaryEmailTemplateId = (SELECT Id 
							FROM EmailTemplate
							WHERE TemplateName = 'SecondaryProviderReferral')

UPDATE EmailHistory
SET EmailTemplateId = @SecondaryEmailTemplateId
FROM Provider p
JOIN EmailHistory eh
	ON eh.SentTo = p.SecondaryContactEmail
JOIN EmailTemplate et
	ON et.Id = eh.EmailTemplateId
WHERE eh.EmailTemplateId = @PrimaryEmailTemplateId
	AND p.Id NOT IN (SELECT p1.Id
					FROM Provider p1
					WHERE p1.PrimaryContactEmail = p1.SecondaryContactEmail)