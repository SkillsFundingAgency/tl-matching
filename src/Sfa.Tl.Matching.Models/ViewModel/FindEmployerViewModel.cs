using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class FindEmployerViewModel
    {
        public int SelectedEmployerId { get; set; }
        public int OpportunityId { get; set; }

        [Required(ErrorMessage = "You must find and choose an employer")]
        public string EmployerName { get; set; }
        public string PreviousEmployerName { get; set; }
    }
}