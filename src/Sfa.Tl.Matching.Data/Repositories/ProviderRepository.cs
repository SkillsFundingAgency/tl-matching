using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class ProviderRepository : GenericRepository<Provider>, IProviderRepository
    {
        public ProviderRepository(ILogger<ProviderRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
        }

        public async Task<IList<ProviderWithFundingDto>> GetProvidersWithFundingAsync()
        {
            return await GetMany(p => p.IsCdfProvider)
                .Select(provider => new ProviderWithFundingDto
                {
                    Id = provider.Id,
                    Name = provider.Name,
                    PrimaryContact = provider.PrimaryContact,
                    PrimaryContactPhone = provider.PrimaryContactPhone,
                    PrimaryContactEmail = provider.PrimaryContactEmail,
                    SecondaryContact = provider.SecondaryContact,
                    SecondaryContactPhone = provider.SecondaryContactPhone,
                    SecondaryContactEmail = provider.SecondaryContactEmail,
                    CreatedBy = provider.CreatedBy,
                    ProviderVenues = provider.ProviderVenue.Select(venue => new ProviderVenueQualificationsInfoDto
                    {
                        Postcode = venue.Postcode,
                        Qualifications = venue.ProviderQualification
                            //.OrderBy(q => new { q.Qualification.Title, q.Qualification.LarsId })
                            .Select(qualification => new QualificationInfoDto
                            {
                                LarsId = qualification.Qualification.LarsId,
                                Title = qualification.Qualification.Title
                            }).Distinct().ToList()
                    }).OrderBy(v => v.Postcode).ToList()
                })
                .OrderBy(p => p.Id)
                .ToListAsync();
        }
    }
}
