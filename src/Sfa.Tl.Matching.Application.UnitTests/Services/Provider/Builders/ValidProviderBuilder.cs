namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders
{
    public class ValidProviderBuilder
    {
        public Domain.Models.Provider Build() => new Domain.Models.Provider
        {
            Id = 1,
            UkPrn = 10000546,
            Name = "Test Provider",
            DisplayName = "Test Provider Display Name",
            PrimaryContact = "Test",
            PrimaryContactEmail = "Test@test.com",
            PrimaryContactPhone = "0123456789",
            SecondaryContact = "Test 2",
            SecondaryContactEmail = "Test2@test.com",
            SecondaryContactPhone = "0123456789",
            IsCdfProvider = true,
            IsEnabledForReferral = true,
            Source = "Test",
            CreatedBy = "CreatedBy",
            ModifiedBy = "ModifiedBy"
        };
    }
}
