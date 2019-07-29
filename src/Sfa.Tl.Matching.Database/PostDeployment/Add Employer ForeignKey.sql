SET NOCOUNT ON;
SET XACT_ABORT ON;
IF NOT EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.FK_Opportunity_Employer')
   AND parent_object_id = OBJECT_ID(N'dbo.Opportunity')
)
BEGIN

	ALTER TABLE [dbo].[Opportunity] WITH NOCHECK
    ADD CONSTRAINT [FK_Opportunity_Employer] FOREIGN KEY ([EmployerId]) REFERENCES [dbo].[Employer] ([Id]);

END;