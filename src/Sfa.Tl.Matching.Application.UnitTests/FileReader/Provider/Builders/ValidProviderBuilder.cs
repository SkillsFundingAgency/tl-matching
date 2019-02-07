using Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Constants;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Builders
{
    internal class ValidProviderBuilder
    {
        private readonly Domain.Models.Provider _provider;

        public ValidProviderBuilder()
        {
            _provider = new Domain.Models.Provider
            {
                UkPrn = ProviderConstants.UkPrn,
                Name = ProviderConstants.Name,
                OfstedRating = (int)ProviderConstants.OfstedRating,
                Active = ProviderConstants.Active,
                ActiveReason = ProviderConstants.ActiveReason,
                PrimaryContact = ProviderConstants.PrimaryContact,
                PrimaryContactEmail = ProviderConstants.PrimaryContactEmail,
                PrimaryContactPhone = ProviderConstants.PrimaryContactPhone,
                SecondaryContact = ProviderConstants.SecondaryContact,
                SecondaryContactEmail = ProviderConstants.SecondaryContactEmail,
                SecondaryContactPhone = ProviderConstants.SecondaryContactPhone,
                Source = (int)ProviderConstants.Source
            };
        }

        public Domain.Models.Provider Build() =>
            _provider;
    }
}