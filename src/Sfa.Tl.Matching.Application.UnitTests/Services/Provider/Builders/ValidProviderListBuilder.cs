using System.Collections.Generic;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders
{
    public class ValidProviderListBuilder
    {
        public IList<Domain.Models.Provider> Build() => new List<Domain.Models.Provider>
        {
            new Domain.Models.Provider
            {
                Id = 1,
                UkPrn = 10000546,
                Name = "ProviderName",
                DisplayName = "Test Provider Display Name",
                OfstedRating = 1,
                IsCdfProvider = true,
                IsEnabledForReferral = true,
                Status = true,
                StatusReason = "StatusReason",
                PrimaryContact = "PrimaryContact",
                PrimaryContactEmail = "primary@contact.co.uk",
                PrimaryContactPhone = "01777757777",
                SecondaryContact = "SecondaryContact",
                SecondaryContactEmail = "secondary@contact.co.uk",
                SecondaryContactPhone = "01777757777",
                Source = "PMF_1018",
                CreatedBy = "CreatedBy",
                ModifiedBy = "ModifiedBy"
            },
            new Domain.Models.Provider
            {
                Id = 2,
                UkPrn = 10000123,
                Name = "ProviderName2",
                DisplayName = "Test Provider 2 Display Name",
                OfstedRating = 2,
                IsCdfProvider = false,
                IsEnabledForReferral = false,
                Status = false,
                StatusReason = "StatusReason2",
                PrimaryContact = "PrimaryContact2",
                PrimaryContactEmail = "anotherprimary@contact.co.uk",
                PrimaryContactPhone = "01777751111",
                SecondaryContact = "SecondaryContact2",
                SecondaryContactEmail = "anothersecondary@contact.co.uk",
                SecondaryContactPhone = "01777752222",
                Source = "PMF_1018",
                CreatedBy = "CreatedBy",
                ModifiedBy = "ModifiedBy"
            }
        };
    }
}
