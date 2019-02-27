using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders
{
    public class ValidProviderDtoListBuilder
    {
        private readonly IList<ProviderDto> _providerDtos;

        public ValidProviderDtoListBuilder(int numberOfItems)
        {
            _providerDtos = new List<ProviderDto>();

            for (var i = 0; i < numberOfItems; i++)
            {
                _providerDtos.Add(new ProviderDto
                {
                    UkPrn = 10000546,
                    Name = "ProviderName",
                    OfstedRating = OfstedRating.Good,
                    Status = true,
                    StatusReason = "StatusReason",
                    PrimaryContact = "PrimaryContact",
                    PrimaryContactEmail = "primary@contact.co.uk",
                    PrimaryContactPhone = "01777757777",
                    SecondaryContact = "SecondaryContact",
                    SecondaryContactEmail = "secondary@contact.co.uk",
                    SecondaryContactPhone = "01777757777",
                    Source = "PMF_1018",
                    CreatedBy = "Test"
                });
            }
        }

        public IEnumerable<ProviderDto> Build() =>
            _providerDtos;
    }
}
