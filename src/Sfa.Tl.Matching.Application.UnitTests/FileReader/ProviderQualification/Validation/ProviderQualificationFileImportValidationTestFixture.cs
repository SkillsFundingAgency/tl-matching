using NSubstitute;
using Sfa.Tl.Matching.Application.FileReader.ProviderQualification;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderQualification.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderQualification.Validation
{
    public class ProviderQualificationFileImportValidationTestFixture
    {
        public ProviderQualificationFileImportDto Dto;
        public IRepository<Domain.Models.ProviderQualification> ProviderQualificationRepository;
        public IRepository<Domain.Models.ProviderVenue> ProviderVenueRepository;
        public IRepository<Qualification> QualificationRepository;
        public ProviderQualificationDataValidator Validator;

        public ProviderQualificationFileImportValidationTestFixture()
        {
            Dto = new ValidProviderQualificationFileImportDtoBuilder().Build();
            
            ProviderQualificationRepository = Substitute.For<IRepository<Domain.Models.ProviderQualification>>();
            
            ProviderVenueRepository = Substitute.For<IRepository<Domain.Models.ProviderVenue>>();
            
            QualificationRepository = Substitute.For<IRepository<Qualification>>();
            
            Validator = new ProviderQualificationDataValidator(ProviderVenueRepository, ProviderQualificationRepository, QualificationRepository);
        }
    }
}