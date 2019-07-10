using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class RemoveEmployerViewModel
    {
        public int OpportunityId { get; set; }
        public string EmployerName { get; set; }
        public int EmployerCount { get; set; }
        public int Count { get; set; }

        public string ConfirmDeleteText => Count == 1
            ? $"Confirm you want to delete {Count} opportunity created for {EmployerName}?"
            : $"Confirm you want to delete {Count} opportunities created for {EmployerName}?";

        public string WarningDeleteText => EmployerCount == 1
            ? "This cannot be undone and will mean you have no more employers with saved opportunities."
            : "This cannot be undone.";
    }
}
