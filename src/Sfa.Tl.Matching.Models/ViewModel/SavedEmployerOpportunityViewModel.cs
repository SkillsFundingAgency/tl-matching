using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class SavedEmployerOpportunityViewModel
    {
        public IList<EmployerOpportunityViewModel> EmployerOpportunities { get; set; } =
            new List<EmployerOpportunityViewModel>();
    }
}