using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class ProviderVenueRepository : GenericRepository<ProviderVenue>, IProviderVenueRepository
    {
        private readonly MatchingDbContext _dbContext;

        public ProviderVenueRepository(ILogger<ProviderVenueRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProviderVenueDetailViewModel> GetVenueWithQualifications(string postcode)
        {
            var venueWithQualifications = await (from pv in _dbContext.ProviderVenue
                             where pv.Postcode == postcode
                             select new ProviderVenueDetailViewModel
                             {
                                 Id = pv.Id,
                                 ProviderId = pv.ProviderId,
                                 VenueName = pv.Name,
                                 Qualifications = (from q in _dbContext.Qualification
                                                   join pq in _dbContext.ProviderQualification on q.Id equals pq.QualificationId
                                                   join pv1 in _dbContext.ProviderVenue on pq.ProviderVenueId equals pv1.Id
                                                   where pv1.Postcode == postcode
                                                   select new QualificationDetailViewModel
                                                   {
                                                       LarsId = q.LarsId
                                                   }).Distinct().ToList()
                             }).SingleOrDefaultAsync();

            return venueWithQualifications;
        }
    }
}