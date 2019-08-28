using System;
using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderQualification
{
    public class When_ProviderQualificationService_Is_Called_To_CreateProviderQualification
    {
        private readonly IRepository<Domain.Models.ProviderQualification> _providerQualificationRepository;
        private readonly int _result;

        public When_ProviderQualificationService_Is_Called_To_CreateProviderQualification()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();
            httpcontextAccesor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, "createdByUserName"),
                    new Claim(ClaimTypes.Upn, "email@user.com")
                }))
            });

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(ProviderQualificationMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<AddQualificationViewModel, Domain.Models.ProviderQualification>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<AddQualificationViewModel, Domain.Models.ProviderQualification>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<AddQualificationViewModel, Domain.Models.ProviderQualification>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);

            _providerQualificationRepository = Substitute.For<IRepository<Domain.Models.ProviderQualification>>();
            _providerQualificationRepository.Create(Arg.Any<Domain.Models.ProviderQualification>())
                .Returns(1);
            _providerQualificationRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderQualification, bool>>>())
                .Returns((Domain.Models.ProviderQualification)null);

            var providerQualificationService = new ProviderQualificationService(mapper, _providerQualificationRepository);

            var viewModel = new AddQualificationViewModel
            {
                ProviderVenueId = 1,
                QualificationId = 2,
                Postcode = "CV1 2WT",
                LarId = "10042982"
            };

            _result = providerQualificationService.CreateProviderQualificationAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderVenueRepository_Create_Is_Called_Exactly_Once()
        {
            _providerQualificationRepository
                .Received(1)
                .Create(Arg.Any<Domain.Models.ProviderQualification>());
        }

        [Fact]
        public void Then_ProviderQualificationRepository_Create_Is_Called_With_Expected_Values()
        {
            _providerQualificationRepository
                .Received()
                .Create(Arg.Is<Domain.Models.ProviderQualification>(
                    p => p.ProviderVenueId == 1 &&
                         p.QualificationId == 2
                        ));
        }

        [Fact]
        public void Then_Created_ProviderQualification_Id_Should_Be_Greater_Than_Zero()
        {
            _result.Should().BeGreaterThan(0);
        }
    }
}