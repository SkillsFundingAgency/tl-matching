using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Models.Enums
{
    public enum DataImportType
    {
        [Description("Employer CRM data"), FileExtensions(Extensions = FileImportTypeExtensions.Excel)]
        Employer = 1,

        [Description("Learning Aim Reference"), FileExtensions(Extensions = FileImportTypeExtensions.Csv)]
        LearningAimReference
    }
}