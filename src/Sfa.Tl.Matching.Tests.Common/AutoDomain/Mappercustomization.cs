using System.Security.Claims;
using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Tests.Common.AutoDomain
{
    public class MapperCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, "adminUserName")
                }))
            });

            var dateTimeProvider = fixture.Create<DateTimeProvider>();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(OpportunityMapper).Assembly);
                c.AddMaps(typeof(ProviderMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserNameResolver")
                        ? (object) new LoggedInUserNameResolver<ProviderDetailViewModel, Provider>(httpContextAccessor)
                        : type.Name.Contains("UtcNowResolver")
                            ? (object) new
                                UtcNowResolver<OpportunityItemIsSelectedWithUsernameForCompleteDto, OpportunityItem>(
                                    dateTimeProvider)
                            : type.Name.Contains("UtcNowResolver")
                                ? new UtcNowResolver<ProviderDetailViewModel, Provider>(dateTimeProvider)
                                : null);
            });

            fixture.Register(() => new Mapper(mapperConfig));

            fixture.Customize<Mapper>(composer => composer.FromFactory(() => new Mapper(mapperConfig)));
        }
    }
}