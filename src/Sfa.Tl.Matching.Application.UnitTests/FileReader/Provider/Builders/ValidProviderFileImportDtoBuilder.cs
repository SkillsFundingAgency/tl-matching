using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Builders
{
    public class ValidProviderFileImportDtoBuilder
    {
        public const string UkPrn = "10000546";
        public const string Name = "ProviderName";
        public const string OfstedRating = "Good";
        public const string Status = "Yes";
        public const string StatusReason = "StatusReason";
        public const string PrimaryContactName = "PrimaryContact";
        public const string PrimaryContactEmail = "primary@contact.co.uk";
        public const string PrimaryContactTelephone = "01777757777";
        public const string SecondaryContactName = "SecondaryContact";
        public const string SecondaryContactEmail = "secondary@contact.co.uk";
        public const string SecondaryContactTelephone = "01777757777";
        public const string Source = "PMF_1018";
        public const string CreatedBy = "CreatedBy";

        public ProviderFileImportDto Build() => new ProviderFileImportDto
        {
            UkPrn = UkPrn,
            ProviderName = Name,
            OfstedRating = OfstedRating,
            Status = Status,
            StatusReason = StatusReason,
            PrimaryContactName = PrimaryContactName,
            PrimaryContactEmail = PrimaryContactEmail,
            PrimaryContactTelephone = PrimaryContactTelephone,
            SecondaryContactName = SecondaryContactName,
            SecondaryContactEmail = SecondaryContactEmail,
            SecondaryContactTelephone = SecondaryContactTelephone,
            Source = Source,
            CreatedBy = CreatedBy
        };
    }
}