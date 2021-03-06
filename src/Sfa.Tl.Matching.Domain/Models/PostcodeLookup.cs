﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.Matching.Domain.Models
{
    // ReSharper disable once UnusedMember.Global
    public class PostcodeLookup : BaseEntity
    {
        [MergeKey]
        public string Postcode { get; set; } 
        public string PrimaryLepCode { get; set; }
        public string SecondaryLepCode { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int ChecksumCol { get; set; }
    }
}
