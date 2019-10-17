using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.FailedEmail.Builders
{
    internal class EmailBodyDtoBuilder
    {
        private readonly EmailBodyDto _emailBodyDto;

        public EmailBodyDtoBuilder()
        {
            _emailBodyDto = new EmailBodyDto
            {
                PrimaryContactEmail = "primary-contact@email.com",
                SecondaryContactEmail = "secondary-contact@email.com",
                ProviderDisplayName = "Provider Display Name",
                ProviderVenueName = "Provider Venue Name",
                ProviderVenuePostcode = "AB1 1AA"
            };
        }

        public EmailBodyDtoBuilder AddEmployerEmail()
        {
            _emailBodyDto.EmployerEmail = "employer@email.com";

            return this;
        }

        public EmailBodyDto Build() => _emailBodyDto;
    }
}