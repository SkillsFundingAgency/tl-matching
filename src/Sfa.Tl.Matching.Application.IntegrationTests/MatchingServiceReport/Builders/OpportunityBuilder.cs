using System;
using System.Collections.Generic;
using System.Linq;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.Builders
{
    public class OpportunityBuilder
    {
        private readonly MatchingDbContext _context;

        public OpportunityBuilder(MatchingDbContext context)
        {
            _context = context;
        }

        public Domain.Models.Opportunity CreateOpportunity(Guid employerCrmId, List<OpportunityItem> opportunityItems)
        {
            var opportunity = new Domain.Models.Opportunity
            {
                EmployerCrmId = employerCrmId,
                EmployerContact = "test",
                EmployerContactEmail = "test@test.com",
                EmployerContactPhone = "01234567890",
                OpportunityItem = opportunityItems,
                CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
            };

            _context.Add(opportunity);

            _context.SaveChanges();

            return opportunity;
        }

        public OpportunityItem CreaeReferralOpportunityItem(bool isSaved, bool isCompleted, params int[] providerVenueId)
        {
            return new OpportunityItem
            {
                Town = "test",
                JobRole = "test",
                Postcode = "test",

                Placements = 1,
                PlacementsKnown = true,

                IsSaved = isSaved,
                IsCompleted = isCompleted,
                IsSelectedForReferral = true,
                OpportunityType = OpportunityType.Referral.ToString(),

                RouteId = 1,
                SearchResultProviderCount = 1,
                CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                Referral = providerVenueId.Select(pv => new Referral
                {
                    DistanceFromEmployer = 1.1m,
                    JourneyTimeByCar = 1,
                    JourneyTimeByPublicTransport = 1,
                    ProviderVenueId = pv,
                    CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests"
                }).ToList()
            };
        }

        public OpportunityItem CreateProvisionGapOpportunityItem(bool isSaved, bool isCompleted)
        {
            return new OpportunityItem
            {
                Town = "test",
                JobRole = "test",
                Postcode = "test",

                Placements = 1,
                PlacementsKnown = true,

                IsSaved = isSaved,
                IsCompleted = isCompleted,
                IsSelectedForReferral = true,
                OpportunityType = OpportunityType.ProvisionGap.ToString(),

                RouteId = 1,
                SearchResultProviderCount = 1,
                CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                ProvisionGap = new List<ProvisionGap>
                {
                    new ProvisionGap
                    {
                        HadBadExperience = false,
                        NoSuitableStudent = false,
                        ProvidersTooFarAway = false,
                        CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                    }
                }
            };
        }

        public void ClearData()
        {
            _context.Referral.Remove(_context.Referral.First(r => r.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests"));

            _context.ProvisionGap.Remove(_context.ProvisionGap.First(r => r.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests"));

            _context.OpportunityItem.RemoveRange(_context.OpportunityItem.Where(o => o.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests"));

            _context.Opportunity.Remove(_context.Opportunity.First(o => o.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests"));

            _context.SaveChanges();
        }
    }

    public class ProviderBuilder
    {
        private readonly MatchingDbContext _context;

        public ProviderBuilder(MatchingDbContext context)
        {
            _context = context;
        }

        public Provider CreaeProvider(int venueCount = 1)
        {
            var provider = new Provider
            {
                DisplayName = "test",
                Name = "test",
                OfstedRating = 1,
                PrimaryContact = "test",
                PrimaryContactEmail = "test@test.com",
                PrimaryContactPhone = "01234567890",
                SecondaryContact = "test",
                SecondaryContactEmail = "test@test.com",
                SecondaryContactPhone = "01234567890",
                Source = "test",
                UkPrn = 12345678,
                IsCdfProvider = true,
                IsEnabledForReferral = true,
                IsTLevelProvider = true,
                CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                ProviderVenue = Enumerable.Range(1, venueCount).Select(venue =>
                    new ProviderVenue
                    {
                        IsRemoved = true,
                        IsEnabledForReferral = true,
                        County = "test",
                        Name = "test" + venue,
                        Postcode = "test",
                        Source = "test",
                        Town = "test",
                        CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                        ProviderQualification = new List<ProviderQualification>
                        {
                            new ProviderQualification
                            {
                                CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                                NumberOfPlacements = 1,
                                Source = "test",
                                Qualification = new Qualification
                                {
                                    CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                                    LarsId = "12345678",
                                    ShortTitle = "test",
                                    Title = "test",
                                    QualificationRouteMapping = new List<QualificationRouteMapping>
                                    {
                                        new QualificationRouteMapping
                                        {
                                            RouteId = 1,
                                            CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                                            Source = "test",
                                        }
                                    }
                                }
                            }
                        }
                    }).ToList()
            };

            _context.Add(provider);

            _context.SaveChanges();

            return provider;
        }

        public void ClearData()
        {
            _context.QualificationRouteMapping.Remove(_context.QualificationRouteMapping.First(q => q.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests"));

            _context.Qualification.Remove(_context.Qualification.First(q => q.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests"));

            _context.ProviderQualification.Remove(_context.ProviderQualification.First(q => q.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests"));

            _context.ProviderVenue.Remove(_context.ProviderVenue.First(q => q.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests"));

            _context.Provider.Remove(_context.Provider.First(q => q.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests"));
        }
    }

    public class EmployerBuilder
    {
        private readonly MatchingDbContext _context;

        public EmployerBuilder(MatchingDbContext context)
        {
            _context = context;
        }

        public Domain.Models.Employer CreaeEmployer(Guid crmId)
        {
            var employer = new Domain.Models.Employer
            {
                Postcode = "test",
                AlsoKnownAs = "test",
                Aupa = "test",
                CompanyName = "test",
                CompanyType = "test",
                CrmId = Guid.NewGuid(),
                Email = "test@test.com",
                Owner = "test",
                Phone = "01234567890",
                PrimaryContact = "test",
                CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
            };

            _context.Add(employer);
            _context.SaveChanges();

            return employer;
        }

        public void ClearData()
        {
            _context.Employer.Remove(_context.Employer.First(e => e.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests"));
        }
    }
}