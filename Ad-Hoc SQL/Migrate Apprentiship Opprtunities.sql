SELECT * FROM [Route]

--DELETE FROM [Route] WHERE Id IN (12, 13, 14, 15)

SELECT * FROM QualificationRouteMapping WHERE RouteId IN (12, 13, 14, 15)

--DELETE FROM QualificationRouteMapping WHERE Id IN (12, 13, 14, 15)

SELECT 
    OI.Id,
    RouteId,
    CASE 
        WHEN routeid = 12 THEN 'Care services'
        WHEN routeid = 13 THEN 'Protective services'
        WHEN routeid = 14 THEN 'Sales, marketing and procurement'
        WHEN routeid = 15 THEN 'Transport and logistics' 
    END AS RouteName,
    COUNT(ProviderVenueId) ProviderCount,
    e.CompanyName,
    oi.JobRole,
    oi.Placements
FROM Opportunity AS O
INNER JOIN OpportunityItem AS OI ON o.id = oi.OpportunityId
INNER JOIN Referral AS R on oi.Id = r.OpportunityItemId
INNER JOIN Employer AS E ON o.EmployerId = e.Id
WHERE RouteId in (12, 13, 14, 15)
GROUP BY OI.Id, routeid, CompanyName, JobRole, Placements
ORDER BY OI.Id

SELECT * FROM OpportunityItem WHERE Id IN (25, 77, 79, 117, 195, 196, 225, 248, 268, 280, 299, 312, 327, 343, 359, 360, 368, 385, 388, 396, 404, 431, 455, 458, 464, 468, 469, 492, 516, 519, 532, 548, 550, 559, 585, 590, 604, 605, 606, 693, 796, 801, 806, 815, 817, 828, 851, 876, 887, 895, 910, 922, 943, 945, 948, 961, 1123, 1126, 1163, 1176, 1198, 1211, 1223, 1232, 1233, 1234, 1239, 1300, 1335, 1340, 1361, 1363, 1382, 1458, 1462)