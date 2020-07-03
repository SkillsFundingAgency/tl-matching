using Sfa.Tl.Matching.Models.Dto;
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Application.FileReader.ProviderVenueQualification

{
    public class ProviderVenueQualificationReadResult
    {
        public List<ProviderVenueQualificationDto> Qualifications { get; set; }
        public string Error { get; set; }
    }
}