using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders
{
    public class ValidOpportunityReferralBuilder
    {
        public Domain.Models.Opportunity Build() => new Domain.Models.Opportunity
        {
            Id = 1,
            RouteId = 1,
            Postcode = "AA1 1AA",
            SearchRadius = 10,
            JobTitle = "Testing Job Title",
            DropOffStage = 9,
            PlacementsKnown = true,
            Placements = 3,
            SearchResultProviderCount = 12,
            EmployerId = 5,
            EmployerName = "Employer",
            EmployerContact = "Employer Contact",
            EmployerContactPhone = "020 123 4567",
            EmployerContactEmail = "employer.contact@employer.co.uk",
            UserEmail = "employer.contact@employer.co.uk",
            ConfirmationSelected = true,
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn,
            Route = new Domain.Models.Route
            {
                Id = 1,
                Name = "Test Route",
                Keywords = "Keywords",
                Summary = "Summary",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                Path = new List<Domain.Models.Path>
                {
                    new Domain.Models.Path
                    {
                        Id = 1,
                        Name = "Path1"
                    },
                    new Domain.Models.Path
                    {
                        Id = 2,
                        Name = "Path2"
                    }
                }
            },
            Referral = new List<Referral>
            {
                new Referral
                {
                    Id = 1,
                    OpportunityId = 1,
                    ProviderVenueId = 1,
                    DistanceFromEmployer = 3.5M,
                    CreatedBy = EntityCreationConstants.CreatedByUser,
                    CreatedOn = EntityCreationConstants.CreatedOn,
                    ProviderVenue = new Domain.Models.ProviderVenue
                    {
                        Id = 1,
                        ProviderId = 1,
                        Postcode = "AA1 1AA",
                        Town = "Town",
                        County = "County",
                        Latitude = 52.648869M,
                        Longitude = 2.095574M,
                        Source = "Test",
                        CreatedBy = EntityCreationConstants.CreatedByUser,
                        CreatedOn = EntityCreationConstants.CreatedOn,
                        Provider = new Domain.Models.Provider
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
                            CreatedOn = EntityCreationConstants.CreatedOn
                        },
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
                                    Title = "Title",
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
                                    CreatedOn = EntityCreationConstants.CreatedOn,
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
            }
        };

        public IList<Domain.Models.QualificationRoutePathMapping> BuildQualificationRoutePathMapping() => new List<Domain.Models.QualificationRoutePathMapping>
        {
            new Domain.Models.QualificationRoutePathMapping
            {
                Id = 1,
                PathId = 1,
                Source = "Test",
                QualificationId = 1,
            },
            new Domain.Models.QualificationRoutePathMapping
            {
                Id = 2,
                PathId = 1,
                Source = "Test",
                QualificationId = 2,
            },
            new Domain.Models.QualificationRoutePathMapping
            {
                Id = 3,
                PathId = 2,
                Source = "Test",
                QualificationId = 1,
            },
            new Domain.Models.QualificationRoutePathMapping
            {
                Id = 4,
                PathId = 2,
                Source = "Test",
                QualificationId = 2,
            },
            new Domain.Models.QualificationRoutePathMapping
            {
                Id = 5,
                PathId = 3,
                Source = "Test",
                QualificationId = 3,
            },
            new Domain.Models.QualificationRoutePathMapping
            {
                Id = 6,
                PathId = 4,
                Source = "Test",
                QualificationId = 3,
            }
        };
    }
}
