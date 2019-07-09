namespace Sfa.Tl.Matching.Models.Dto
{
    public class CompanyNameDto : BaseOpportunityDto
    {
        public int? EmployerId { get; set; }
        public string CompanyName { get; set; }
        public bool HasChanged { get; set; }
    }
}