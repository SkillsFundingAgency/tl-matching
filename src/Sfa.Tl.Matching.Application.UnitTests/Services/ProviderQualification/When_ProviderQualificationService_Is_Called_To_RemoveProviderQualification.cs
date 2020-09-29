using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderQualification.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderQualification
{
    public class When_ProviderQualificationService_Is_Called_To_RemoveProviderQualification
    {
        private readonly IRepository<Domain.Models.ProviderQualification> _providerQualificationRepository;

        public When_ProviderQualificationService_Is_Called_To_RemoveProviderQualification()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(QualificationMapper).Assembly));
            var mapper = new Mapper(config);

            var mockDbSet = new ProviderQualificationListBuilder()
                .Build()
                .AsQueryable()
                .BuildMockDbSet();

            _providerQualificationRepository = Substitute.For<IRepository<Domain.Models.ProviderQualification>>();
            _providerQualificationRepository.GetManyAsync(Arg.Any<Expression<Func<Domain.Models.ProviderQualification, bool>>>()).Returns(mockDbSet);

            var providerQualificationService = new ProviderQualificationService(mapper, _providerQualificationRepository);

            providerQualificationService.RemoveProviderQualificationAsync(1, 2).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderQualificationRepository_DeleteMany_Is_Called_Exactly_Once_With_Expected_Values()
        {
            _providerQualificationRepository
                .Received()
                .DeleteManyAsync(Arg.Is<IList<Domain.Models.ProviderQualification>>(
                    p => p.Count == 1 && 
                         p.First().ProviderVenueId == 1 &&
                         p.First().QualificationId == 2));
        }
    }
}