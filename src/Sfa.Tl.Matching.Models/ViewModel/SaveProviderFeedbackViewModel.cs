using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class SaveProviderFeedbackViewModel
    {
        public string SubmitAction { get; set; }
        public List<ProviderSearchResultItemViewModel> Providers { get; set; }
    }
}