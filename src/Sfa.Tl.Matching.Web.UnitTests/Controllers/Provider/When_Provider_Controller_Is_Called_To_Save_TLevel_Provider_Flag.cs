using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_Is_Called_To_Save_TLevel_Provider_Flag
    {
        [Theory, AutoDomainData]
        public async Task Then_Make_TLevel_Provider_Flag_To_True(
            MatchingDbContext dbContext,
            Domain.Models.Provider provider,
            List<ProviderVenueViewModel> venueViewModels,
            MatchingConfiguration config,
            ILogger<GenericRepository<Domain.Models.Provider>> providerLogger,
            ILogger<GenericRepository<ProviderReference>> providerReferenceLogger,
            IHttpContextAccessor httpContextAccessor,
            IDateTimeProvider dateTimeProvider
            )
        {
            //Arrange
            provider.IsTLevelProvider = false;

            await dbContext.AddAsync(provider);
            await dbContext.SaveChangesAsync();

            dbContext.Entry(provider).State = EntityState.Detached;

            var viewModel = new ProviderDetailViewModel
            {
                Id = provider.Id,
                IsTLevelProvider = true,
                ProviderVenues = venueViewModels
            };

            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, "System")
                }))
            });

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(ProviderMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserNameResolver")
                        ? (object)new LoggedInUserNameResolver<ProviderDetailViewModel, Domain.Models.Provider>(httpContextAccessor)
                        : type.Name.Contains("UtcNowResolver")
                                ? new UtcNowResolver<ProviderDetailViewModel, Domain.Models.Provider>(dateTimeProvider)
                                : null);
            });

            var mapper = new Mapper(mapperConfig);

            var providerReferenceRepo = new GenericRepository<ProviderReference>(providerReferenceLogger, dbContext);
            var repo = new GenericRepository<Domain.Models.Provider>(providerLogger, dbContext);
            var providerService = new ProviderService(mapper, repo, providerReferenceRepo);

            var sut = new ProviderController(providerService, config);

            //Act
            await sut.SaveProviderDetail(viewModel);

            //Assert
            var expectedResult = await repo.GetSingleOrDefaultAsync(x => x.Id == provider.Id);

            provider.IsTLevelProvider.Should().BeFalse();
            expectedResult.IsTLevelProvider.Should().BeTrue();
        }
    }
}
