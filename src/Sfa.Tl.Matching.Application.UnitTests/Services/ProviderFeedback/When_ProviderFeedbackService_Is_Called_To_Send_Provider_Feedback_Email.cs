using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback
{
    public class When_ProviderFeedbackService_Is_Called_To_Send_Provider_Feedback_Email
    {
        [Theory, AutoDomainData]
        public async Task Test(
            MatchingConfiguration configuration,
            ILogger<ProviderFeedbackService> logger,
            ILogger<GenericRepository<BankHoliday>> bankHolidayLogger,
            ILogger<OpportunityRepository> opportunityLogger,
            ILogger<GenericRepository<OpportunityItem>> opportunityItemLogger,
            IDateTimeProvider dateTimeProvider,
            IEmailService emailService,
            IEmailHistoryService emailHistoryService,
            MatchingDbContext dbContext,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue
        )
        {
            //Arrange
            var bankHolidayRepo = new GenericRepository<BankHoliday>(bankHolidayLogger, dbContext);
            var opportunityRepo = new OpportunityRepository(opportunityLogger, dbContext);
            var opportunityItemRepo = new GenericRepository<OpportunityItem>(opportunityItemLogger, dbContext);

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(OpportunityMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("UtcNowResolver")
                        ? new UtcNowResolver<OpportunityItemWithUsernameForProviderFeedbackSentDto, OpportunityItem>(
                            dateTimeProvider)
                        : null);
            });
            var mapper = new Mapper(config);

            dateTimeProvider
                .AddWorkingDays(Arg.Any<DateTime>(), Arg.Any<TimeSpan>(), Arg.Any<IList<DateTime>>())
                .Returns(DateTime.Parse("2019-09-19 23:59:59"));

            dateTimeProvider
                .IsHoliday(Arg.Any<DateTime>(), Arg.Any<IList<DateTime>>())
                .Returns(false);

            await ProviderFeedbackInMemoryTestData.SetTestData(dbContext, provider, venue, opportunity);

            var sut = new ProviderFeedbackService(mapper, configuration, logger, dateTimeProvider, emailService,
                emailHistoryService, bankHolidayRepo, opportunityRepo, opportunityItemRepo);

            //Act
            var emailsCount = await sut.SendProviderFeedbackEmailsAsync("test system");

            //Assert
            emailsCount.Should().Be(1);
        }
    }
}
