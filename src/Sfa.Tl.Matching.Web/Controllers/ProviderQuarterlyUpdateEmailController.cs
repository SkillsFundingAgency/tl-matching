using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser)]
    public class ProviderQuarterlyUpdateEmailController : Controller
    {
        private readonly MatchingConfiguration _configuration;
        private readonly IProviderService _providerService;
        private readonly IProviderQuarterlyUpdateEmailService _providerQuarterlyUpdateEmailService;

        public ProviderQuarterlyUpdateEmailController(IProviderQuarterlyUpdateEmailService providerQuarterlyUpdateEmailService,
            IProviderService providerService,
            MatchingConfiguration configuration)
        {
            _providerService = providerService;
            _providerQuarterlyUpdateEmailService = providerQuarterlyUpdateEmailService;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("send-provider-email", Name = "ConfirmSendProviderEmail")]
        public async Task<IActionResult> ConfirmSendProviderEmailAsync()
        {
            if (!HttpContext.User.IsAuthorisedAdminUser(_configuration.AuthorisedAdminUserEmail))
            {
                return RedirectToRoute("FailedLogin");
            }

            var viewModel = await GetConfirmSendProviderEmailViewModel();
            return View("ConfirmSendProviderEmail", viewModel);
        }

        [HttpPost]
        [Route("send-provider-email", Name = "SendProviderEmail")]
        public async Task<IActionResult> ConfirmSendProviderEmailAsync(ConfirmSendProviderEmailViewModel viewModel)
        {
            if (!HttpContext.User.IsAuthorisedAdminUser(_configuration.AuthorisedAdminUserEmail))
            {
                return RedirectToRoute("FailedLogin");
            }

            if (!ModelState.IsValid)
            {
                return View("ConfirmSendProviderEmail", await GetConfirmSendProviderEmailViewModel(viewModel));
            }

            if (viewModel.SendEmail.GetValueOrDefault())
            {
                var user = HttpContext.User.GetUserName();
                await _providerQuarterlyUpdateEmailService.RequestProviderQuarterlyUpdateAsync(user);
            }

            return RedirectToRoute("SearchProvider");
        }

        private async Task<ConfirmSendProviderEmailViewModel> GetConfirmSendProviderEmailViewModel(ConfirmSendProviderEmailViewModel viewModel = null)
        {
            var count = await _providerService.GetProvidersWithFundingCountAsync();
            return new ConfirmSendProviderEmailViewModel
            {
                ProviderCount = count,
                SendEmail = viewModel?.SendEmail
            };
        }
    }
}