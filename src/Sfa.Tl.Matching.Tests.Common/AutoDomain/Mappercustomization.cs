using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Tests.Common.AutoDomain
{
    public class Mappercustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var httpContextAccessor = new HttpContextAccessor();
            var dateTimeProvider = fixture.Create<DateTimeProvider>();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(OpportunityMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver")
                        ? new LoggedInUserNameResolver<OpportunityItemIsSelectedForReferralDto, OpportunityItem>(
                            httpContextAccessor)
                        : type.Name.Contains("LoggedInUserEmailResolver")
                            ? new LoggedInUserNameResolver<ProvisionGapDto, ProvisionGap>(
                                httpContextAccessor)
                            : type.Name.Contains("LoggedInUserEmailResolver")
                                ? new LoggedInUserNameResolver<ReferralDto, Referral>(
                                    httpContextAccessor)
                                : type.Name.Contains("LoggedInUserEmailResolver")
                                    ? new LoggedInUserNameResolver<OpportunityItemDto, OpportunityItem>(
                                        httpContextAccessor)
                                    : type.Name.Contains("LoggedInUserEmailResolver")
                                        ? new LoggedInUserNameResolver<OpportunityDto, Opportunity>(
                                            httpContextAccessor)
                                        : type.Name.Contains("LoggedInUserEmailResolver")
                                            ? new LoggedInUserEmailResolver<OpportunityItemIsSelectedForCompleteDto,
                                                OpportunityItem>(
                                                httpContextAccessor)
                                            : type.Name.Contains("LoggedInUserNameResolver")
                                                ? (object) new
                                                    LoggedInUserNameResolver<OpportunityItemIsSelectedForCompleteDto,
                                                        OpportunityItem>(
                                                        httpContextAccessor)
                                                : type.Name.Contains("UtcNowResolver")
                                                    ? (object) new
                                                        UtcNowResolver<
                                                            OpportunityItemIsSelectedWithUsernameForCompleteDto,
                                                            OpportunityItem>(
                                                            dateTimeProvider)
                                                    : type.Name.Contains("UtcNowResolver")
                                                        ? (object) new
                                                            UtcNowResolver<OpportunityItemIsSelectedForCompleteDto,
                                                                OpportunityItem>(
                                                                dateTimeProvider)
                                                        : type.Name.Contains("UtcNowResolver")
                                                            ? (object) new
                                                                UtcNowResolver<OpportunityItemIsSelectedForReferralDto,
                                                                    OpportunityItem>(
                                                                    dateTimeProvider)
                                                            : type.Name.Contains("UtcNowResolver")
                                                                ? new UtcNowResolver<
                                                                    OpportunityItemIsSelectedWithUsernameForCompleteDto
                                                                    , OpportunityItem>(
                                                                    dateTimeProvider)
                                                                : null);
            });

            fixture.Register(() => new Mapper(mapperConfig));

            fixture.Customize<Mapper>(composer => composer.FromFactory(() => new Mapper(mapperConfig)));

        }
    }
}