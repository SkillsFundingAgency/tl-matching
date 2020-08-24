using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderVenueQualificationFileImportDto : FileImportDto
    {
        public override Stream FileDataStream { get; set; }
        public override int? NumberOfHeaderRows => 1;

        [Column(Order = 0)] public string UkPrn { get; set; }
        [Column(Order = 1)] public string InMatchingService { get; set; }
        [Column(Order = 2)] public string ProviderName { get; set; }
        [Column(Order = 3)] public string Name { get; set; }
        [Column(Order = 4)] public string DisplayName { get; set; }
        [Column(Order = 5)] public string IsCdfProvider { get; set; }
        [Column(Order = 6)] public string IsEnabledForReferral { get; set; }
        [Column(Order = 7)] public string PrimaryContact { get; set; }
        [Column(Order = 8)] public string PrimaryContactEmail { get; set; }
        [Column(Order = 9)] public string PrimaryContactPhone { get; set; }
        [Column(Order = 10)] public string SecondaryContact { get; set; }
        [Column(Order = 11)] public string SecondaryContactEmail { get; set; }
        [Column(Order = 12)] public string SecondaryContactPhone { get; set; }
        [Column(Order = 13)] public string VenuePostcode { get; set; }
        [Column(Order = 14)] public string Town { get; set; }
        [Column(Order = 15)] public string VenueName { get; set; }
        [Column(Order = 16)] public string VenueIsEnabledForReferral { get; set; }
        [Column(Order = 17)] public string VenueIsRemoved { get; set; }
        [Column(Order = 18)] public string LarId { get; set; }
        [Column(Order = 19)] public string QualificationTitle { get; set; }
        [Column(Order = 20)] public string QualificationShortTitle { get; set; }
        [Column(Order = 21)] public string QualificationIsOffered { get; set; }
        [Column(Order = 22)] public string Routes { get; set; }
    }
}