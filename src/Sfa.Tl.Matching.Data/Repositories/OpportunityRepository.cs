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
    public class OpportunityRepository : GenericRepository<Opportunity>,
                                         IOpportunityRepository
    {
        private readonly ILogger<OpportunityRepository> _logger;
        private readonly MatchingDbContext _dbContext;

        public OpportunityRepository(ILogger<OpportunityRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IList<OpportunityReferralDto>> GetProviderOpportunities(int opportunityId)
        {
            return await (from op in _dbContext.Opportunity
                          join re in _dbContext.Referral on op.Id equals re.OpportunityId
                          join pv in _dbContext.ProviderVenue on re.ProviderVenueId equals pv.Id
                          join p in _dbContext.Provider on pv.ProviderId equals p.Id
                          join r in _dbContext.Route on op.RouteId equals r.Id
                          where op.Id == opportunityId
                          select new OpportunityReferralDto
                          {
                              OpportunityId = op.Id,
                              ReferralId = re.Id,
                              ProviderName = p.Name,
                              ProviderPrimaryContact = p.PrimaryContact,
                              ProviderPrimaryContactEmail = p.PrimaryContactEmail,
                              ProviderSecondaryContactEmail = p.SecondaryContactEmail,
                              EmployerName = op.EmployerName,
                              EmployerContact = op.EmployerContact,
                              EmployerContactPhone = op.EmployerContactPhone,
                              EmployerContactEmail = op.EmployerContactEmail,
                              SearchRadius = op.Distance,
                              Postcode = op.PostCode,
                              JobTitle = op.JobTitle,
                              ProviderVenuePostcode = pv.Postcode,
                              PlacementsKnown = op.PlacementsKnown,
                              Placements = op.Placements,
                              RouteName = r.Name
                          }).ToListAsync();
        }
    }
}