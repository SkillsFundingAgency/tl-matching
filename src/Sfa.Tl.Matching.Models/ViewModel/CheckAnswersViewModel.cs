using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class CheckAnswersViewModel
    {
        public int OpportunityId { get; set; }
        public string EmployerName { get; set; }
        public string Route { get; set; }
        public string Postcode { get; set; }
        public short Distance { get; set; }
        public string JobTitle { get; set; }
        public short? Placements { get; set; }
        public string Contact { get; set; }
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must confirm that we can share the employer’s details with the selected providers.")]
        public bool ConfirmationSelected { get; set; }
        public string CreatedBy { get; set; }
    }
}