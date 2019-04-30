using System.Security;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser)]
    public class ProviderFeedbackController : Controller
    {
        private readonly MatchingConfiguration _configuration;
        private readonly IProviderService _providerService;
        private readonly IProviderFeedbackService _providerFeedbackService;

        public ProviderFeedbackController(IProviderFeedbackService providerFeedbackService, 
            IProviderService providerService, 
            MatchingConfiguration configuration)
        {
            _providerService = providerService;
            _providerFeedbackService = providerFeedbackService;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("send-provider-email", Name = "ConfirmSendProviderEmail")]
        public async Task<IActionResult> ConfirmSendProviderEmail()
        {
            if (!HttpContext.User.IsAuthorisedAdminUser(_configuration.AuthorisedAdminUserEmail))
            {
                throw new SecurityException("User is not authorised to use this page");
            }

            var viewModel = await GetConfirmSendProviderEmailViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [Route("send-provider-email", Name = "SendProviderEmail")]
        public async Task<IActionResult> ConfirmSendProviderEmail(ConfirmSendProviderEmailViewModel viewModel)
        {
            if (!HttpContext.User.IsAuthorisedAdminUser(_configuration.AuthorisedAdminUserEmail))
            {
                throw new SecurityException("User is not authorised to use this page");
            }

            if (!ModelState.IsValid)
            {
                return View(await GetConfirmSendProviderEmailViewModel(viewModel));
            }

            if (viewModel.SendEmail.GetValueOrDefault())
            {
                var user = HttpContext.User.GetUserName();
                await _providerFeedbackService.RequestProviderQuarterlyUpdateAsync(user);
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