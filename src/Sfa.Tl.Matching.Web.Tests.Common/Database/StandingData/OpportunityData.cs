using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Web.Tests.Common.Database.StandingData
{
    internal class OpportunityData
    {
        private const int Opportunity1Id = 1000;
        private const int OpportunityItem1Id = 2000;
        private const int OpportunityItem2Id = 2001;
        private const int Referral1Id = 3000;
        private const int Referral2Id = 3001;

        // ReferralSingle
        private const int OpportunityReferralSingleId = 1010;
        private const int OpportunityItemReferralSingleId = 1011;
        private const int ReferralSingleId = 1012;

        // ProvisionGapSingle
        private const int OpportunityProvisionGapSingleId = 1020;
        private const int OpportunityItemProvisionGapSingleId = 1021;
        private const int ProvisionGapSingleId = 1022;

        // ReferralMultiple
        private const int OpportunityReferralMultipleId = 1030;
        private const int OpportunityItemReferralMultiple1Id = 1031;
        private const int OpportunityItemReferralMultiple2Id = 1032;
        private const int ReferralMultiple1Id = 1033;
        private const int ReferralMultiple2Id = 1034;
        private const int ReferralMultiple3Id = 1035;

        // SingleReferralAndProvisionGap
        private const int OpportunityReferralSingleAndProvisionGapId = 1040;
        private const int OpportunityItemReferralSingleAndProvisionGap1Id = 1041;
        private const int OpportunityItemReferralSingleAndProvisionGap2Id = 1042;
        private const int ReferralSingle1Id = 1043;
        private const int ProvisionGapSingle1Id = 1044;

        // MultipleReferralAndProvisionGap
        private const int OpportunityMultipleReferralAndProvisionGapId = 1050;
        private const int OpportunityItemMultipleReferralAndProvisionGap1Id = 1051;
        private const int OpportunityItemMultipleReferralAndProvisionGap2Id = 1052;
        private const int OpportunityItemMultipleReferralAndProvisionGap3Id = 1053;
        private const int ProvisionGapMultipleReferralAndProvisionGap1Id = 1054;
        private const int ReferralAndProvisionGapMultiple1Id = 1055;
        private const int ReferralAndProvisionGapMultiple2Id = 1056;

        // Multiple Providers
        private const int OpportunityProviderMultipleId = 1060;
        private const int OpportunityItemProviderMultipleId = 1061;
        private const int ProviderReferral1Id = 1062;
        private const int ProviderReferral2Id = 1063;
        
        internal static Opportunity CreateReferralSingle()
        {
            return new()
            {
                Id = OpportunityReferralSingleId,
                EmployerCrmId = new Guid("11111111-1111-1111-1111-111111111111"),
                EmployerContact = "Employer Contact",
                EmployerContactEmail = "employer-contact@email.com",
                EmployerContactPhone = "01474 787878",
                CreatedOn = new(2019, 1, 1),
                CreatedBy = "Dev Surname",
                OpportunityItem = new List<OpportunityItem>
                {
                    new()
                    {
                        Id = OpportunityItemReferralSingleId,
                        OpportunityId = OpportunityReferralSingleId,
                        OpportunityType = OpportunityType.Referral.ToString(),
                        Town = "London",
                        Postcode = "SW1A 2AA",
                        SearchRadius = 25,
                        SearchResultProviderCount = 1,
                        JobRole = "Job Role",
                        PlacementsKnown = true,
                        Placements = 1,
                        IsSaved = true,
                        IsSelectedForReferral = false,
                        IsCompleted = false,
                        RouteId = 1,
                        CreatedOn = new(2019, 1, 1),
                        CreatedBy = "Dev Surname",
                        Referral = new List<Referral>
                        {
                            new()
                            {
                                Id = ReferralSingleId,
                                ProviderVenueId = 1,
                                DistanceFromEmployer = 1.23m,
                                CreatedOn = new(2019, 1, 1),
                                CreatedBy = "Dev Surname",
                            }
                        }
                    }
                }
            };
        }

        internal static Opportunity CreateReferralMultiple()
        {
            return new()
            {
                Id = OpportunityReferralMultipleId,
                EmployerCrmId = new Guid("11111111-1111-1111-1111-111111111111"),
                EmployerContact = "Employer Contact",
                EmployerContactEmail = "employer-contact@email.com",
                EmployerContactPhone = "01474 787878",
                CreatedOn = new(2019, 1, 2),
                CreatedBy = "Dev Surname",
                OpportunityItem = new List<OpportunityItem>
                {
                    new()
                    {
                        Id = OpportunityItemReferralMultiple1Id,
                        OpportunityId = OpportunityReferralMultipleId,
                        OpportunityType = OpportunityType.Referral.ToString(),
                        Town = "London",
                        Postcode = "SW1A 2AA",
                        SearchRadius = 25,
                        SearchResultProviderCount = 1,
                        JobRole = "Job Role",
                        PlacementsKnown = true,
                        Placements = 1,
                        IsSaved = true,
                        IsSelectedForReferral = false,
                        IsCompleted = false,
                        RouteId = 1,
                        CreatedOn = new(2019, 1, 2),
                        CreatedBy = "Dev Surname",
                        Referral = new List<Referral>
                        {
                            new()
                            {
                                Id = ReferralMultiple1Id,
                                ProviderVenueId = 1,
                                DistanceFromEmployer = 1.23m,
                                CreatedOn = new(2019, 1, 2),
                                CreatedBy = "Dev Surname",
                            },
                            new()
                            {
                                Id = ReferralMultiple3Id,
                                ProviderVenueId = 2,
                                DistanceFromEmployer = 2.5m,
                                CreatedOn = new(2019, 1, 2),
                                CreatedBy = "Dev Surname",
                            }
                        }
                    },
                    new()
                    {
                        Id = OpportunityItemReferralMultiple2Id,
                        OpportunityId = OpportunityReferralMultipleId,
                        OpportunityType = OpportunityType.Referral.ToString(),
                        Town = "London",
                        Postcode = "SW2A 3AA",
                        SearchRadius = 25,
                        SearchResultProviderCount = 1,
                        JobRole = "Job Role",
                        PlacementsKnown = true,
                        Placements = 1,
                        IsSaved = true,
                        IsSelectedForReferral = false,
                        IsCompleted = false,
                        RouteId = 1,
                        CreatedOn = new(2019, 1, 3),
                        CreatedBy = "Dev Surname",
                        Referral = new List<Referral>
                        {
                            new()
                            {
                                Id = ReferralMultiple2Id,
                                ProviderVenueId = 1,
                                DistanceFromEmployer = 1.23m,
                                CreatedOn = new(2019, 1, 3),
                                CreatedBy = "Dev Surname",
                            }
                        }
                    }
                }
            };
        }

        internal static Opportunity CreateProvisionGapSingle()
        {
            return new()
            {
                Id = OpportunityProvisionGapSingleId,
                EmployerCrmId = new Guid("11111111-1111-1111-1111-111111111111"),
                EmployerContact = "Employer Contact",
                EmployerContactEmail = "employer-contact@email.com",
                EmployerContactPhone = "01474 787878",
                CreatedOn = new(2019, 1, 4),
                CreatedBy = "Dev Surname",
                OpportunityItem = new List<OpportunityItem>
                {
                    new()
                    {
                        Id = OpportunityItemProvisionGapSingleId,
                        OpportunityId = OpportunityProvisionGapSingleId,
                        OpportunityType = OpportunityType.ProvisionGap.ToString(),
                        Town = "London",
                        Postcode = "SW1A 2AA",
                        SearchRadius = 25,
                        SearchResultProviderCount = 1,
                        JobRole = "Job Role",
                        PlacementsKnown = true,
                        Placements = 1,
                        IsSaved = true,
                        IsSelectedForReferral = false,
                        IsCompleted = false,
                        RouteId = 1,
                        CreatedOn = new(2019, 1, 4),
                        CreatedBy = "Dev Surname",
                        ProvisionGap = new List<ProvisionGap>
                        {
                            new()
                            {
                                Id = ProvisionGapSingleId,
                                HadBadExperience = true,
                                CreatedOn = new(2019, 1, 4),
                                CreatedBy = "Dev Surname",
                            }
                        }
                    }
                }
            };
        }

        internal static Opportunity CreateReferralSingleAndProvisionGap()
        {
            return new()
            {
                Id = OpportunityReferralSingleAndProvisionGapId,
                EmployerCrmId = new Guid("11111111-1111-1111-1111-111111111111"),
                EmployerContact = "Employer Contact",
                EmployerContactEmail = "employer-contact@email.com",
                EmployerContactPhone = "01474 787878",
                CreatedOn = new(2018, 11, 5, 16, 22, 11),
                CreatedBy = "Dev Surname",
                OpportunityItem = new List<OpportunityItem>
                {
                    new()
                    {
                        Id = OpportunityItemReferralSingleAndProvisionGap1Id,
                        OpportunityId = OpportunityReferralSingleAndProvisionGapId,
                        OpportunityType = OpportunityType.Referral.ToString(),
                        Town = "London",
                        Postcode = "SW1A 2AA",
                        SearchRadius = 25,
                        SearchResultProviderCount = 1,
                        JobRole = "Job Role",
                        PlacementsKnown = true,
                        Placements = 1,
                        IsSaved = true,
                        IsSelectedForReferral = false,
                        IsCompleted = false,
                        RouteId = 1,
                        CreatedOn = new(2018, 11, 5, 16, 22, 11),
                        CreatedBy = "Dev Surname",
                        Referral = new List<Referral>
                        {
                            new()
                            {
                                Id = ReferralSingle1Id,
                                ProviderVenueId = 1,
                                DistanceFromEmployer = 1.23m,
                                CreatedOn = new(2018, 11, 5, 16, 22, 11),
                                CreatedBy = "Dev Surname",
                            }
                        }
                    },
                    new()
                    {
                        Id = OpportunityItemReferralSingleAndProvisionGap2Id,
                        OpportunityId = OpportunityReferralSingleAndProvisionGapId,
                        OpportunityType = OpportunityType.ProvisionGap.ToString(),
                        Town = "London",
                        Postcode = "SW1A 2AA",
                        SearchRadius = 25,
                        SearchResultProviderCount = 1,
                        JobRole = "Job Role",
                        PlacementsKnown = true,
                        Placements = 1,
                        IsSaved = true,
                        IsSelectedForReferral = false,
                        IsCompleted = false,
                        RouteId = 1,
                        CreatedOn = new(2019, 1, 6),
                        CreatedBy = "Dev Surname",
                        ProvisionGap = new List<ProvisionGap>
                        {
                            new()
                            {
                                Id = ProvisionGapSingle1Id,
                                ProvidersTooFarAway = true,
                                CreatedOn = new(2019, 1, 6),
                                CreatedBy = "Dev Surname",
                            }
                        }
                    }
                }
            };
        }

        internal static Opportunity CreateReferralMultipleAndProvisionGap()
        {
            return new()
            {
                Id = OpportunityMultipleReferralAndProvisionGapId,
                EmployerCrmId = new Guid("11111111-1111-1111-1111-111111111111"),
                EmployerContact = "Employer Contact",
                EmployerContactEmail = "employer-contact@email.com",
                EmployerContactPhone = "01474 787878",
                CreatedOn = new(2019, 4, 7, 22, 59, 0),
                CreatedBy = "Dev Surname",
                OpportunityItem = new List<OpportunityItem>
                {
                    new()
                    {
                        Id = OpportunityItemMultipleReferralAndProvisionGap1Id,
                        OpportunityId = OpportunityMultipleReferralAndProvisionGapId,
                        OpportunityType = OpportunityType.Referral.ToString(),
                        Town = "London",
                        Postcode = "SW1A 2AA",
                        SearchRadius = 25,
                        SearchResultProviderCount = 1,
                        JobRole = "Job Role",
                        PlacementsKnown = true,
                        Placements = 1,
                        IsSaved = true,
                        IsSelectedForReferral = false,
                        IsCompleted = false,
                        RouteId = 1,
                        CreatedOn = new(2019, 4, 7, 22, 59, 0),
                        CreatedBy = "Dev Surname",
                        Referral = new List<Referral>
                        {
                            new()
                            {
                                Id = ReferralAndProvisionGapMultiple1Id,
                                ProviderVenueId = 1,
                                DistanceFromEmployer = 1.23m,
                                CreatedOn = new(2019, 4, 7, 22, 59, 0),
                                CreatedBy = "Dev Surname",
                            }
                        }
                    },
                    new()
                    {
                        Id = OpportunityItemMultipleReferralAndProvisionGap2Id,
                        OpportunityId = OpportunityMultipleReferralAndProvisionGapId,
                        OpportunityType = OpportunityType.Referral.ToString(),
                        Town = "London",
                        Postcode = "SW2A 3AA",
                        SearchRadius = 25,
                        SearchResultProviderCount = 1,
                        JobRole = "Job Role",
                        PlacementsKnown = true,
                        Placements = 1,
                        IsSaved = true,
                        IsSelectedForReferral = false,
                        IsCompleted = false,
                        RouteId = 1,
                        CreatedOn = new(2019, 1, 8),
                        CreatedBy = "Dev Surname",
                        Referral = new List<Referral>
                        {
                            new()
                            {
                                Id = ReferralAndProvisionGapMultiple2Id,
                                ProviderVenueId = 1,
                                DistanceFromEmployer = 1.23m,
                                CreatedOn = new(2019, 1, 8),
                                CreatedBy = "Dev Surname",
                            }
                        }
                    },
                    new()
                    {
                        Id = OpportunityItemMultipleReferralAndProvisionGap3Id,
                        OpportunityId = OpportunityMultipleReferralAndProvisionGapId,
                        OpportunityType = OpportunityType.ProvisionGap.ToString(),
                        Town = "London",
                        Postcode = "SW1A 2AA",
                        SearchRadius = 25,
                        SearchResultProviderCount = 1,
                        JobRole = "Job Role",
                        PlacementsKnown = true,
                        Placements = 1,
                        IsSaved = true,
                        IsSelectedForReferral = false,
                        IsCompleted = false,
                        RouteId = 1,
                        CreatedOn = new(2019, 1, 9),
                        CreatedBy = "Dev Surname",
                        ProvisionGap = new List<ProvisionGap>
                        {
                            new()
                            {
                                Id = ProvisionGapMultipleReferralAndProvisionGap1Id,
                                HadBadExperience = true,
                                CreatedOn = new(2019, 1, 9),
                                CreatedBy = "Dev Surname",
                            }
                        }
                    }
                }
            };
        }

        internal static IList<Opportunity> Create()
        {
            return new List<Opportunity>
            {
                new()
                {
                    Id = Opportunity1Id,
                    EmployerCrmId = new Guid("11111111-1111-1111-1111-111111111111"),
                    EmployerContact = "Employer Contact",
                    EmployerContactEmail = "employer-contact@email.com",
                    EmployerContactPhone = "01474 787878",
                    CreatedOn = new(2019, 1, 10),
                    CreatedBy = "Dev Surname",
                    OpportunityItem = new List<OpportunityItem>
                    {
                        new()
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
                            CreatedOn = new(2019, 1, 10),
                            CreatedBy = "Dev Surname",
                            Referral = new List<Referral>
                            {
                                new()
                                {
                                    Id = Referral1Id,
                                    ProviderVenueId = 1,
                                    DistanceFromEmployer = 1.23m,
                                    CreatedOn = new(2019, 1, 10),
                                    CreatedBy = "Dev Surname",
                                }
                            }
                        },
                        new()
                        {
                            Id = OpportunityItem2Id,
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
                            CreatedOn = new(2019, 1, 11),
                            CreatedBy = "Dev Surname",
                            Referral = new List<Referral>
                            {
                                new()
                                {
                                    Id = Referral2Id,
                                    ProviderVenueId = 1,
                                    DistanceFromEmployer = 1.23m,
                                    CreatedOn = new(2019, 1, 11),
                                    CreatedBy = "Dev Surname",
                                }
                            }
                        }
                    }
                }
            };
        }

        internal static Opportunity CreateProvidersMultiple()
        {
            return new()
            {
                Id = OpportunityProviderMultipleId,
                EmployerCrmId = new Guid("11111111-1111-1111-1111-111111111111"),
                EmployerContact = "Employer Contact",
                EmployerContactEmail = "employer-contact@email.com",
                EmployerContactPhone = "01474 787878",
                CreatedOn = new(2019, 1, 12),
                CreatedBy = "Dev Surname",
                OpportunityItem = new List<OpportunityItem>
                {
                    new()
                    {
                        Id = OpportunityItemProviderMultipleId,
                        OpportunityId = OpportunityProviderMultipleId,
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
                        CreatedOn = new(2019, 1, 12),
                        CreatedBy = "Dev Surname",
                        Referral = new List<Referral>
                        {
                            new()
                            {
                                Id = ProviderReferral1Id,
                                ProviderVenueId = 1,
                                DistanceFromEmployer = 1.23m,
                                CreatedOn = new(2019, 1, 12),
                                CreatedBy = "Dev Surname",
                            },
                            new()
                            {
                                Id = ProviderReferral2Id,
                                ProviderVenueId = 2,
                                DistanceFromEmployer = 2.93m,
                                CreatedOn = new(2019, 1, 12),
                                CreatedBy = "Dev Surname",
                            }
                        }
                    }
                }
            };
        }

        internal static Opportunity CreateNReferrals(int nItems)
        {
            const int opportunityId = 3000;
            var opportunityItemId = 4000;
            var referralId = 5000;

            var items = new List<OpportunityItem>();

            for (var nItem = 1; nItem <= nItems; nItem++)
            {
                var item = new OpportunityItem
                {
                    Id = opportunityItemId,
                    OpportunityId = opportunityId,
                    OpportunityType = OpportunityType.Referral.ToString(),
                    Town = "London",
                    Postcode = "SW1A 2AA",
                    SearchResultProviderCount = 1,
                    JobRole = "Job Role",
                    PlacementsKnown = true,
                    Placements = 1,
                    IsSaved = true,
                    IsSelectedForReferral = false,
                    IsCompleted = false,
                    RouteId = 1,
                    CreatedOn = new(2019, 1, 2),
                    CreatedBy = "Dev Surname",
                    Referral = new List<Referral>
                    {
                        new()
                        {
                            Id = referralId,
                            ProviderVenueId = 1,
                            DistanceFromEmployer = 1.23m,
                            CreatedOn = new(2019, 1, 2),
                            CreatedBy = "Dev Surname",
                        }
                    }
                };

                referralId += 1;
                opportunityItemId += 1;

                items.Add(item);
            }

            var opportunity = new Opportunity
            {
                Id = opportunityId,
                EmployerCrmId = new Guid("11111111-1111-1111-1111-111111111111"),
                EmployerContact = "Employer Contact",
                EmployerContactEmail = "employer-contact@email.com",
                EmployerContactPhone = "01474 787878",
                CreatedOn = new(2019, 1, 2),
                CreatedBy = "Dev Surname",
                OpportunityItem = items
            };

            return opportunity;
        }

    }
}