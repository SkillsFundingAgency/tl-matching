using System.ComponentModel;

namespace Sfa.Tl.Matching.Domain.Enums
{
    public enum FileUploadType
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