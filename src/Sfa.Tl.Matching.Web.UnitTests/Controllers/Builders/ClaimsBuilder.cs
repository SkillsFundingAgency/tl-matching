using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders
{
    internal class ClaimsBuilder<T> where T : ControllerBase
    {
        private readonly T _controller;
        private readonly List<Claim> _claimList = new();

        internal ClaimsBuilder(T controller)
        {
            _controller = controller;
        }

        internal ClaimsBuilder<T> Add(string type, string value)
        {
            _claimList.Add(new Claim(type, value));

            return this;
        }

        internal ClaimsBuilder<T> AddAdminUser()
        {
            _claimList.Add(new Claim(ClaimTypes.Role, RolesExtensions.AdminUser));

            return this;
        }

        internal ClaimsBuilder<T> AddStandardUser()
        {
            _claimList.Add(new Claim(ClaimTypes.Role, RolesExtensions.StandardUser));

            return this;
        }

        internal ClaimsBuilder<T> AddUserName(string username)
        {
            _claimList.Add(new Claim(ClaimTypes.GivenName, username));

            return this;
        }

        internal ClaimsBuilder<T> AddEmail(string email)
        {
            _claimList.Add(new Claim(ClaimTypes.Upn, email));

            return this;
        }

        internal T Build()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(_claimList));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            return _controller;
        }
    }
}