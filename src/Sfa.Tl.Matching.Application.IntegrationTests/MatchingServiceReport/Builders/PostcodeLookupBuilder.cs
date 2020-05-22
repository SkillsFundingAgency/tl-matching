using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Sfa.Tl.Matching.Data;

namespace Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.Builders
{
    public class PostcodeLookupBuilder
    {
        private readonly MatchingDbContext _context;

        public PostcodeLookupBuilder(MatchingDbContext context)
        {
            _context = context;
        }

        public IList<Domain.Models.PostcodeLookup> CreatePostcodeLookup(int postcodeCount = 1)
        {
            var postcodes = new List<Domain.Models.PostcodeLookup>();

            _context.AddRange(
                Enumerable.Range(1, postcodeCount).Select(index =>
                    new Domain.Models.PostcodeLookup
                    {
                        Postcode = $"POST PO{index}",
                        PrimaryLepCode = "LEP000001",
                        SecondaryLepCode = "LEP000002",
                        CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests"
                    }));

            _context.AddRange(postcodes);
            _context.SaveChanges();

            return postcodes;
        }

        public void ClearData()
        {
            var postcode = _context.PostcodeLookup.Where(q => q.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests").ToList();
            if (!postcode.IsNullOrEmpty()) _context.PostcodeLookup.RemoveRange(postcode);

            _context.SaveChanges();
        }
    }
}