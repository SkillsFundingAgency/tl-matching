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
        private readonly MatchingDbContext _dbContext;

        public ProviderRepository(ILogger<ProviderRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<ProviderWithFundingDto>> GetProvidersWithFundingAsync()
        {
            return await (from p in _dbContext.Provider
                          orderby p.Id
                          where p.IsFundedForNextYear
                          select new ProviderWithFundingDto
                          {
                              Id = p.Id,
                              Name = p.Name,
                              PrimaryContact = p.PrimaryContact,
                              PrimaryContactEmail = p.PrimaryContactEmail,
                              PrimaryContactPhone = p.PrimaryContactPhone,
                              SecondaryContact = p.SecondaryContact,
                              SecondaryContactEmail = p.SecondaryContactEmail,
                              SecondaryContactPhone = p.SecondaryContactPhone,
                              CreatedBy = p.CreatedBy,
                              ProviderVenues =
                                  (from pv in _dbContext.ProviderVenue
                                   where pv.ProviderId == p.Id
                                   orderby pv.Postcode
                                   select new ProviderVenueQualificationsInfoDto
                                   {
                                       Postcode = pv.Postcode,
                                       Qualifications =
                                           (from pq in _dbContext.ProviderQualification
                                            join q in _dbContext.Qualification on pq.QualificationId equals q.Id
                                            where pv.Id == pq.ProviderVenueId
                                            orderby q.ShortTitle, q.LarsId
                                            select new QualificationInfoDto
                                            {
                                                LarsId = q.LarsId,
                                                ShortTitle = q.ShortTitle
                                            }).Distinct().ToList()
                                   }).ToList()
                          }).ToListAsync();
        }
    }
}
