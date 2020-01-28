using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Web.Controllers;

namespace Sfa.Tl.Matching.Web.UnitTests.Fixtures
{
    public class ProviderControllerFixture
    {
        internal IProviderService ProviderService;
        
        internal ProviderController Sut;

        public ProviderControllerFixture()
        {
            ProviderService = Substitute.For<IProviderService>();
            
            Sut = new ProviderController(ProviderService, new MatchingConfiguration());
        }
    }
}
