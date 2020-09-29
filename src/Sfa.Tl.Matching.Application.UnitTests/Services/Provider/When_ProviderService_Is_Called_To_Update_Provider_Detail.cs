using System;
using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderService_Is_Called_To_Update_Provider_Detail
    {
        private readonly IRepository<Domain.Models.Provider> _repository;

        public When_ProviderService_Is_Called_To_Update_Provider_Detail()
        {
            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, "TestUser")
                }))
            });

            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            dateTimeProvider.UtcNow().Returns(new DateTime(2019, 1, 1));
            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(ProviderMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<ProviderDetailViewModel, Domain.Models.Provider>(httpContextAccessor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<ProviderDetailViewModel, Domain.Models.Provider>(httpContextAccessor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<ProviderDetailViewModel, Domain.Models.Provider>(dateTimeProvider) :
                                null);
            });
            var mapper = new Mapper(config);

            _repository = Substitute.For<IRepository<Domain.Models.Provider>>();
            _repository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.Provider, bool>>>())
                .Returns(new ValidProviderBuilder().Build());

            var referenceRepository = Substitute.For<IRepository<ProviderReference>>();

            var providerService = new ProviderService(mapper, _repository, referenceRepository);

            var viewModel = new ProviderDetailViewModel
            {
                Id = 1,
                UkPrn = 123,
                Name = "ProviderName",
                DisplayName = "display name"
            };

            providerService.UpdateProviderDetailAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _repository.Received(1)
                .GetSingleOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.Provider, bool>>>());
        }
        
        [Fact]
        public void Then_ProviderRepository_Update_Is_Called_Exactly_Once_With_Expected_Values()
        {
            _repository.Received(1)
                .UpdateAsync(Arg.Is<Domain.Models.Provider>(
                    p => p.Id == 1 &&
                         p.UkPrn == 123 &&
                         p.Name == "ProviderName" &&
                         p.DisplayName == "Display Name" &&
                         p.ModifiedBy == "TestUser" &&
                         p.ModifiedOn == new DateTime(2019, 1, 1)));
        }
    }
}