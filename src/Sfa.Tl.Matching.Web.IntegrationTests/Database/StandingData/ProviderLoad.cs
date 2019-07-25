using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Database.StandingData
{
    internal class ProviderLoad
    {
        public static Provider[] Create()
        {
            var providers = new[]
            {
                new Provider
                {
                    Id = 1
                }
            };

            return providers;
        }
    }
}