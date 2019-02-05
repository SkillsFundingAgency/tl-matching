using Sfa.Tl.Matching.Application.FileReader.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.Provider
{
    public class ProviderDataParser : IDataParser<ProviderDto>
    {
        public ProviderDto Parse(string[] cells)
        {
            var provider = new ProviderDto
            {
                UkPrn = cells[(int)ProviderColumnIndex.UkPrn].ToLong(),
                Name = cells[(int)ProviderColumnIndex.Name],
                OfstedRating = cells[(int)ProviderColumnIndex.OfstedRating].ToOfstedRating(),
                Active = cells[(int)ProviderColumnIndex.Active].ToBool(),
                ActiveReason = cells[(int)ProviderColumnIndex.ActiveReason],
                PrimaryContact = cells[(int)ProviderColumnIndex.PrimaryContact],
                PrimaryContactEmail = cells[(int)ProviderColumnIndex.PrimaryContactEmail],
                PrimaryContactPhone = cells[(int)ProviderColumnIndex.PrimaryContactPhone],
                SecondaryContact = cells[(int)ProviderColumnIndex.SecondaryContact],
                SecondaryContactEmail = cells[(int)ProviderColumnIndex.SecondaryContactEmail],
                SecondaryContactPhone = cells[(int)ProviderColumnIndex.SecondaryContactPhone],
                Source = cells[(int)ProviderColumnIndex.Source].ToSource()
            };

            return provider;
        }
    }
}