using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;
using Xunit.Abstractions;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Home.Security
{
    public class When_Home_Controller_Maintenance_Action_Authorize_Attribute
    {
        private readonly AuthorizeAttribute _authorizeAttribute;

        public When_Home_Controller_Maintenance_Action_Authorize_Attribute(ITestOutputHelper output)
        {
            var output1 = output;

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

            var methodInfos = typeof(HomeController)
                .GetMember(nameof(HomeController.MaintenanceAsync), MemberTypes.Method, flags)
                .Cast<MethodInfo>().ToList();

            for (var i = 0; i < methodInfos.Count; i++)
            {
                output1.WriteLine($"O::index = {i} - method {methodInfos[i].Name}");
                var attr = methodInfos[i].GetCustomAttributes();
                foreach (var a in attr)
                {
                    output1.WriteLine($"   O::Attribute {a.GetType().FullName}");
                }
            }

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