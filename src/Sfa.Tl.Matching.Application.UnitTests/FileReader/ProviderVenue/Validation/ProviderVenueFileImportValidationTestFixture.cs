using NSubstitute;
using Sfa.Tl.Matching.Application.FileReader.ProviderVenue;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Validation
{
    public class ProviderVenueFileImportValidationTestFixture
    {
        public ProviderVenueFileImportDto Dto;
        public IRepository<Domain.Models.ProviderVenue> ProviderVenueRepository;
        public IRepository<Domain.Models.Provider> ProviderRepository;
        public ProviderVenueDataValidator Validator;

        public ProviderVenueFileImportValidationTestFixture()
        {
            Dto = new ValidProviderVenueFileImportDtoBuilder().Build();
            
            ProviderVenueRepository = Substitute.For<IRepository<Domain.Models.ProviderVenue>>();
            
            ProviderRepository = Substitute.For<IRepository<Domain.Models.Provider>>();
            
            Validator = new ProviderVenueDataValidator(ProviderRepository, ProviderVenueRepository);
        }
    }
}