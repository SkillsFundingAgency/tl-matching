using NSubstitute;
using Sfa.Tl.Matching.Application.FileReader.QualificationRoutePathMapping;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class QualificationRoutePathMappingFileImportValidationTestFixture
    {
        public QualificationRoutePathMappingFileImportDto Dto;
        public QualificationRoutePathMappingDataValidator Validator;

        public IRepository<Domain.Models.QualificationRoutePathMapping> QualificationRoutePathMappingRepository;
        public IRepository<Qualification> QualificationRepository;
        public IRepository<Path> PathRepository;

        public QualificationRoutePathMappingFileImportValidationTestFixture()
        {
            Dto = new ValidQualificationRoutePathMappingFileImportDtoBuilder().Build();

            QualificationRoutePathMappingRepository = Substitute.For<IRepository<Domain.Models.QualificationRoutePathMapping>>();
            QualificationRepository = Substitute.For<IRepository<Qualification>>();

            PathRepository = Substitute.For<IRepository<Path>>();

            PathRepository.GetMany().Returns(new PathListBuilder().Build());

            Validator = new QualificationRoutePathMappingDataValidator(QualificationRoutePathMappingRepository, QualificationRepository, PathRepository);
        }

    }
}