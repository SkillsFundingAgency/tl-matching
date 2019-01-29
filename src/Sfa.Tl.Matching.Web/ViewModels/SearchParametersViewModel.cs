using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sfa.Tl.Matching.Web.ViewModels
{
    public class SearchParametersViewModel
    {
        [Required(ErrorMessage = "You must enter a postcode")]
        public string Postcode { get; set; }

        public string SelectedRouteId { get; set; }

        public IList<SelectListItem> RoutesSelectList { get; set; }
    }
}
