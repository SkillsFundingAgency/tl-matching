using System;
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
        public IRepository<RoutePathMapping> Repository;
        public QualificationRoutePathMappingDataValidator Validator;
        public IRepository<Path> PathRepository;

        public QualificationRoutePathMappingFileImportValidationTestFixture()
        {
            Dto = new ValidQualificationRoutePathMappingFileImportDtoBuilder().Build();

            Repository = Substitute.For<IRepository<RoutePathMapping>>();
            PathRepository = Substitute.For<IRepository<Path>>();
            PathRepository
                .GetMany(Arg.Any<Func<Path, bool>>())
                .Returns(new PathListBuilder().Build());

            Validator = new QualificationRoutePathMappingDataValidator(Repository, PathRepository);
        }
    }
}