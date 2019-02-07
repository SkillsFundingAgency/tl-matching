 using System.ComponentModel.DataAnnotations;

 namespace Sfa.Tl.Matching.Domain.Models
{
    public class Provider
    {
        [Key]
        public int Id { get; set; }
        public long UkPrn { get; set; }
        public string Name { get; set; }
        public int OfstedRating { get; set; }
        public bool Active { get; set; }
        public string ActiveReason { get; set; }
        public string PrimaryContact { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string SecondaryContact { get; set; }
        public string SecondaryContactEmail { get; set; }
        public string SecondaryContactPhone { get; set; }
        public string Source { get; set; }
    }
}