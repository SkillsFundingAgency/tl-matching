
namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderQualification.Builders
{
    public class ProviderQualificationBuilder
    {
        public Domain.Models.ProviderQualification Build() => new Domain.Models.ProviderQualification
        {
            Id = 1,
            ProviderVenueId = 1,
            QualificationId = 100,
            CreatedBy = "CreatedBy",
            ModifiedBy = "ModifiedBy"
        };
    }
}
