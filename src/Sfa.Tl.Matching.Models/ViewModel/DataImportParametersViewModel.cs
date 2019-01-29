using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class DataImportParametersViewModel
    {
        public SelectListItem[] ImportType { get; set; }
        public int SelectedImportTypeId { get; set; }
        public bool IsImportsuccessful { get; set; }
    }
}