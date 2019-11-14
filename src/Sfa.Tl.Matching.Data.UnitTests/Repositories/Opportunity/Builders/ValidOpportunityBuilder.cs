using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders
{
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

        public ValidOpportunityBuilder AddReferrals(bool isCompleted = false)
        {
            if (_opportunity.OpportunityItem == null)
            {
                _opportunity.OpportunityItem = new List<Domain.Models.OpportunityItem>();
            }

            _opportunity.OpportunityItem.Add(
                new Domain.Models.OpportunityItem
                {
                    //Id = _opportunity.OpportunityItem.Count + 1,
                    OpportunityId = 1,
                    OpportunityType = "Referral",
                    JobRole = "Automation Tester",
                    PlacementsKnown = true,
                    Placements = 5,
                    Town = "Coventry",
                    Postcode = "CV1 2WT",
                    IsSaved = true,
                    IsCompleted = isCompleted,
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
                        new Referral
                        {
                            //Id = 1,
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
                                    Name = "ProviderName",
                                    DisplayName = "Provider display name",
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
                                },
                            }
                        }
                    }
                });

            return this;
        }

        public ValidOpportunityBuilder AddProvisionGaps()
        {
            if (_opportunity.OpportunityItem == null)
            {
                _opportunity.OpportunityItem = new List<Domain.Models.OpportunityItem>();
            }

            _opportunity.OpportunityItem.Add(
                new Domain.Models.OpportunityItem
                {
                    //Id = _opportunity.OpportunityItem.Count + 1,
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
                        new Domain.Models.ProvisionGap
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
            if (_opportunity.OpportunityItem == null)
            {
                _opportunity.OpportunityItem = new List<Domain.Models.OpportunityItem>();
            }

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
            if (_opportunity.OpportunityItem == null)
            {
                _opportunity.OpportunityItem = new List<Domain.Models.OpportunityItem>();
            }

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