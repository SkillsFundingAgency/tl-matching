using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Provider.Builders
{
    internal class ValidProviderListBuilder
    {
        private readonly IList<Domain.Models.Provider> _provider;

        public ValidProviderListBuilder()
        {
            _provider = new List<Domain.Models.Provider>
            {
                new Domain.Models.Provider
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
                },
                new Domain.Models.Provider
                {
                    Id = 2,
                    UkPrn = 10000123,
                    Name = "ProviderName2",
                    OfstedRating = 2,
                    Status = true,
                    StatusReason = "StatusReason2",
                    PrimaryContact = "PrimaryContact2",
                    PrimaryContactEmail = "anotherprimary@contact.co.uk",
                    PrimaryContactPhone = "01777751111",
                    SecondaryContact = "SecondaryCon1",
                    SecondaryContactEmail = "anothersecondary@contact.co.uk",
                    SecondaryContactPhone = "01777752222",
                    Source = "PMF_1018",
                    CreatedBy = EntityCreationConstants.CreatedByUser,
                    CreatedOn = EntityCreationConstants.CreatedOn,
                    ModifiedBy = EntityCreationConstants.ModifiedByUser,
                    ModifiedOn = EntityCreationConstants.ModifiedOn
                }
            };
        }

        public IEnumerable<Domain.Models.Provider> Build() =>
            _provider;
    }
}
