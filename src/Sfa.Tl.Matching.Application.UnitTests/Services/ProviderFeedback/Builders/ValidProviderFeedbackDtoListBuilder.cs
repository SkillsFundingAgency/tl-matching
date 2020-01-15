using System.Collections.Generic;
using System.Linq;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback.Builders
{
    public class ValidProviderFeedbackDtoListBuilder
    {
        private readonly IList<ProviderFeedbackDto> _providerFeedbackDtoList;

        public ValidProviderFeedbackDtoListBuilder()
        {
            _providerFeedbackDtoList = new List<ProviderFeedbackDto>
            {
                new ProviderFeedbackDto
                {
                    ProviderId = 1,
                    ProviderName = "Provider",
                    ProviderDisplayName = "Provider display name",
                    PrimaryContact = "Provider Contact",
                    PrimaryContactEmail = "primary.contact@provider.co.uk",
                    SecondaryContact = null,
                    SecondaryContactEmail = null,
                    EmployerCompanyName = "Company",
                    Postcode = "AA1 1AA",
                    Town = "Town",
                    Routes = new List<string>
                    {
                        "Test route 1",
                        "Test route 2"
                    }
                }
            };
        }

        public ValidProviderFeedbackDtoListBuilder AddSecondaryContact()
        {
            _providerFeedbackDtoList
                .First()
                .SecondaryContact = "Provider Secondary Contact";

            _providerFeedbackDtoList
                .First()
                .SecondaryContactEmail = "secondary.contact@provider.co.uk";

            return this;
        }

        public IList<ProviderFeedbackDto> Build() => _providerFeedbackDtoList;
    }
}
