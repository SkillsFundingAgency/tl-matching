using System.Collections.Generic;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderQualification.Builders
{
    public class ProviderQualificationBuilder
    {
        public Domain.Models.ProviderQualification Build(bool isDeleted = false) => new Domain.Models.ProviderQualification
        {
            Id = 101,
            ProviderVenueId = 1,
            QualificationId = 100,
            IsDeleted = isDeleted,
            CreatedBy = "CreatedBy",
            ModifiedBy = "ModifiedBy"
        };
    }
}
