using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Provider.Builders
{
    public class ValidProviderWithFundingBuilder
    {
        public Domain.Models.Provider Build() => new Domain.Models.Provider
        {
            Id = 1,
            UkPrn = 10000546,
            Name = "ProviderName",
            DisplayName = "Provider Display Name",
            OfstedRating = 1,
            IsCdfProvider = true,
            IsEnabledForReferral = true,
            PrimaryContact = "PrimaryContact",
            PrimaryContactEmail = "primary@contact.co.uk",
            PrimaryContactPhone = "01777757777",
            SecondaryContact = "SecondaryContact",
            SecondaryContactEmail = "secondary@contact.co.uk",
            SecondaryContactPhone = "01888801234",
            Source = "PMF_1018",
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn,
            ProviderVenue = new List<Domain.Models.ProviderVenue>
            {
                new Domain.Models.ProviderVenue
                {
                    Id = 1,
                    ProviderId = 1,
                    Postcode = "AA1 1AA",
                    Name = "AA1 1AA",
                    Town = "Town",
                    County = "County",
                    Latitude = 52.648869M,
                    Longitude = 2.095574M,
                    IsEnabledForReferral = true,
                    IsRemoved = false,
                    Source = "Test",
                    CreatedBy = EntityCreationConstants.CreatedByUser,
                    CreatedOn = EntityCreationConstants.CreatedOn,
                    ProviderQualification = new List<Domain.Models.ProviderQualification>
                    {
                        new Domain.Models.ProviderQualification
                        {
                            Id = 1,
                            ProviderVenueId = 1,
                            QualificationId = 1,
                            CreatedBy = EntityCreationConstants.CreatedByUser,
                            CreatedOn = EntityCreationConstants.CreatedOn,
                            Qualification = new Domain.Models.Qualification
                            {
                                Id = 1,
                                LarsId = "1001",
                                Title = "Title 1",
                                ShortTitle = "Short Title",
                                CreatedBy = EntityCreationConstants.CreatedByUser,
                                CreatedOn = EntityCreationConstants.CreatedOn
                            }
                        },
                        new Domain.Models.ProviderQualification
                        {
                            Id = 2,
                            ProviderVenueId = 1,
                            QualificationId = 2,
                            CreatedBy = EntityCreationConstants.CreatedByUser,
                            CreatedOn = EntityCreationConstants.CreatedOn,
                            Qualification = new Domain.Models.Qualification
                            {
                                Id = 2,
                                LarsId = "1002",
                                Title = "Title 2",
                                ShortTitle = "Duplicate Short Title",
                                CreatedBy = EntityCreationConstants.CreatedByUser,
                                CreatedOn = EntityCreationConstants.CreatedOn
                            }
                        },
                        new Domain.Models.ProviderQualification
                        {
                            Id = 3,
                            ProviderVenueId = 1,
                            QualificationId = 3,
                            CreatedBy = EntityCreationConstants.CreatedByUser,
                            CreatedOn = EntityCreationConstants.CreatedOn,
                            Qualification = new Domain.Models.Qualification
                            {
                                Id = 3,
                                LarsId = "1003",
                                Title = "Title 3",
                                ShortTitle = "Duplicate Short Title",
                                CreatedBy = EntityCreationConstants.CreatedByUser,
                                CreatedOn = EntityCreationConstants.CreatedOn
                            }
                        }
                    }
                }
            }
        };

        public Domain.Models.Provider BuildWithRemovedProviderVenue() => new Domain.Models.Provider
        {
            Id = 1,
            UkPrn = 10000546,
            Name = "ProviderName",
            OfstedRating = 1,
            IsCdfProvider = true,
            IsEnabledForReferral = true,
            PrimaryContact = "PrimaryContact",
            PrimaryContactEmail = "primary@contact.co.uk",
            PrimaryContactPhone = "01777757777",
            SecondaryContact = "SecondaryContact",
            SecondaryContactEmail = "secondary@contact.co.uk",
            SecondaryContactPhone = "01888801234",
            Source = "PMF_1018",
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn,
            ProviderVenue = new List<Domain.Models.ProviderVenue>
            {
                new Domain.Models.ProviderVenue
                {
                    Id = 1,
                    ProviderId = 1,
                    Postcode = "AA1 1AA",
                    Town = "Town",
                    County = "County",
                    Latitude = 52.648869M,
                    Longitude = 2.095574M,
                    IsEnabledForReferral = true,
                    IsRemoved = true,
                    Source = "Test",
                    CreatedBy = EntityCreationConstants.CreatedByUser,
                    CreatedOn = EntityCreationConstants.CreatedOn
                },
                new Domain.Models.ProviderVenue
                {
                    Id = 2,
                    ProviderId = 2,
                    Postcode = "AA2 2AA",
                    Town = "Town",
                    County = "County",
                    Latitude = 52.648869M,
                    Longitude = 2.095574M,
                    IsEnabledForReferral = true,
                    IsRemoved = false,
                    Source = "Test",
                    CreatedBy = EntityCreationConstants.CreatedByUser,
                    CreatedOn = EntityCreationConstants.CreatedOn,
                    ProviderQualification = new List<Domain.Models.ProviderQualification>
                    {
                        new Domain.Models.ProviderQualification
                        {
                            Id = 1,
                            ProviderVenueId = 1,
                            QualificationId = 1,
                            CreatedBy = EntityCreationConstants.CreatedByUser,
                            CreatedOn = EntityCreationConstants.CreatedOn,
                            Qualification = new Domain.Models.Qualification
                            {
                                Id = 1,
                                LarsId = "1001",
                                Title = "Title 1",
                                ShortTitle = "Short Title",
                                CreatedBy = EntityCreationConstants.CreatedByUser,
                                CreatedOn = EntityCreationConstants.CreatedOn
                            }
                        }
                    }
                }
            }
        };

    }
}
