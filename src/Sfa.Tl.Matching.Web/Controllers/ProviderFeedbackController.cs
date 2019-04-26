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
        private readonly IProviderFeedbackService _providerFeedbackService;

        public ProviderFeedbackController(IProviderFeedbackService providerFeedbackService, MatchingConfiguration configuration)
        {
            _providerFeedbackService = providerFeedbackService;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("send-provider-email", Name = "ConfirmSendProviderEmail")]
        public async Task<IActionResult> ConfirmSendProviderEmail()
        {
            var viewModel = new ConfirmSendProviderEmailViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [Route("send-provider-email", Name = "SendProviderEmail")]
        public async Task<IActionResult> ConfirmSendProviderEmail(ConfirmSendProviderEmailViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            // TODO Do the send

            return RedirectToRoute("SearchProvider");
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = 5000)]
        [Route("save-provider-feedback", Name = "SaveProviderFeedback")]
        public async Task<IActionResult> SaveProviderFeedback(SaveProviderFeedbackViewModel viewModel)
        {
            await _providerFeedbackService.UpdateProviderFeedback(viewModel);

            return RedirectToRoute("SearchProvider");
        }
    }
}
