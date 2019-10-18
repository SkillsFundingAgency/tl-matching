using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.ViewComponents
{
    public class CancelViewComponent : ViewComponent
    {
        private readonly IOpportunityService _opportunityService;

        public CancelViewComponent(IOpportunityService opportunityService)
        {
            _opportunityService = opportunityService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int opportunityId, int opportunityItemId)
        {
            const string viewName = "Cancel";
            var viewModel = await GetViewModelAsync(opportunityId, opportunityItemId);

            return View(viewName, viewModel);
        }

        private async Task<CancelViewModel> GetViewModelAsync(int opportunityId, int opportunityItemId)
        {
            var viewModel = new CancelViewModel();
            if (opportunityId == 0) return viewModel;

            viewModel.OpportunityId = opportunityId;
            viewModel.OpportunityItemId = opportunityItemId;

            var opportunityItemCount = await _opportunityService.GetSavedOpportunityItemCountAsync(opportunityId);
            if (opportunityItemCount == 0) return viewModel;

            viewModel.CancelText = "Go back to all opportunities";

            return viewModel;
        }
    }
}