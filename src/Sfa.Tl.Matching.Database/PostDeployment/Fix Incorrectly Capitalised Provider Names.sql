--TLWP-1599 - Provider name incorrectly capitalised after apostrophe

UPDATE [dbo].[Provider]
SET [Name] = REPLACE([Name], '''S', '''s')
WHERE [Name] LIKE '%''S%' COLLATE Latin1_General_CS_AS
