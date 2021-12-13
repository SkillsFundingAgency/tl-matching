using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders
{
    // ReSharper disable UnusedMember.Global
    public class ValidOpportunityBuilder
    {
        private readonly Domain.Models.Opportunity _opportunity;

        public ValidOpportunityBuilder()
        {
            _opportunity = new Domain.Models.Opportunity
            {
                Id = 1,
                EmployerCrmId = new Guid("55555555-5555-5555-5555-555555555555"),
                EmployerContact = "Employer Contact",
                EmployerContactPhone = "020 123 4567",
                EmployerContactEmail = "employer.contact@employer.co.uk",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            };
        }

        public ValidOpportunityBuilder AddEmployer()
        {
            _opportunity.Employer = new Domain.Models.Employer
            {
                Id = 5,
                CrmId = new Guid("55555555-5555-5555-5555-555555555555"),
                CompanyName = "Company",
                AlsoKnownAs = "Also Known As"
            };

            return this;
        }

        public ValidOpportunityBuilder AddReferrals(bool isCompleted = false, bool isSelectedForReferral = false, bool isCdfProvider = true)
        {
            _opportunity.OpportunityItem ??= new List<Domain.Models.OpportunityItem>();
            _opportunity.OpportunityItem.Add(
                new Domain.Models.OpportunityItem
                {
                    OpportunityId = 1,
                    OpportunityType = "Referral",
                    JobRole = "Automation Tester",
                    PlacementsKnown = true,
                    Placements = 5,
                    Town = "Coventry",
                    Postcode = "CV1 2WT",
                    IsSaved = true,
                    IsCompleted = isCompleted,
                    IsSelectedForReferral = isSelectedForReferral,
                    CreatedBy = EntityCreationConstants.CreatedByUser,
                    CreatedOn = EntityCreationConstants.CreatedOn,
                    ModifiedBy = EntityCreationConstants.ModifiedByUser,
                    ModifiedOn = EntityCreationConstants.ModifiedOn,
                    Route = new Domain.Models.Route
                    {
                        Id = 1,
                        Name = "Test Route",
                        CreatedBy = EntityCreationConstants.CreatedByUser,
                        CreatedOn = EntityCreationConstants.CreatedOn,
                    },
                    Referral = new List<Referral>
                    {
                        new()
                        {
                            ProviderVenueId = 1,
                            DistanceFromEmployer = 3.5M,
                            ProviderVenue = new Domain.Models.ProviderVenue
                            {
                                Id = 1,
                                ProviderId = 1,
                                Postcode = "AA1 1AA",
                                Town = "Town",
                                County = "County",
                                Name = "Venue name",
                                Latitude = 52.648869M,
                                Longitude = 2.095574M,
                                IsEnabledForReferral = true,
                                IsRemoved = false,
                                Source = "Test",
                                CreatedBy = EntityCreationConstants.CreatedByUser,
                                CreatedOn = EntityCreationConstants.CreatedOn,
                                Provider = new Domain.Models.Provider
                                {
                                    Id = 1,
                                    UkPrn = 10000546,
                                    Name = "Provider Name",
                                    DisplayName = "Provider display name",
                                    OfstedRating = 1,
                                    PrimaryContact = "PrimaryContact",
                                    PrimaryContactEmail = "primary@contact.co.uk",
                                    PrimaryContactPhone = "01777757777",
                                    SecondaryContact = "SecondaryContact",
                                    SecondaryContactEmail = "secondary@contact.co.uk",
                                    SecondaryContactPhone = "01777757777",
                                    IsCdfProvider = isCdfProvider,
                                    IsEnabledForReferral = true,
                                    Source = "PMF_1018",
                                    CreatedBy = EntityCreationConstants.CreatedByUser,
                                    CreatedOn = EntityCreationConstants.CreatedOn
                                }
                            }
                        },
                    }
                });

            return this;
        }

        public ValidOpportunityBuilder AddReferralsWithOneNonCdfProvider(bool isCompleted = false, bool isSelectedForReferral = false)
        {
            _opportunity.OpportunityItem ??= new List<Domain.Models.OpportunityItem>();

            _opportunity.OpportunityItem.Add(
                new Domain.Models.OpportunityItem
                {
                    OpportunityId = 1,
                    OpportunityType = "Referral",
                    JobRole = "Automation Tester",
                    PlacementsKnown = true,
                    Placements = 5,
                    Town = "Coventry",
                    Postcode = "CV1 2WT",
                    IsSaved = true,
                    IsCompleted = isCompleted,
                    IsSelectedForReferral = isSelectedForReferral,
                    CreatedBy = EntityCreationConstants.CreatedByUser,
                    CreatedOn = EntityCreationConstants.CreatedOn,
                    ModifiedBy = EntityCreationConstants.ModifiedByUser,
                    ModifiedOn = EntityCreationConstants.ModifiedOn,
                    Route = new Domain.Models.Route
                    {
                        Id = 1,
                        Name = "Test Route",
                        CreatedBy = EntityCreationConstants.CreatedByUser,
                        CreatedOn = EntityCreationConstants.CreatedOn,
                    },
                    Referral = new List<Referral>
                    {
                        new()
                        {
                            ProviderVenueId = 1,
                            DistanceFromEmployer = 3.5M,
                            ProviderVenue = new Domain.Models.ProviderVenue
                            {
                                Id = 1,
                                ProviderId = 1,
                                Postcode = "AA1 1AA",
                                Town = "Town",
                                County = "County",
                                Name = "Venue name",
                                Latitude = 52.648869M,
                                Longitude = 2.095574M,
                                IsEnabledForReferral = true,
                                IsRemoved = false,
                                Source = "Test",
                                CreatedBy = EntityCreationConstants.CreatedByUser,
                                CreatedOn = EntityCreationConstants.CreatedOn,
                                Provider = new Domain.Models.Provider
                                {
                                    Id = 1,
                                    UkPrn = 10000546,
                                    Name = "Provider Name",
                                    DisplayName = "Provider display name",
                                    OfstedRating = 1,
                                    PrimaryContact = "PrimaryContact",
                                    PrimaryContactEmail = "primary@contact.co.uk",
                                    PrimaryContactPhone = "01777757777",
                                    SecondaryContact = "SecondaryContact",
                                    SecondaryContactEmail = "secondary@contact.co.uk",
                                    SecondaryContactPhone = "01777757777",
                                    IsCdfProvider = false,
                                    IsEnabledForReferral = true,
                                    Source = "PMF_1018",
                                    CreatedBy = EntityCreationConstants.CreatedByUser,
                                    CreatedOn = EntityCreationConstants.CreatedOn
                                }
                            }
                        }
                    }
                });

            _opportunity.OpportunityItem.Add(
                new Domain.Models.OpportunityItem
                {
                    OpportunityId = 1,
                    OpportunityType = "Referral",
                    JobRole = "Tea Taster",
                    PlacementsKnown = true,
                    Placements = 2,
                    Town = "Coventry",
                    Postcode = "CV1 2WT",
                    IsSaved = true,
                    IsCompleted = isCompleted,
                    IsSelectedForReferral = isSelectedForReferral,
                    CreatedBy = EntityCreationConstants.CreatedByUser,
                    CreatedOn = EntityCreationConstants.CreatedOn,
                    ModifiedBy = EntityCreationConstants.ModifiedByUser,
                    ModifiedOn = EntityCreationConstants.ModifiedOn,
                    Route = new Domain.Models.Route
                    {
                        Id = 2,
                        Name = "Test Route 2",
                        CreatedBy = EntityCreationConstants.CreatedByUser,
                        CreatedOn = EntityCreationConstants.CreatedOn,
                    },
                    Referral = new List<Referral>
                    {
                        new()
                        {
                            ProviderVenueId = 2,
                            DistanceFromEmployer = 5.0M,
                            ProviderVenue = new Domain.Models.ProviderVenue
                            {
                                Id = 2,
                                ProviderId = 2,
                                Postcode = "BB2 2BB",
                                Town = "Town",
                                County = "County",
                                Name = "Venue name",
                                Latitude = 52.648869M,
                                Longitude = 2.095574M,
                                IsEnabledForReferral = true,
                                IsRemoved = false,
                                Source = "Test",
                                CreatedBy = EntityCreationConstants.CreatedByUser,
                                CreatedOn = EntityCreationConstants.CreatedOn,
                                Provider = new Domain.Models.Provider
                                {
                                    Id = 2,
                                    UkPrn = 10000321,
                                    Name = "Provider Name 2",
                                    DisplayName = "Provider display name 2",
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
                                    CreatedOn = EntityCreationConstants.CreatedOn
                                }
                            }
                        }
                    }
                });

            return this;
        }

        public ValidOpportunityBuilder AddProvisionGaps()
        {
            _opportunity.OpportunityItem ??= new List<Domain.Models.OpportunityItem>();
            _opportunity.OpportunityItem.Add(
                new Domain.Models.OpportunityItem
                {
                    OpportunityId = 1,
                    OpportunityType = "ProvisionGap",
                    JobRole = "Unknown job role",
                    Town = "London",
                    Postcode = "SW1 1AA",
                    PlacementsKnown = true,
                    Placements = 2,
                    IsSaved = true,
                    IsCompleted = false,
                    ProvisionGap = new List<Domain.Models.ProvisionGap>
                    {
                        new()
                        {
                            Id = 1,

                            HadBadExperience = true,
                            NoSuitableStudent = true,
                            ProvidersTooFarAway = true
                        }
                    }
                });

            return this;
        }

        public Domain.Models.Opportunity Build() => _opportunity;

        public ValidOpportunityBuilder AddAbandonedOpportunityItem()
        {
            _opportunity.OpportunityItem ??= new List<Domain.Models.OpportunityItem>();
            _opportunity.OpportunityItem.Add(
                new Domain.Models.OpportunityItem
                {
                    OpportunityId = 1,
                    OpportunityType = "Referral",
                    JobRole = "Automation Tester",
                    PlacementsKnown = false,
                    Placements = null,
                    Town = "Coventry",
                    Postcode = "CV1 2WT",
                    IsSaved = false,
                    IsCompleted = false,
                    CreatedBy = EntityCreationConstants.CreatedByUser,
                    CreatedOn = EntityCreationConstants.CreatedOn,
                    ModifiedBy = EntityCreationConstants.ModifiedByUser,
                    ModifiedOn = EntityCreationConstants.ModifiedOn,
                });

            return this;
        }

        public ValidOpportunityBuilder AddSavedOpportunityItem()
        {
            _opportunity.OpportunityItem ??= new List<Domain.Models.OpportunityItem>();
            _opportunity.OpportunityItem.Add(
                new Domain.Models.OpportunityItem
                {
                    OpportunityId = 1,
                    OpportunityType = "Referral",
                    JobRole = "Automation Tester",
                    PlacementsKnown = false,
                    Placements = null,
                    Town = "Coventry",
                    Postcode = "CV1 2WT",
                    IsSaved = true,
                    IsCompleted = false,
                    CreatedBy = EntityCreationConstants.CreatedByUser,
                    CreatedOn = EntityCreationConstants.CreatedOn,
                    ModifiedBy = EntityCreationConstants.ModifiedByUser,
                    ModifiedOn = EntityCreationConstants.ModifiedOn,
                });

            return this;
        }
    }
}