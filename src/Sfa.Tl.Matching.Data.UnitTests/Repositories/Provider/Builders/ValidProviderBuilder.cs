using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Provider.Builders
{
    internal class ValidProviderBuilder
    {
        private readonly Domain.Models.Provider _provider;

        public ValidProviderBuilder()
        {
            _provider = new Domain.Models.Provider
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
                Source = "PMF_1018",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            };
        }

        public Domain.Models.Provider Build() =>
            _provider;
    }
}
