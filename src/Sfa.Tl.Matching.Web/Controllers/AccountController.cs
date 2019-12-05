using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Extensions;

namespace Sfa.Tl.Matching.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            _logger.LogInformation("Start of Sign In");
            var redirectUrl = Url.Action(nameof(PostSignIn), "Account");
            return Challenge(
                new AuthenticationProperties { RedirectUri = redirectUrl },
                WsFederationDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public IActionResult PostSignIn()
        {
            if(!HttpContext.User.HasValidRole())
            {
                _logger.LogInformation($"PostSignIn - User '{HttpContext.User.Identity.Name}' does not have a valid role");
                foreach (var cookie in Request.Cookies.Keys)
                {
                    if(string.Equals(cookie, "seen_cookie_message", StringComparison.InvariantCultureIgnoreCase)) continue;
                    Response.Cookies.Delete(cookie);
                }

                return RedirectToAction("FailedLogin", "Home");
            }

            return RedirectToAction("Start", "Dashboard");
        }

        [HttpGet]
        public async Task<SignOutResult> SignOut()
        {
            var callbackUrl = Url.Action(nameof(SignedOut), "Account", null, Request.Scheme);

            foreach (var cookie in Request.Cookies.Keys)
            {
                if(string.Equals(cookie, "seen_cookie_message", StringComparison.InvariantCultureIgnoreCase)) continue;
                Response.Cookies.Delete(cookie);
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                CookieAuthenticationDefaults.AuthenticationScheme,
                WsFederationDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public IActionResult SignedOut()
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}