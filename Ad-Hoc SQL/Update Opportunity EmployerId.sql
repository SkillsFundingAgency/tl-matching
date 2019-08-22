SELECT o.id, oi.EmployerId as OLD_Employerid, o.EmployerId as NEW_EmployerId, e.Id, e.CompanyName
FROM Opportunity as o
INNER JOIN OldOpportunity as oi on o.Id = oi.Id
LEFT JOIN Employer as e on oi.EmployerCrmId = e.crmId
WHERE o.EmployerId <> e.Id

update Opportunity
set EmployerId = e.Id
FROM Opportunity as o
INNER JOIN OldOpportunity as oi on o.Id = oi.Id
LEFT JOIN Employer as e on oi.EmployerCrmId = e.crmId
where o.id in (6, 8, 9, 10, 20, 21, 22, 24, 25, 38, 33, 36, 45, 46, 47, 49, 51, 58, 57, 61,
64, 67, 68, 71, 72, 69, 70, 77, 79, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94,
95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114,
115, 116, 118, 119, 120, 117, 124, 134, 151, 150, 152, 162, 140, 163, 166, 176, 165, 179, 185, 184,
187, 180, 190, 203, 201, 199, 194, 195, 196, 197, 198, 204, 210, 211, 212, 206, 224, 216, 220, 222,
217, 218, 219, 214, 215, 228, 231, 233, 236, 230, 229, 225, 235, 227, 251, 244, 245, 252, 247, 243,
248, 249, 260, 261, 262, 265, 256, 263, 255, 273, 274, 275, 270, 272, 280, 268, 283, 269, 278, 276,
290, 294, 287, 293, 288, 292, 291, 300, 297, 296, 301, 299, 302, 461 )

SELECT o.id, oi.EmployerId as OLD_Employerid, o.EmployerId as NEW_EmployerId, e.Id, e.CompanyName
FROM Opportunity as o
INNER JOIN OldOpportunity as oi on o.Id = oi.Id
LEFT JOIN Employer as e on oi.EmployerCrmId = e.crmId
WHERE o.EmployerId <> e.Id
