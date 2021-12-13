using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders
{
    internal class ValidRemoveProviderVenueViewModelBuilder
    {
        public RemoveProviderVenueViewModel Build() => new()
        {
            ProviderId = 1,
            ProviderVenueId = 1,
            Postcode = "CV1 2WT"
        };
    }
}