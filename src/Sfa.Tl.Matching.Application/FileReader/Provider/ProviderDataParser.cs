using System.Collections.Generic;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader.Provider
{
    public class ProviderDataParser : IDataParser<ProviderDto>
    {
        public IEnumerable<ProviderDto> Parse(FileImportDto fileImportDto)
        {
            if (!(fileImportDto is ProviderFileImportDto data)) return null;

            var providerDto = new ProviderDto
            {
                UkPrn = data.UkPrn.ToLong(),
                Name = data.ProviderName.Trim(),
                OfstedRating = data.OfstedRating.ToOfstedRating(),
                IsCdfProvider = data.Status.ToBool(),
                IsEnabledForReferral = data.Status.ToBool(),
                PrimaryContact = data.PrimaryContactName,
                PrimaryContactEmail = data.PrimaryContactEmail,
                PrimaryContactPhone = data.PrimaryContactTelephone,
                SecondaryContact = data.SecondaryContactName,
                SecondaryContactEmail = data.SecondaryContactEmail,
                SecondaryContactPhone = data.SecondaryContactTelephone,
                Source = data.Source,
                CreatedBy = data.CreatedBy
            };

            return new List<ProviderDto> { providerDto };
        }
    }
}