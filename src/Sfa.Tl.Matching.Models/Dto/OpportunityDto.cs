using System;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class OpportunityDto
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public string Postcode { get; set; }
        public short Distance { get; set; }
        public short? DropOffStage { get; set; }
        public string JobTitle { get; set; }
        public bool? PlacementsKnown { get; set; }
        public bool? IsReferral { get; set; }
        public int? Placements { get; set; }
        public int? SearchResultProviderCount { get; set; }
        public string EmployerName { get; set; }
        public string EmployerContact { get; set; }
        public string EmployerContactEmail { get; set; }
        public string EmployerContactPhone { get; set; }
        public string UserEmail { get; set; }
        public bool? ConfirmationSelected { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}