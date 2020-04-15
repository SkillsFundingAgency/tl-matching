
namespace Sfa.Tl.Matching.Domain.Models
{
    // ReSharper disable once UnusedMember.Global
    public class PostcodeLookup : BaseEntity
    {
        [MergeKey]
        public string Postcode { get; set; } 
        public string LepCode { get; set; } 
    }
}
