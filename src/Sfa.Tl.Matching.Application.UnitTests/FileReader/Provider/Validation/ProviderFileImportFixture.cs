using NSubstitute;
using Sfa.Tl.Matching.Application.FileReader.Provider;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Validation
{
    public class ProviderFileImportFixture
    {
        public ProviderDataValidator ProviderDataValidator;
        public ProviderFileImportDto ProviderFileImportDto;
        public IRepository<Domain.Models.Provider> Repository;

        public ProviderFileImportFixture()
        {
            ProviderFileImportDto = new ValidProviderFileImportDtoBuilder().Build();
            Repository = Substitute.For<IRepository<Domain.Models.Provider>>();
            ProviderDataValidator = new ProviderDataValidator(Repository);
        }
    }
}