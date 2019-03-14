﻿using System.Collections.Generic;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class Opportunity : BaseEntity
    {
        public int RouteId { get; set; }
        public string PostCode { get; set; }
        public short Distance { get; set; }
        public short? DropOffStage { get; set; }
        public string JobTitle { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
        public int? SearchResultProviderCount { get; set; }
        public string EmployerName { get; set; }
        public string EmployerContact { get; set; }
        public string EmployerContactEmail { get; set; }
        public string EmployerContactPhone { get; set; }
        public string UserEmail { get; set; }
        public bool? ConfirmationSelected { get; set; }

        public virtual Route Route { get; set; }
        public virtual ICollection<ProvisionGap> ProvisionGap { get; set; }
        public virtual ICollection<Referral> Referral { get; set; }
    }
}