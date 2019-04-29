using System;
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

        public ProviderFeedbackController(IProviderFeedbackService providerFeedbackService, IProviderService providerService, MatchingConfiguration configuration)
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
                await _providerFeedbackService.SendProviderQuarterlyUpdateEmailAsync();
            }

            return RedirectToRoute("SearchProvider");
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = 5000)]
        [Route("save-provider-feedback", Name = "SaveProviderFeedback")]
        public async Task<IActionResult> SaveProviderFeedback(SaveProviderFeedbackViewModel viewModel)
        {
            await _providerFeedbackService.UpdateProviderFeedback(viewModel);

            if (!string.IsNullOrWhiteSpace(viewModel.SubmitAction) &&
                string.Equals(viewModel.SubmitAction, "SendEmail", StringComparison.InvariantCultureIgnoreCase))
                return RedirectToRoute("ConfirmSendProviderEmail");

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
