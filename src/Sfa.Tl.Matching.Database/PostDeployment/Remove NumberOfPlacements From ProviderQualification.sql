IF EXISTS (SELECT 1 FROM sys.columns WHERE [Name] = 'NumberOfPlacements' AND OBJECT_ID = OBJECT_ID('[dbo].[ProviderQualification]'))
BEGIN
    ALTER TABLE ProviderQualification DROP COLUMN NumberOfPlacements;
END
