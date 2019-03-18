namespace Sfa.Tl.Matching.Domain.Models
{
    public class EmailPlaceholder : BaseEntity
    {
        public int EmailHistoryId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public virtual EmailHistory EmailHistory { get; set; }
    }
}