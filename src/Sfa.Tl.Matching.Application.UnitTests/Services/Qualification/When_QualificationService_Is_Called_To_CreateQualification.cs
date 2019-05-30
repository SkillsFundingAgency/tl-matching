using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Qualification
{
    public class When_QualificationService_Is_Called_To_CreateQualification
    {
        private readonly IRepository<Domain.Models.Qualification> _qualificationRepository;
        private readonly  int _result;

        public When_QualificationService_Is_Called_To_CreateQualification()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddProfiles(typeof(QualificationMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<MissingQualificationViewModel, Domain.Models.Qualification>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<MissingQualificationViewModel, Domain.Models.Qualification>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<MissingQualificationViewModel, Domain.Models.Qualification>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);

            var learningAimsReferenceRepository = Substitute.For<IRepository<LearningAimsReference>>();

            _qualificationRepository = Substitute.For<IRepository<Domain.Models.Qualification>>();
            _qualificationRepository.Create(Arg.Any<Domain.Models.Qualification>())
                .Returns(1);
            _qualificationRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Qualification, bool>>>())
                .Returns(new Domain.Models.Qualification());

            var qualificationRoutePathMappingRepository = Substitute.For<IRepository<Domain.Models.QualificationRoutePathMapping>>();

            var qualificationService = new QualificationService(mapper, _qualificationRepository, qualificationRoutePathMappingRepository, learningAimsReferenceRepository);

            var viewModel = new MissingQualificationViewModel
            {
                ProviderVenueId = 1,
                LarId = "10042982",
                Title = "Title",
                ShortTitle = "Short Title",
                Source = "Test"
            };

            _result = qualificationService.CreateQualificationAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderVenueRepository_Create_Is_Called_Exactly_Once()
        {
            _qualificationRepository
                .Received(1)
                .Create(Arg.Any<Domain.Models.Qualification>());
        }

        [Fact]
        public void Then_QualificationRepository_Create_Is_Called_With_Expected_Values()
        {
            _qualificationRepository
                .Received()
                .Create(Arg.Is<Domain.Models.Qualification> (
                    p => p.LarsId == "10042982"
                ));
        }

        [Fact]
        public void Then_Created_Qualification_Id_Should_Be_Greater_Than_Zero()
        {
            _result.Should().BeGreaterThan(0);
        }
    }
}