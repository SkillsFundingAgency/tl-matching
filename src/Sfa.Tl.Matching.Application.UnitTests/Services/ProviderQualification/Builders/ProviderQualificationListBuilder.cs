using System.Collections.Generic;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderQualification.Builders
{
    public class ProviderQualificationListBuilder
    {
        public IList<Domain.Models.ProviderQualification> Build() => new List<Domain.Models.ProviderQualification>
        {
            new Domain.Models.ProviderQualification
            {
                Id = 1,
                ProviderVenueId = 1,
                QualificationId = 2,
                CreatedBy = "CreatedBy",
                ModifiedBy = "ModifiedBy"
            }
        };
    }
}
