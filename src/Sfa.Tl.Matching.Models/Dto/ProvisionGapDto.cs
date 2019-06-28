namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProvisionGapDto
    {
        public bool? NoSuitableStudent { get; set; }
        public bool? HadBadExperience { get; set; }
        public bool? ProvidersTooFarAway { get; set; }
        public string CreatedBy { get; set; }
    }
}