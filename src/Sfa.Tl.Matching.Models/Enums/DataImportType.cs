using System.ComponentModel;

// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Models.Enums
{
    public enum DataImportType
    {
        Employer = 1,
        EmployerContacts,
        Provider,
        ProviderContacts,
        Venue,
        Qualification,
        [Description("Route & Pathway")]
        RouteAndPathway
    }
}