using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Provider.Builders
{
    public class ValidProviderListBuilder
    {
        public IList<Domain.Models.Provider> Build() => new List<Domain.Models.Provider>
        {
            new()
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
            },
            new()
            {
                Id = 2,
                UkPrn = 10000123,
                Name = "ProviderName2",
                DisplayName = "Provider Display Name 2",
                OfstedRating = 2,
                PrimaryContact = "PrimaryContact2",
                PrimaryContactEmail = "anotherprimary@contact.co.uk",
                PrimaryContactPhone = "01777751111",
                SecondaryContact = "SecondaryCon1",
                SecondaryContactEmail = "anothersecondary@contact.co.uk",
                SecondaryContactPhone = "01777752222",
                IsCdfProvider = true,
                IsEnabledForReferral = true,
                Source = "PMF_1018",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
