using AutoFixture.Xunit2;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces.ServiceFactory;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.ServiceFactory.FeedbaclEmail
{
    public class When_Feedback_Email_Factory_Is_Called_To_Create_Feedback_Service
    {

        [Theory, AutoDomainData]
        public void Then_Create_Provider_Feedback_Service(
            [Frozen] IFeedbackFactory<ProviderFeedbackService> factory,
            ProviderFeedbackService service)
        {
            factory.Create.Returns(service);

            var instance = factory.Create;

            instance.Should().BeOfType<ProviderFeedbackService>();
            instance.Should().NotBeOfType<EmployerFeedbackService>();
        }

        [Theory, AutoDomainData]
        public void Then_Create_Employer_Feedback_Service(
            [Frozen] IFeedbackFactory<EmployerFeedbackService> factory,
            EmployerFeedbackService service)
        {
            factory.Create.Returns(service);

            var instance = factory.Create;

            instance.Should().BeOfType<EmployerFeedbackService>();
            instance.Should().NotBeOfType<ProviderFeedbackService>();
        }

    }
}
