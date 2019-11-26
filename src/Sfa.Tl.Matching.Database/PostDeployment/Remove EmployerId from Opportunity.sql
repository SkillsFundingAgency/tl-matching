IF EXISTS (SELECT 1 FROM sys.columns WHERE [Name] = 'EmployerId' AND OBJECT_ID = OBJECT_ID('[dbo].[Opportunity]'))
BEGIN
    ALTER TABLE Opportunity DROP COLUMN EmployerId;
END
