using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly IOpportunityService _opportunityService;

        public NavigationViewComponent(IOpportunityService opportunityService)
        {
            _opportunityService = opportunityService;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            int opportunityId, int opportunityItemId)
        {
            const string viewName = "Navigation";
            var viewModel = await GetViewModel(opportunityId, opportunityItemId);

            return View(viewName, viewModel);
        }

        private async Task<NavigationViewModel> GetViewModel(int opportunityId, int opportunityItemId)
        {
            var viewModel = new NavigationViewModel();
            if (opportunityId == 0) return viewModel;

            viewModel.OpportunityId = opportunityId;
            viewModel.OpportunityItemId = opportunityItemId;

            var opportunityItemCount = await _opportunityService.GetSavedOpportunityItemCountAsync(opportunityId);
            if (opportunityItemCount == 0) return viewModel;

            viewModel.CancelText = "Cancel this opportunity";

            return viewModel;
        }
    }
}