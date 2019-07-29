namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProvisionGapItemDto
    {
        public string Workplace { get; set; }
        public string JobRole { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
        public string Reason { get; set; }

        public string PlacementsDetail =>
                PlacementsKnown.GetValueOrDefault()
                    ? Placements.ToString()
                    : "at least 1";
    }
}