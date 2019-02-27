namespace Sfa.Tl.Matching.Models.Enums
{
    public enum ValidationErrorCode
    {
        WrongDataType = 1,
        WrongNumberOfColumns,
        MissingMandatoryData,
        InvalidFormat,
        InvalidLength,
        ColumnValueDoesNotMatchType,
        
        ProviderAlreadyExists,
        ProviderVenueAlreadyExists,
        ProviderQualificationAlreadyExists,
        QualificationRoutePathMappingAlreadyExists,

        ProviderDoesntExist,
        ProviderVenueDoesntExist,
        QualificationDoesntExist
    }
}