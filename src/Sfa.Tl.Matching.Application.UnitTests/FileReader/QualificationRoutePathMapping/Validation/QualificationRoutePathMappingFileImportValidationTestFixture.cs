﻿using NSubstitute;
using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
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

        public QualificationRoutePathMappingFileImportValidationTestFixture()
        {
            Dto = new ValidQualificationRoutePathMappingFileImportDtoBuilder().Build();

            Repository = Substitute.For<IRepository<RoutePathMapping>>();

            Validator = new QualificationRoutePathMappingDataValidator(Repository);
        }
    }
}