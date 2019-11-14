IF EXISTS (SELECT 1 FROM sys.columns WHERE [Name] = 'CompanyType' AND OBJECT_ID = OBJECT_ID('[dbo].[Employer]'))
BEGIN
    ALTER TABLE Employer DROP COLUMN CompanyType;
END

IF EXISTS (SELECT 1 FROM sys.columns WHERE [Name] = 'Postcode' AND OBJECT_ID = OBJECT_ID('[dbo].[Employer]'))
BEGIN
    ALTER TABLE Employer DROP COLUMN Postcode;
END
