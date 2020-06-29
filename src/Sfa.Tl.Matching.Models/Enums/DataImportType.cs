using System.ComponentModel.DataAnnotations;
using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Models.Enums
{
    public enum DataImportType
    {
        [Display(Name = "Learning Aim Reference")]
        [FileExtensions(Extensions = FileImportTypeExtensions.Csv)]
        LearningAimReference,

        [Display(Name = "Local Enterprise Partnership")]
        [FileExtensions(Extensions = FileImportTypeExtensions.Csv)]
        LocalEnterprisePartnership,

        [Display(Name = "ONS Postcodes", Description = "Postcodes")]
        [FileExtensions(Extensions = FileImportTypeExtensions.Zip)]
        Postcodes
    }
}