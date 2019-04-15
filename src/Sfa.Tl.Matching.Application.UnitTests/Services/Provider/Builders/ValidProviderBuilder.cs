using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders
{
    public class ValidProviderBuilder
    {
        public Domain.Models.Provider Build() => new Domain.Models.Provider
        {
            Id = 1,
            UkPrn = 10000546,
            Name = "Test Provider",
            Status = true,
            PrimaryContact = "Test",
            PrimaryContactEmail = "Test@test.com",
            PrimaryContactPhone = "0123456789",
            SecondaryContact = "Test 2",
            SecondaryContactEmail = "Test2@test.com",
            SecondaryContactPhone = "0123456789",
            IsEnabledForSearch = true,
            IsEnabledForReferral = true,
            Source = "Test",
            CreatedBy = "CreatedBy",
            ModifiedBy = "ModifiedBy"
        };
        public Domain.Models.Provider BuildWithVenueAndQualifications() => new Domain.Models.Provider
        {
            Id = 1,
            UkPrn = 10000546,
            Name = "Test Name",
            DisplayName = "Test DisplayName",
            Status = true,
            PrimaryContact = "Test",
            PrimaryContactEmail = "Test@test.com",
            PrimaryContactPhone = "0123456789",
            SecondaryContact = "Test 2",
            SecondaryContactEmail = "Test2@test.com",
            SecondaryContactPhone = "0123456789",
            Source = "Test",
            CreatedBy = "CreatedBy",
            ModifiedBy = "ModifiedBy",
            IsEnabledForReferral = true,
            IsEnabledForSearch = true,
            ProviderVenue = new List<ProviderVenue>
            {
                new ProviderVenue
                {
                    IsEnabledForSearch = true,
                    Postcode = "CV1 1WT",
                    Id = 10,
                    ProviderQualification = new List<ProviderQualification>
                    {
                        new ProviderQualification
                        {
                            Id = 100,
                            NumberOfPlacements = 1,
                            QualificationId = 1000
                        }
                    }
                },
                new ProviderVenue
                {
                    IsEnabledForSearch = true,
                    Postcode = "CV1 2WT",
                    Id = 20,
                    ProviderQualification = new List<ProviderQualification>
                    {
                        new ProviderQualification
                        {
                            Id = 200,
                            NumberOfPlacements = 2,
                            QualificationId = 2000
                        },
                        new ProviderQualification
                        {
                            Id = 300,
                            NumberOfPlacements = 1,
                            QualificationId = 3000
                        }
                    }
                }
            }
        };
    }
}
