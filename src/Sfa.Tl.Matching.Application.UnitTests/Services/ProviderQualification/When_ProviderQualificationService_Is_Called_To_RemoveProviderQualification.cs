using System;
using System.Linq.Expressions;
using AutoMapper;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
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

            _providerQualificationRepository = Substitute.For<IRepository<Domain.Models.ProviderQualification>>();
            _providerQualificationRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderQualification, bool>>>())
                .Returns(new Domain.Models.ProviderQualification
                {
                    Id = 1,
                    ProviderVenueId = 1,
                    QualificationId = 2
                });

            var providerQualificationService = new ProviderQualificationService(mapper, _providerQualificationRepository);

            providerQualificationService.RemoveProviderQualificationAsync(1, 2).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderVenueRepository_Delete_Is_Called_Exactly_Once()
        {
            _providerQualificationRepository
                .Received(1)
                .Delete(Arg.Any<Domain.Models.ProviderQualification>());
        }

        [Fact]
        public void Then_ProviderQualificationRepository_Delete_Is_Called_With_Expected_Values()
        {
            _providerQualificationRepository
                .Received()
                .Delete(Arg.Is<Domain.Models.ProviderQualification> (
                    p => p.ProviderVenueId == 1 &&
                         p.QualificationId == 2
                        ));
        }
    }
}