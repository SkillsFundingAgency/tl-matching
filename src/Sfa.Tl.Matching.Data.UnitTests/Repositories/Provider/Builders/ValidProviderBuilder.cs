using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Provider.Builders
{
    public class ValidProviderBuilder
    {
        public Domain.Models.Provider Build() => new()
        {
            Id = 1,
            UkPrn = 10000546,
            Name = "ProviderName",
            DisplayName = "Provider Display Name",
            OfstedRating = 1,
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
