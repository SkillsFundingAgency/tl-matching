using System.ComponentModel;

namespace Sfa.Tl.Matching.Models.Enums
{
    public enum EmailDeliveryStatusType
    {
        [Description("Email address does not exist")]
        PermanentFailure = 1,
        [Description("Inbox not accepting messages right now")]
        TemporaryFailure,
        [Description("Problem between Notify and the provider")]
        TechnicalFailure
    }
}