CREATE NONCLUSTERED INDEX IX_Employer_CrmId
ON [dbo].[Employer] ([CrmId])
INCLUDE ([CompanyName])
