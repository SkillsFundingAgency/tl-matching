using System.Collections.Generic;
using Sfa.Tl.Matching.Application.FileReader.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader.Provider
{
    public class ProviderDataParser : IDataParser<ProviderDto>
    {
        public IEnumerable<ProviderDto> Parse(FileImportDto dto)
        {
            if (!(dto is ProviderFileImportDto data)) return null;
            
            var provider = new ProviderDto
            {
                UkPrn = data.UkPrn.ToLong(),
                Name = data.ProviderName.Trim(),
                OfstedRating = data.OfstedRating.ToOfstedRating(),
                Status = data.Status.ToBool(),
                StatusReason = data.StatusReason,
                PrimaryContact = data.PrimaryContactName,
                PrimaryContactEmail = data.PrimaryContactEmail,
                PrimaryContactPhone = data.PrimaryContactTelephone,
                SecondaryContact = data.SecondaryContactName,
                SecondaryContactEmail = data.SecondaryContactEmail,
                SecondaryContactPhone = data.SecondaryContactTelephone,
                Source = data.Source,
                CreatedBy = data.CreatedBy
            };

            return new List<ProviderDto> {provider};
        }
    }
}