﻿using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderWithFundingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PrimaryContact { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string SecondaryContact { get; set; }
        public string SecondaryContactEmail { get; set; }
        public string SecondaryContactPhone { get; set; }
        public IEnumerable<ProviderVenueQualificationsInfoDto> ProviderVenues { get; set; }
        public string CreatedBy { get; set; }
    }
}
