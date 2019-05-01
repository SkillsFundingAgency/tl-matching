--TODO: After IsFundedForNextYear is released to live, 
--      set the column in the table to NOT NULL and remove this file

UPDATE [dbo].[Provider]
SET [IsFundedForNextYear] = 1
WHERE [IsFundedForNextYear] IS NULL

ALTER TABLE [dbo].[Provider] ALTER COLUMN [IsFundedForNextYear] BIT NOT NULL;
