using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Models.Enums
{
    public enum DataImportType
    {
        [Description("Learning Aim Reference")]
        [FileExtensions(Extensions = FileImportTypeExtensions.Csv)]
        LearningAimReference,

        [Description("Local Enterprise Partnership")]
        [FileExtensions(Extensions = FileImportTypeExtensions.Csv)]
        LocalEnterprisePartnership,

        [Description("Postcodes")]
        [DisplayName("ONS Postcodes")]
        [FileExtensions(Extensions = FileImportTypeExtensions.Zip)]
        Postcodes,

        [Description("Provider Venue Qualification")]
        [DisplayName("CDF Provider Update")]
        [FileExtensions(Extensions = FileImportTypeExtensions.Excel)]
        ProviderVenueQualification
    }
}