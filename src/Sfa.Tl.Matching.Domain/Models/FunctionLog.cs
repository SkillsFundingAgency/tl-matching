namespace Sfa.Tl.Matching.Domain.Models
{
    public class FunctionLog : BaseEntity
    {
        public string FunctionName { get; set; }
        public int RowNumber { get; set; }
        public string ErrorMessage { get; set; }
    }
}