using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class SearchParametersViewModel
    {
        public const int DefaultSearchRadius = 30;
        public const int ZeroResultsSearchRadius = 30;
        [Required(ErrorMessage = "You must enter a postcode")]
        public string Postcode { get; set; }
        public int? SelectedRouteId { get; set; }
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public string CompanyNameWithAka { get; set; }
        public IList<SelectListItem> RoutesSelectList { get; set; }
    }
}