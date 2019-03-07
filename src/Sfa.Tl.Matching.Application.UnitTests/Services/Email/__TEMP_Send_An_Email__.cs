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

        public const string APPLY_SIGNUP_ERROR = "ApplySignupError";
        public const string CANDIDATE_CONTACT_US = "VacancyService_CandidateContactUsMessage";
        public const string TEST_TEMPLATE = "Test_Template";
        public const string PROVIDER_REFERRAL = "ProviderReferral";
        

        public __TEMP_Send_An_Email__()
        {
            _configuration = ConfigurationLoader.Load(
                "LOCAL",
                "UseDevelopmentStorage=true;",
                "1.0",
                "Sfa.Tl.Matching");

            _notificationsApi = RegisterNotificationApi();
            var logger = Substitute.For<ILogger<EmailService>>();
            var loggerForRepository = Substitute.For<ILogger<GenericRepository<Domain.Models.EmailTemplate>>>();
            //var loggerForRepository = new Logger<GenericRepository<Domain.Models.EmailTemplate>>(new NullLoggerFactory());

            using (var dbContext = GetDbContext())
            {
                _emailTemplateRepository = new GenericRepository<EmailTemplate>(loggerForRepository, dbContext);

                var emailService = new EmailService(_notificationsApi, _emailTemplateRepository, logger);

                var subject = "A test email";
                //var templateName = APPLY_SIGNUP_ERROR;
                //var templateName = CANDIDATE_CONTACT_US;
                //var templateName = TEST_TEMPLATE;
                var templateName = PROVIDER_REFERRAL;

                var toAddress = "";
                var replyToAddress = "reply@test.com";

                dynamic tokens;
                switch (templateName)
                {
                    case APPLY_SIGNUP_ERROR:
                        tokens = new { first_name = "Tester" };
                        break;
                    case CANDIDATE_CONTACT_US:
                        tokens = (dynamic) new
                        {
                            UserEmailAddress = replyToAddress,
                            UserFullName = "Test <strong>User</strong>",
                            UserEnquiry = "I have a question",
                            UserEnquiryDetails = "<ul><li>item</li><li>item</li></ul>"
                        };
                        break;
                    case TEST_TEMPLATE:
                        tokens = (dynamic)new
                        {
                            first_name = "Mike",
                            under18 = "yes"
                        };
                        break;
                    case PROVIDER_REFERRAL:
                        tokens = null;
                        break;
                    default:
                        throw new Exception("Test template {templateName} was not recognized");
                }

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
    }
}
