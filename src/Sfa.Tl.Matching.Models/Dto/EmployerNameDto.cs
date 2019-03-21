namespace Sfa.Tl.Matching.Models.Dto
{
    public class EmployerNameDto : BaseOpportunityUpdateDto
    {
        public int EmployerId { get; set; }
        public string CompanyName { get; set; }
    }
}