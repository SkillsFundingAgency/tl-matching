using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.ViewComponents
{
    public class BackViewComponent : ViewComponent
    {
        private readonly IOpportunityService _opportunityService;

        public BackViewComponent(IOpportunityService opportunityService)
        {
            _opportunityService = opportunityService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int opportunityId, int opportunityItemId)
        {
            var viewName = "BackToStart";

            var opportunityItemCount = await _opportunityService.GetSavedOpportunityItemCountAsync(opportunityId);
            if (opportunityItemCount > 0)
                viewName = "BackToBasket";

            return View(viewName, new BackViewModel
            {
                OpportunityId = opportunityId,
                OpportunityItemId = opportunityItemId
            });
        }
    }
}