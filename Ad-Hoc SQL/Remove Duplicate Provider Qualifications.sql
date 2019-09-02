SELECT 
    Id,
    ProviderVenueId, 
    QualificationId, 
    RowNumber
FROM 
(
    SELECT 
        Id,
        ProviderVenueId, 
        QualificationId, 
        ROW_NUMBER() OVER (PARTITION BY ProviderVenueId, QualificationId ORDER BY ProviderVenueId, QualificationId) RowNumber 
    FROM ProviderQualification
) AS CTE
WHERE RowNumber > 1

DELETE FROM CTE
FROM
(
    SELECT 
        Id,
        ProviderVenueId, 
        QualificationId, 
        ROW_NUMBER() OVER (PARTITION BY ProviderVenueId, QualificationId ORDER BY ProviderVenueId, QualificationId) RowNumber 
    FROM ProviderQualification
) AS CTE
WHERE RowNumber > 1