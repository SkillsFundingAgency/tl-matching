using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Models.Enums
{
    public enum DataImportType
    {
        [Description("Employer CRM data")]
        Employer = 1,

        [FileExtensions(Extensions = FileImportTypeExtensions.Excel, ErrorMessage = "Excel")]
        Provider,

        [Description("Provider venue"), FileExtensions(Extensions = FileImportTypeExtensions.Excel, ErrorMessage = "Excel")]
        ProviderVenue,

        [Description("Provider qualification"), FileExtensions(Extensions = FileImportTypeExtensions.Excel, ErrorMessage = "Excel")]
        ProviderQualification,

        [Description("Route & pathway mapping"), FileExtensions(Extensions = FileImportTypeExtensions.Excel, ErrorMessage = "Excel")]
        QualificationRoutePathMapping,

        [Description("Learning Aims Reference"), FileExtensions(Extensions = FileImportTypeExtensions.Csv, ErrorMessage = "Csv")]
        LearningAimsReference
    }
}