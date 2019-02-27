using System;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class Opportunity : BaseEntity
    {
        public int RouteId { get; set; }
        public string PostCode { get; set; }
        public short Distance { get; set; }
        public string JobTitle { get; set; }
        public bool? PlacementsKnown { get; set; }
        public short? Placements { get; set; }
        public string EmployerName { get; set; }
        public string EmployerContact { get; set; }
        public string EmployerContactEmail { get; set; }
        public string EmployerContactPhone { get; set; }
        public string UserEmail { get; set; }
    }
}