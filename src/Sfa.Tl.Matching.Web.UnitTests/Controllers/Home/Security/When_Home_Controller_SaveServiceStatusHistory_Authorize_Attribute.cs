using System;
using System.Diagnostics;
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
    public class When_Home_Controller_SaveServiceStatusHistory_Authorize_Attribute
    {
        private readonly AuthorizeAttribute _authorizeAttribute;

        private readonly ITestOutputHelper _output;

        public When_Home_Controller_SaveServiceStatusHistory_Authorize_Attribute(ITestOutputHelper output)
        {
            _output = output;

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

            var methodInfos = typeof(HomeController)
                .GetMember(nameof(HomeController.SaveServiceStatusHistoryAsync), MemberTypes.Method, flags)
                .Cast<MethodInfo>().ToList();

            for (var i = 0; i < methodInfos.Count; i++)
            {
                _output.WriteLine($"O::index = {i} - method {methodInfos[i].Name}");
                var attr = methodInfos[i].GetCustomAttributes();
                foreach (var a in attr)
                {
                    _output.WriteLine($"   O::Attribute {a.GetType().FullName}");
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