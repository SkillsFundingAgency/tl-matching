using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.Builders
{
    public class ProviderBuilder
    {
        private readonly MatchingDbContext _context;

        public ProviderBuilder(MatchingDbContext context)
        {
            _context = context;
        }

        public Provider CreateProvider(int venueCount = 1)
        {
            var provider = new Provider
            {
                DisplayName = "test provider display name",
                Name = "test provider",
                OfstedRating = 1,
                PrimaryContact = "test primary contact",
                PrimaryContactEmail = "testprimary@test.com",
                PrimaryContactPhone = "01234567890",
                SecondaryContact = "test secondary contact",
                SecondaryContactEmail = "testsecondary@test.com",
                SecondaryContactPhone = "01234567891",
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
                        County = $"test county{venue}",
                        Name = $"test name{venue}",
                        Postcode = $"POST PO{venue}",
                        Source = $"test{venue}",
                        Town = $"test town{venue}",
                        CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                        ProviderQualification = new List<ProviderQualification>
                        {
                            new ProviderQualification
                            {
                                CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                                Source = "test",
                                Qualification = new Qualification
                                {
                                    CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                                    LarId = "12345678",
                                    ShortTitle = "test short title",
                                    Title = "test title",
                                    QualificationRouteMapping = new List<QualificationRouteMapping>
                                    {
                                        new QualificationRouteMapping
                                        {
                                            RouteId = 1,
                                            CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                                            Source = "test"
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
            var qrm = _context.QualificationRouteMapping.Where(q => q.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests").ToList();
            if (!qrm.IsNullOrEmpty()) _context.QualificationRouteMapping.RemoveRange(qrm);

            var qual = _context.Qualification.Where(q => q.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests").ToList();
            if (!qual.IsNullOrEmpty()) _context.Qualification.RemoveRange(qual);

            var pq = _context.ProviderQualification.Where(q => q.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests").ToList();
            if (!pq.IsNullOrEmpty()) _context.ProviderQualification.RemoveRange(pq);

            var pv = _context.ProviderVenue.Where(q => q.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests").ToList();
            if (!pv.IsNullOrEmpty()) _context.ProviderVenue.RemoveRange(pv);

            var p = _context.Provider.Where(q => q.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests").ToList();
            if (!p.IsNullOrEmpty()) _context.Provider.RemoveRange(p);

            _context.SaveChanges();
        }
    }
}