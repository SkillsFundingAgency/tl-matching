using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Database.StandingData
{
    internal class OpportunityData
    {
        private const int Opportunity1Id = 1000;
        private const int OpportunityItem1Id = 2000;
        private const int OpportunityItem2Id = 2001;
        private const int Referral1Id = 3000;
        private const int Referral2Id = 3001;

        internal static IList<Opportunity> Create()
        {
            return new List<Opportunity>
            {
                new Opportunity
                {
                    Id = Opportunity1Id,
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
                            Id = OpportunityItem1Id,
                            OpportunityId = Opportunity1Id,
                            OpportunityType = OpportunityType.ProvisionGap.ToString(),
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
                                    Id = Referral1Id,
                                    ProviderVenueId = 1,
                                    DistanceFromEmployer = 1.23m,
                                    CreatedOn = new DateTime(2019, 1, 1),
                                    CreatedBy = "IntegrationTests",
                                }
                            }
                        },
                        new OpportunityItem
                        {
                            Id = OpportunityItem2Id,
                            OpportunityId = Opportunity1Id,
                            OpportunityType = OpportunityType.Referral.ToString(),
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
                                    Id = Referral2Id,
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