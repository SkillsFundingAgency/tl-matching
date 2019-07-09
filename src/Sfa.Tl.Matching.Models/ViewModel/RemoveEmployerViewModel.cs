using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class RemoveEmployerViewModel
    {
        public int OpportunityId { get; set; }
        public int Count { get; set; }
        public string ConfirmDeleteText { get; set; }
        public string WarningDeleteText { get; set; }
    }
}
