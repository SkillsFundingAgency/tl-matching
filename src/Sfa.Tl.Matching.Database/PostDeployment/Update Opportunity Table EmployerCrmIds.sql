UPDATE Opportunity
SET Opportunity.EmployerCrmId = e.CrmId
FROM Opportunity o
JOIN Employer e
	ON o.EmployerId = e.Id
WHERE o.EmployerCrmId IS NULL;