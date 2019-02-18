using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class EmployerNameViewModel
    {
        [Required(ErrorMessage = "You must enter a business name")]
        public string BusinessName { get; set; }
    }
}