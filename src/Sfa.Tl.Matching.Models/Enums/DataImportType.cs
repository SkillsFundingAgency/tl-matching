using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Models.Enums
{
    public enum DataImportType
    {
        [Description("Employer CRM data"), FileExtensions(Extensions = FileImportTypeExtensions.Excel)]
        Employer = 1,

        [FileExtensions(Extensions = FileImportTypeExtensions.Excel)]
        Provider,

        [Description("Provider venue"), FileExtensions(Extensions = FileImportTypeExtensions.Excel)]
        ProviderVenue,

        [Description("Provider qualification"), FileExtensions(Extensions = FileImportTypeExtensions.Excel)]
        ProviderQualification,

        [Description("Route & pathway mapping"), FileExtensions(Extensions = FileImportTypeExtensions.Excel)]
        QualificationRoutePathMapping,

        [Description("Learning Aims Reference"), FileExtensions(Extensions = FileImportTypeExtensions.Csv)]
        LearningAimsReference
    }
}