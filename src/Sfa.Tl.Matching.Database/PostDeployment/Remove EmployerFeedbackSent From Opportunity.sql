IF EXISTS (SELECT 1 FROM sys.columns WHERE [Name] = 'EmployerFeedbackSentOn' AND OBJECT_ID = OBJECT_ID('[dbo].[Opportunity]'))
BEGIN
    ALTER TABLE Opportunity DROP COLUMN EmployerFeedbackSentOn;
END