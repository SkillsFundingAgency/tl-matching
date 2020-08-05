using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderQualification.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderQualification
{
    public class When_ProviderQualificationService_Is_Called_To_Get_Deleted_ProviderQualification
    {
        private const int ProviderVenueId = 1;
        private const int QualificationId = 100;

        private readonly IRepository<Domain.Models.ProviderQualification> _providerQualificationRepository;
        private readonly ProviderQualificationDto _result;

        public When_ProviderQualificationService_Is_Called_To_Get_Deleted_ProviderQualification()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(ProviderQualificationMapper).Assembly));
            var mapper = new Mapper(config);

            _providerQualificationRepository = Substitute.For<IRepository<Domain.Models.ProviderQualification>>();
            _providerQualificationRepository.CreateAsync(Arg.Any<Domain.Models.ProviderQualification>())
                .Returns(1);
            _providerQualificationRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.ProviderQualification, bool>>>())
                .Returns(new ProviderQualificationBuilder().Build(true));

            var providerQualificationService = new ProviderQualificationService(mapper, _providerQualificationRepository);

            _result = providerQualificationService.GetProviderQualificationAsync(ProviderVenueId, QualificationId).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_GetSingleOrDefaultAsync_Is_Called_Exactly_Once()
        {
            _providerQualificationRepository
                .Received(1)
                .GetSingleOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.ProviderQualification, bool>>>());
        }

        [Fact]
        public void Then_Result_Is_Null()
        {
            _result.Should().BeNull();
        }
    }
}