using System.ComponentModel;

// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Infrastructure.Enums
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