using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.EmailDeliveryStatusService.Builders
{
    internal class EmailBodyDtoBuilder
    {
        private readonly EmailBodyDto _emailBodyDto;

        public EmailBodyDtoBuilder()
        {
            _emailBodyDto = new EmailBodyDto();
        }

        public EmailBodyDtoBuilder AddProviderEmail()
        {
            _emailBodyDto.PrimaryContactEmail = "primary-contact@email.com";
            _emailBodyDto.SecondaryContactEmail = "secondary-contact@email.com";
            _emailBodyDto.ProviderDisplayName = "Provider Display Name";
            _emailBodyDto.ProviderVenueName = "Provider Venue Name";
            _emailBodyDto.ProviderVenuePostcode = "AB1 1AA";

            return this;
        }

        public EmailBodyDtoBuilder AddEmployerEmail()
        {
            _emailBodyDto.EmployerEmail = "employer@email.com";

            return this;
        }

        public EmailBodyDto Build() => _emailBodyDto;
    }
}