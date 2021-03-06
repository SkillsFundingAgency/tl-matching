﻿using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class AddQualificationViewModel
    {
        public int ProviderVenueId { get; set; }
        public int QualificationId { get; set; }
        public string Postcode { get; set; }
        [Required(ErrorMessage = "You must enter a learning aim reference (LAR)")]
        public string LarId { get; set; }
        public string Source { get; set; }
    }
}