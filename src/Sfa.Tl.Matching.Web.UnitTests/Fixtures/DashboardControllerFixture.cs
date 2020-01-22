using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;

namespace Sfa.Tl.Matching.Web.UnitTests.Fixtures
{
    public class DashboardControllerFixture
    {

        internal readonly IEmployerService EmployerService;
        internal readonly IServiceStatusHistoryService ServiceStatusHistoryService;
        internal readonly IHttpContextAccessor HttpcontextAccesor;
        
        public DashboardControllerFixture()
        {
            EmployerService = Substitute.For<IEmployerService>();
            ServiceStatusHistoryService = Substitute.For<IServiceStatusHistoryService>();
            HttpcontextAccesor = Substitute.For<IHttpContextAccessor>();
            SubjectUnderTest = new DashboardController(EmployerService, ServiceStatusHistoryService);
        }

        public DashboardController SubjectUnderTest { get; }
        
        public DashboardController GetControllerWithClaims => new ClaimsBuilder<DashboardController>(SubjectUnderTest)
                                                                        .AddStandardUserPermission()
                                                                        .AddUserName("username")
                                                                        .Build();

    }
}