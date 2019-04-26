using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback.Builders
{
    public class ValidProviderWithFundingDtoListBuilder
    {
        public IList<ProviderWithFundingDto> Build() => new List<ProviderWithFundingDto>
        {
            new ProviderWithFundingDto
            {
                Id = 1,
                Name = "Provider Name",
                PrimaryContact = "Provider Contact",
                PrimaryContactEmail = "primary.contact@provider.co.uk",
                PrimaryContactPhone = "01777757777",
                SecondaryContact = "SecondaryContact",
                SecondaryContactEmail = "secondary@contact.co.uk",
                SecondaryContactPhone = "01234559999",
            }
        };
    }
}
