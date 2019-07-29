namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class RemoveEmployerViewModel
    {
        public int OpportunityId { get; set; }
        public string EmployerName { get; set; }
        public int EmployerCount { get; set; }
        public int OpportunityCount { get; set; }

        public string ConfirmDeleteText => OpportunityCount == 1
            ? $"Confirm you want to delete {OpportunityCount} opportunity created for {EmployerName}"
            : $"Confirm you want to delete {OpportunityCount} opportunities created for {EmployerName}";

        public string WarningDeleteText => EmployerCount == 1
            ? "This cannot be undone and will mean you have no more employers with saved opportunities."
            : "This cannot be undone.";

        public string SubmitActionText => EmployerCount == 1 
            ? "Confirm and finish" 
            : "Confirm and continue";
    }
}
