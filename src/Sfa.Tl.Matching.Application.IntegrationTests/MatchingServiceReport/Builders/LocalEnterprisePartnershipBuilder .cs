using System.Linq;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Data;

namespace Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.Builders
{
    public class LocalEnterprisePartnershipBuilder
    {
        private readonly MatchingDbContext _context;

        public LocalEnterprisePartnershipBuilder(MatchingDbContext context)
        {
            _context = context;
        }

        public Domain.Models.LocalEnterprisePartnership CreateLocalEnterprisePartnership()
        {
            var lep = new Domain.Models.LocalEnterprisePartnership
            {
                Code = "LEP000001",
                Name = "LEP Name",
                CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests"
            };

            _context.Add(lep);

            _context.SaveChanges();

            return lep;
        }

        public void ClearData()
        {
            var lep = _context.LocalEnterprisePartnership.Where(q => q.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests").ToList();
            if (!lep.IsNullOrEmpty()) _context.LocalEnterprisePartnership.RemoveRange(lep);

            _context.SaveChanges();
        }
    }
}