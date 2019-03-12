using System;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SFA.DAS.Http;
using SFA.DAS.Http.TokenGenerators;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Services;
using SFA.DAS.Notifications.Api.Client;
using Xunit;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data;
using Microsoft.EntityFrameworkCore;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Email
{
    public class __TEMP_Send_An_Email__
    {
        private readonly MatchingConfiguration _configuration;
        IRepository<EmailTemplate> _emailTemplateRepository;
        private readonly INotificationsApi _notificationsApi;

        public const string CANDIDATE_CONTACT_US = "VacancyService_CandidateContactUsMessage";
        public const string TEST_TEMPLATE = "Test_Template";
        public const string EMPLOYER_REFERRAL = "EmployerReferral";
        public const string PROVIDER_REFERRAL = "ProviderReferral";
        public const string PROVISION_GAP = "ProvisionGapReport";

        public __TEMP_Send_An_Email__()
        {
            try
            {
                _configuration = ConfigurationLoader.Load(
                    "LOCAL",
                    "UseDevelopmentStorage=true;",
                    "1.0",
                    "Sfa.Tl.Matching");
            }
            catch
            {
                //This will fail on a non-local environment
                return;
            }

            if (_configuration == null)
            {
                return;
            }

            _notificationsApi = RegisterNotificationApi();

            var logger = Substitute.For<ILogger<EmailService>>();
            var loggerForRepository = Substitute.For<ILogger<GenericRepository<Domain.Models.EmailTemplate>>>();
            //var loggerForRepository = new Logger<GenericRepository<Domain.Models.EmailTemplate>>(new NullLoggerFactory());

            using (var dbContext = GetDbContext())
            {
                _emailTemplateRepository = new GenericRepository<EmailTemplate>(loggerForRepository, dbContext);

                var subject = "A test email";
                //var templateName = CANDIDATE_CONTACT_US;
                //var templateName = TEST_TEMPLATE;
                //var templateName = EMPLOYER_REFERRAL;
                var templateName = PROVIDER_REFERRAL;
                //var templateName = PROVISION_GAP;

                var toAddress = "";
                var replyToAddress = "reply@test.com";

                dynamic tokens;
                switch (templateName)
                {
                    case CANDIDATE_CONTACT_US:
                        tokens = (dynamic)new
                        {
                            UserEmailAddress = replyToAddress,
                            UserFullName = "Test <strong>User</strong>",
                            UserEnquiry = "I have a question",
                            UserEnquiryDetails = "And here is the question"
                        };
                        break;
                    case TEST_TEMPLATE:
                        tokens = (dynamic)new
                        {
                            first_name = "Mike",
                            under18 = "yes"
                        };
                        break;
                    case EMPLOYER_REFERRAL:
                        tokens = (dynamic)new
                        {
                            employer_business_name = "Big Co.",
                            employer_contact_name = "Bog Boss",
                            employer_contact_number = "0201 234 567",
                            employer_contact_email = "test@test.com",
                            employer_postcode = "XX1 2YY",
                            route = "Engineering and manufacturing",
                            job_role = "Welder"
                        };
                        break;
                    case PROVIDER_REFERRAL:
                        tokens = (dynamic)new
                        {
                            primary_contact_name = "Mike",
                            provider_name = "Your Provider",
                            route = "Catering and hospitality",
                            venue_postcode = "AA1 1AA",
                            distance = "3.6",
                            job_role = "Assistant Chef",
                            employer_business_name = "Big Co.",
                            employer_contact_name = "Bog Boss",
                            employer_contact_number = "0201 234 567",
                            employer_contact_email = "test@test.com",
                            employer_postcode = "XX1 2YY",
                            number_of_placements = "at least 1"
                        };
                        break;
                    case PROVISION_GAP:
                        tokens = null;
                        break;
                    default:
                        throw new Exception("Test template {templateName} was not recognized");
                }

                var emailService = new EmailService(_notificationsApi, _emailTemplateRepository, logger);

                emailService
                    .SendEmail(templateName, toAddress, subject, tokens, replyToAddress)
                    .GetAwaiter().GetResult();
            }
        }

        public MatchingDbContext GetDbContext()
        {
            var dbOptions = new DbContextOptionsBuilder<MatchingDbContext>()
                .UseSqlServer(_configuration.SqlConnectionString, builder => builder.EnableRetryOnFailure())
                .Options;

            var matchingDbContext = new MatchingDbContext(dbOptions);
            return matchingDbContext;
        }

        private INotificationsApi RegisterNotificationApi()
        {
            var apiConfiguration = _configuration.NotificationsApiClientConfiguration;

            var httpClient = string.IsNullOrWhiteSpace(apiConfiguration.ClientId)
                ? new HttpClientBuilder().WithBearerAuthorisationHeader(new JwtBearerTokenGenerator(apiConfiguration)).Build()
                : new HttpClientBuilder().WithBearerAuthorisationHeader(new AzureADBearerTokenGenerator(apiConfiguration)).Build();

            return new NotificationsApi(httpClient, apiConfiguration);

            //For startup can do something like
            //services.AddTransient<INotificationsApi, NotificationsApi>(provider =>
            //    new NotificationsApi(httpClient, apiConfiguration));
        }

        //[Fact]
        //public void _Test_Runs_But_We_Dont_Want_It()
        //    => throw new Xunit.Sdk.XunitException("This test is for temporary development testing only");
        //=> true.Equals(true);
    }
}
