using System;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class OpportunityDto
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public string PostCode { get; set; }
        public short Distance { get; set; }
        public string JobTitle { get; set; }
        public bool? PlacementsKnown { get; set; }
        public short? Placements { get; set; }
        public Guid? EmployerCrmId { get; set; }
        public string EmployerName { get; set; }
        public string EmployerAupa { get; set; }
        public string EmployerOwner { get; set; }
        public string Contact { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string LocalAuthority { get; set; }
        public string UserEmail { get; set; }
        public string CreatedBy { get; set; }
    }
}