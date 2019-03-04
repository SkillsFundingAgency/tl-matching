using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class SearchResultsViewModelItem
    {
        public bool IsSelected { get; set; }
        public string Postcode { get; set; }
        public decimal? Distance { get; set; }
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
        public IEnumerable<string> QualificationShortTitles { get; set; }
    }
}
