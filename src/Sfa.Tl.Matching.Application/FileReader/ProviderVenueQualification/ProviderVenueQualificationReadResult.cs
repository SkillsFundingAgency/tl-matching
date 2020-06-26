using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Application.FileReader.ProviderVenueQualification

{
    public class ProviderVenueQualificationReadResult
    {
        public List<Domain.Models.ProviderVenueQualification> Qualifications { get; set; }
        public string Error { get; set; }
    }
}