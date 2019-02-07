using Humanizer;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Extensions
{
    public static class ProviderExtensions
    {
        private const string Yes = "Yes";
        private const string No = "No";

        public static string[] ToStringArray(this Domain.Models.Provider provider)
        {
            var providerArray = new[]
            {
                provider.UkPrn.ToString(),
                provider.Name,
                ((OfstedRating)provider.OfstedRating).Humanize(),
                provider.Active ? Yes : No,
                provider.ActiveReason,
                provider.PrimaryContact,
                provider.PrimaryContactEmail,
                provider.PrimaryContactPhone,
                provider.SecondaryContact,
                provider.SecondaryContactEmail,
                provider.SecondaryContactPhone,
                provider.Source
            };

            return providerArray;
        }
    }
}