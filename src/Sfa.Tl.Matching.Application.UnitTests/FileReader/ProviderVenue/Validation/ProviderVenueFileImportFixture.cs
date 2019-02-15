using NSubstitute;
using Sfa.Tl.Matching.Application.FileReader.ProviderVenue;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using System;
using Sfa.Tl.Matching.Application.UnitTests.Provider.Builders;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Validation
{
    public class ProviderVenueFileImportFixture
    {
        public ProviderVenueDataValidator Validator;
        public ProviderVenueFileImportDto Dto;
        public IRepository<Domain.Models.Provider> ProviderRepository;
        public IRepository<Domain.Models.ProviderVenue> ProviderVenueRepository;

        public ProviderVenueFileImportFixture()
        {
            Dto = new ProviderVenueFileImportDtoBuilder().Build();
            ProviderRepository = Substitute.For<IRepository<Domain.Models.Provider>>();

            var provider = ValidProviderBuilder.Build();
            ProviderRepository.GetSingleOrDefault(
                    Arg.Any<Func<Domain.Models.Provider, bool>>())
                .Returns(provider);

            ProviderVenueRepository = Substitute.For<IRepository<Domain.Models.ProviderVenue>>();
            Validator = new ProviderVenueDataValidator(ProviderRepository,
                ProviderVenueRepository);
        }
    }
}