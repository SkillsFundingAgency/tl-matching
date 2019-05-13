using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Provider.Builders
{
    public class ValidProviderBuilder
    {
        public Domain.Models.Provider Build() => new Domain.Models.Provider
        {
            Id = 1,
            UkPrn = 10000546,
            Name = "ProviderName",
            OfstedRating = 1,
            Status = true,
            StatusReason = "StatusReason",
            PrimaryContact = "PrimaryContact",
            PrimaryContactEmail = "primary@contact.co.uk",
            PrimaryContactPhone = "01777757777",
            SecondaryContact = "SecondaryContact",
            SecondaryContactEmail = "secondary@contact.co.uk",
            SecondaryContactPhone = "01777757777",
            IsCdfProvider = true,
            IsEnabledForReferral = true,
            Source = "PMF_1018",
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
