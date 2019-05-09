using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders
{
    internal class ValidProviderDtoBuilder
    {
        public ProviderDto Build() => new ProviderDto
        {
            Id = 1,
            UkPrn = 10000546,
            Name = "Test Provider",
            Status = true,
            PrimaryContact = "Test",
            PrimaryContactEmail = "Test@test.com",
            PrimaryContactPhone = "0123456789",
            SecondaryContact = "Test 2",
            SecondaryContactEmail = "Test2@test.com",
            SecondaryContactPhone = "0123456789",
            IsCdfProvider = true,
            IsEnabledForReferral = true,
            Source = "Test",
            CreatedBy = "CreatedBy"
        };
    }
}