using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class LocalAuthorityMapping
    {
        [Key]
        public Guid Id { get; set; }
        public string LocalAuthorityCode { get; set; }
        public string LocalAuthority { get; set; }
        public string LocalEnterprisePartnership { get; set; }
        public bool InMultipleLeps { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}