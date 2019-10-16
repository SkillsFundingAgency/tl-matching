using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.FailedEmail.Builders
{
    internal class FailedEmailBodyDtoBuilder
    {
        private readonly FailedEmailBodyDto _failedEmailBodyDto;

        public FailedEmailBodyDtoBuilder()
        {
            _failedEmailBodyDto = new FailedEmailBodyDto
            {
                PrimaryContactEmail = "primary-contact@email.com",
                SecondaryContactEmail = "secondary-contact@email.com",
                ProviderDisplayName = "Provider Display Name",
                ProviderVenueName = "Provider Venue Name",
                ProviderVenuePostcode = "AB1 1AA"
            };
        }

        public FailedEmailBodyDto Build() => _failedEmailBodyDto;
    }
}