using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Home.Security
{
    public class When_Home_Controller_SaveServiceStatusHistory_Authorize_Attribute
    {
        private readonly AuthorizeAttribute _authorizeAttribute;

        public When_Home_Controller_SaveServiceStatusHistory_Authorize_Attribute()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

            var methodInfos = typeof(HomeController)
                .GetMember(nameof(HomeController.SaveServiceStatusHistoryAsync), MemberTypes.Method, flags)
                .Cast<MethodInfo>().ToList();

            _authorizeAttribute = methodInfos[0].GetCustomAttribute<AuthorizeAttribute>();
        }

        [Fact]
        public void Then_Authorize_Is_On_Post_Method() =>
            _authorizeAttribute.Should().NotBeNull();

        [Fact]
        public void Then_Authorize_Has_Expected_Role()
        {
            _authorizeAttribute.Roles.Should().Be(RolesExtensions.AdminUser);
        }
    }
}