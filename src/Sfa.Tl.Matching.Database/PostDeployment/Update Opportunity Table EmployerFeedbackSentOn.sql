UPDATE Opportunity
SET EmployerFeedbackSentOn = OI.ModifiedOn
FROM
    Opportunity O
JOIN OpportunityItem OI ON OI.OpportunityId = O.Id
WHERE
    OI.EmployerFeedbackSent = 1