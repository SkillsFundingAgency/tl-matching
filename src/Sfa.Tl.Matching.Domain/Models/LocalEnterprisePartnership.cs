
namespace Sfa.Tl.Matching.Domain.Models
{
    // ReSharper disable once UnusedMember.Global
    public class LocalEnterprisePartnership : BaseEntity
    {
        [MergeKey]
        public string Code { get; set; } 
        public string Name { get; set; }
    }
}
