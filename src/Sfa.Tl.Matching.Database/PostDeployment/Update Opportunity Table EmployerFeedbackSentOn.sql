IF EXISTS (SELECT 1 FROM sys.columns WHERE [Name] = 'EmployerFeedbackSent' AND  OBJECT_ID = OBJECT_ID('[dbo].[OpportunityItem]'))
BEGIN
	UPDATE Opportunity
	SET EmployerFeedbackSentOn = OI.ModifiedOn
	FROM
		Opportunity O
	JOIN OpportunityItem OI ON OI.OpportunityId = O.Id
	WHERE
	    OI.EmployerFeedbackSent = 1
END