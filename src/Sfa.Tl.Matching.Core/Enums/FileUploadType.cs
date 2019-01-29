using System.ComponentModel;

namespace Sfa.Tl.Matching.Core.Enums
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