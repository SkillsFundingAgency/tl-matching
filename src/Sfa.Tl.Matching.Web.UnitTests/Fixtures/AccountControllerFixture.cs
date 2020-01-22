using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;

namespace Sfa.Tl.Matching.Web.UnitTests.Fixtures
{
    public class AccountControllerFixture
    {
        internal readonly AccountController AccountController;
        internal readonly IUrlHelper MockUrlHelper;
        public AccountControllerFixture()
        {
            var logger = Substitute.For<ILogger<AccountController>>();
            MockUrlHelper = Substitute.For<IUrlHelper>();

            AccountController = new AccountController(logger)
            {
                Url = MockUrlHelper
            };
        }

        public AccountController GetFailedLoginUser =>
            new ClaimsBuilder<AccountController>(AccountController).AddUserName("username").Build();

        public AccountController GetStandardLoginUser =>
            new ClaimsBuilder<AccountController>(AccountController).AddStandardUserPermission().Build();
    }
}