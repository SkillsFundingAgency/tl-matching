using Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Constants;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Builders
{
    internal class ValidProviderFileImportDtoBuilder
    {
        private readonly ProviderFileImportDto _providerFileImportDto;

        public ValidProviderFileImportDtoBuilder()
        {
            _providerFileImportDto = new ProviderFileImportDto
            {
                UkPrn = ProviderConstants.UkPrn.ToString(),
                ProviderName = ProviderConstants.Name,
                OfstedRating = ProviderConstants.OfstedRating,
                Status = ProviderConstants.Status,
                StatusReason = ProviderConstants.StatusReason,
                PrimaryContactName = ProviderConstants.PrimaryContactName,
                PrimaryContactEmail = ProviderConstants.PrimaryContactEmail,
                PrimaryContactTelephone = ProviderConstants.PrimaryContactTelephone,
                SecondaryContactName = ProviderConstants.SecondaryContactName,
                SecondaryContactEmail = ProviderConstants.SecondaryContactEmail,
                SecondaryContactTelephone = ProviderConstants.SecondaryContactTelephone,
                Source = ProviderConstants.Source,
                CreatedBy = ProviderConstants.CreatedBy
            };
        }

        public ProviderFileImportDto Build() => 
            _providerFileImportDto;
    }
}