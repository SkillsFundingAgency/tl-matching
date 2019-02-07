using System.ComponentModel;

namespace Sfa.Tl.Matching.Models.Enums
{
    public enum DataImportType
    {
        Employer = 1,
        
        Provider,
        
        ProviderVenue,
        
        ProviderQualification,
        
        [Description("Route & Pathway Mapping")]
        QualificationRoutePathMapping
    }
}