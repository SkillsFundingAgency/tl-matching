using System.ComponentModel;

namespace Sfa.Tl.Matching.Models.Enums
{
    public enum ValidationErrorCode
    {
        [Description("Wrong data type")]
        WrongDataType = 1,
        [Description("Wrong number of columns")]
        WrongNumberOfColumns,
        [Description("Missing mandatory data")]
        MissingMandatoryData,
        [Description("Invalid format")]
        InvalidFormat
    }
}