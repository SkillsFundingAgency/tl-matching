using System.Collections.Generic;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders
{
    public class ValidProviderBuilder
    {
        private readonly Domain.Models.Provider _provider;

        public ValidProviderBuilder()
        {
            _provider = new Domain.Models.Provider
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

        public ValidProviderBuilder AddProviderVenuesWithQualifications()
        {
            _provider.ProviderVenue ??= new List<Domain.Models.ProviderVenue>();
            _provider.ProviderVenue.Add(new Domain.Models.ProviderVenue
            {
                Id = 10,
                ProviderId = _provider.Id,
                Postcode = "CV1 1WT",
                IsEnabledForReferral = true,
                CreatedBy = "CreatedBy",
                Provider = _provider,
                ProviderQualification = new List<Domain.Models.ProviderQualification>
                {
                    new()
                    {
                        Qualification = new Domain.Models.Qualification
                        {
                            Id = 1,
                            LarId = "12345678",
                            Title = "Qualification 1",
                            ShortTitle = "Qualification 1 Short Title"
                        }
                    }
                }
            });

            _provider.ProviderVenue.Add(new Domain.Models.ProviderVenue
            {
                Id = 20,
                ProviderId = _provider.Id,
                Postcode = "CV1 2WT",
                IsEnabledForReferral = true,
                CreatedBy = "CreatedBy",
                Provider = _provider,
                ProviderQualification = new List<Domain.Models.ProviderQualification>
                {
                    new()
                    {
                        Qualification = new Domain.Models.Qualification
                        {
                            Id = 2,
                            LarId = "00001234",
                            Title = "Qualification 2",
                            ShortTitle = "Qualification 2 Short Title"
                        }
                    },
                    new()
                    {
                        Qualification = new Domain.Models.Qualification
                        {
                            Id = 3,
                            LarId = "00005678",
                            Title = "Qualification 3",
                            ShortTitle = "Qualification 3 Short Title"
                        }
                    }
                }
            });

            return this;
        }

        public Domain.Models.Provider Build() => _provider;
    }
}
