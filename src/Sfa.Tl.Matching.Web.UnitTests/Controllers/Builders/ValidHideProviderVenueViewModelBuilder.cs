using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders
{
    internal class ValidHideProviderVenueViewModelBuilder
    {
        public HideProviderVenueViewModel Build() => new HideProviderVenueViewModel
        {
            ProviderVenueId = 1,
            Postcode = "CV1 2WT",
            ProviderVenueName = "Test Provider",
            IsRemoved = true
        };
    }
}