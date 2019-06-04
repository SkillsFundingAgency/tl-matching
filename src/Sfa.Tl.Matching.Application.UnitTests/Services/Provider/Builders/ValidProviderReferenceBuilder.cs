using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders
{
    public class ValidProviderReferenceBuilder
    {
        public ProviderReference Build() => new ProviderReference
        {
            Id = 1,
            UkPrn = 10000546,
            Name = "Test Provider",
            CreatedBy = "CreatedBy",
            ModifiedBy = "ModifiedBy"
        };
    }
}
