using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IRepository<QualificationRoutePathMapping>  _qualificationRoutePathMappingRepository;
        private readonly  int _result;

        public When_QualificationService_Is_Called_To_CreateQualification()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(QualificationMapper).Assembly);
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

            var learningAimReferenceRepository = Substitute.For<IRepository<LearningAimReference>>();

            var qualificationRepository = Substitute.For<IRepository<Domain.Models.Qualification>>();
            qualificationRepository
                .Create(Arg.Do<Domain.Models.Qualification>(
                q => q.Id = 1))
                .Returns(1);
            qualificationRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Qualification, bool>>>())
                .Returns(new Domain.Models.Qualification());
            
            _qualificationRoutePathMappingRepository = Substitute.For<IRepository<QualificationRoutePathMapping>>();
            _qualificationRoutePathMappingRepository
                .CreateMany(Arg.Do<IList<QualificationRoutePathMapping>>(
                        qrpm => qrpm.First().Qualification.Id = 1))
                .Returns(1);

            var qualificationService = new QualificationService(mapper, qualificationRepository, _qualificationRoutePathMappingRepository, learningAimReferenceRepository);

            var viewModel = new MissingQualificationViewModel
            {
                ProviderVenueId = 1,
                LarId = "10042982",
                Title = "Title",
                ShortTitle = "Short Title",
                Source = "Test",
                Routes = new List<RouteViewModel>
                {
                    new RouteViewModel
                    {
                        Id = 1,
                        Name = "Route 1",
                        IsSelected = true,
                        PathNames = new List<string>
                        {
                            "Path 1"
                        }
                    }
                }
            };

            _result = qualificationService.CreateQualificationAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_QualificationRoutePathMappingRepository_CreateMany_Is_Called_Exactly_Once()
        {
            _qualificationRoutePathMappingRepository
                .Received(1)
                .CreateMany(Arg.Any<IList<QualificationRoutePathMapping>>());
        }

        [Fact]
        public void Then_QualificationRoutePathMappingRepository_CreateMany_Is_Called_With_Expected_Values()
        {
            _qualificationRoutePathMappingRepository
                .Received(1)
                .CreateMany(Arg.Is<IList<QualificationRoutePathMapping>>(
                    qrpm => qrpm.Count == 1 && 
                            qrpm.First().Qualification.LarsId == "10042982" &&
                            qrpm.First().Qualification.ShortTitle == "Short Title" &&
                            qrpm.First().RouteId == 1
                ));
        }

        [Fact]
        public void Then_Created_Qualification_Id_Should_Be_Greater_Than_Zero()
        {
            _result.Should().BeGreaterThan(0);
        }
    }
}