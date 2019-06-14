using System.ComponentModel;

// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Api.Clients.ProviderReference
{
    public enum UkRlpRecordStatus
    {
        [Description("A")]
        Active,
        [Description("V")]
        Verified,
        [Description("PD1")]
        DeactivationInProcess,
        [Description("PD2")]
        DeactivateComplete
    }
}