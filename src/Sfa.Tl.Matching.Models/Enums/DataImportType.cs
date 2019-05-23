using System.ComponentModel;

namespace Sfa.Tl.Matching.Models.Enums
{
    public enum DataImportType
    {
        [Description("Employer CRM data")]
        Employer = 1,
        
        Provider,

        [Description("Provider venue")]
        ProviderVenue,

        [Description("Provider qualification")]
        ProviderQualification,
    }
}