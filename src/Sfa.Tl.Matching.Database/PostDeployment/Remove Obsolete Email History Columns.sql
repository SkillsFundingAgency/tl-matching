IF EXISTS (SELECT 1 FROM sys.columns 
           WHERE [Name] = 'BlindCopiedTo' 
           AND   OBJECT_ID = OBJECT_ID('[dbo].[EmailHistory]'))
BEGIN
    ALTER TABLE EmailHistory DROP COLUMN BlindCopiedTo;
END

IF EXISTS (SELECT 1 FROM sys.columns 
           WHERE [Name] = 'CopiedTo' 
           AND   OBJECT_ID = OBJECT_ID('[dbo].[EmailHistory]'))
BEGIN
    ALTER TABLE EmailHistory DROP COLUMN CopiedTo;
END