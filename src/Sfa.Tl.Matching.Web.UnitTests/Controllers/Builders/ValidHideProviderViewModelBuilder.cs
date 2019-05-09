using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders
{
    internal class ValidHideProviderViewModelBuilder
    {
        public HideProviderViewModel Build() => new HideProviderViewModel
        {
            ProviderId = 1,
            UkPrn = 10000546,
            ProviderName = "Test Provider",
            IsCdfProvider = true
        };
    }
}