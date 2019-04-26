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
                //TODO: Join and get venues and qualifications
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
                    CreatedBy = p.CreatedBy
                }).ToListAsync();
        }
    }
}
