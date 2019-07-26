using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Database.StandingData
{
    internal class OpportunityData
    {
        internal static IList<Opportunity> Create()
        {
            return new List<Opportunity>
            {
                new Opportunity
                {
                    Id = 1,
                    EmployerId = 1,
                    EmployerContact = "Employer Contact",
                    EmployerContactEmail = "employer-contact@email.com",
                    EmployerContactPhone = "01474 787878",
                    CreatedOn = new DateTime(2019, 1, 1),
                    CreatedBy = "IntegrationTests",
                    OpportunityItem = new List<OpportunityItem>
                    {
                        new OpportunityItem
                        {
                            Id = 1,
                            OpportunityId = 1,
                            OpportunityType = "Referral",
                            Town = "London",
                            Postcode = "SW1A 2AA",
                            SearchRadius = 10,
                            SearchResultProviderCount = 4,
                            JobRole = "Job Role",
                            PlacementsKnown = true,
                            Placements = 1,
                            IsSaved = false,
                            IsSelectedForReferral = false,
                            IsCompleted = false,
                            RouteId = 1,
                            CreatedOn = new DateTime(2019, 1, 1),
                            CreatedBy = "IntegrationTests",
                            Referral = new List<Referral>
                            {
                                new Referral
                                {
                                    Id = 1,
                                    ProviderVenueId = 1,
                                    DistanceFromEmployer = 1.23m,
                                    CreatedOn = new DateTime(2019, 1, 1),
                                    CreatedBy = "IntegrationTests",
                                }
                            }
                        },
                        new OpportunityItem
                        {
                            Id = 2,
                            OpportunityId = 1,
                            OpportunityType = "Referral",
                            Town = "London",
                            Postcode = "SW1A 2AA",
                            SearchRadius = 10,
                            SearchResultProviderCount = 4,
                            JobRole = "Job Role",
                            PlacementsKnown = true,
                            Placements = 1,
                            IsSaved = false,
                            IsSelectedForReferral = false,
                            IsCompleted = false,
                            RouteId = 1,
                            CreatedOn = new DateTime(2019, 1, 1),
                            CreatedBy = "IntegrationTests",
                            Referral = new List<Referral>
                            {
                                new Referral
                                {
                                    Id = 2,
                                    ProviderVenueId = 1,
                                    DistanceFromEmployer = 1.23m,
                                    CreatedOn = new DateTime(2019, 1, 1),
                                    CreatedBy = "IntegrationTests",
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}