using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Builders
{
    internal class ValidProviderBuilder
    {
        private readonly ProviderFileImportDto _provider;

        public ValidProviderBuilder()
        {
            _provider = new ProviderFileImportDto
            {
                UkPrn = "10000546",
                ProviderName = "ProviderName",
                OfstedRating = Models.Enums.OfstedRating.Good.ToString(),
                Status = "Yes",
                StatusReason = "StatusReason",
                PrimaryContactName = "PrimaryContact",
                PrimaryContactEmail = "primary@contact.co.uk",
                PrimaryContactTelephone = "01777757777",
                SecondaryContactName = "SecondaryContact",
                SecondaryContactEmail = "secondary@contact.co.uk",
                SecondaryContactTelephone = "01777757777",
                Source = "PMF_1018"
            };
        }

        public ProviderFileImportDto Build() => _provider;
    }
}