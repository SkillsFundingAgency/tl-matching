using System.Linq;
using Castle.Core.Internal;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.Builders
{
    public class PostcodeLookupBuilder
    {
        private readonly MatchingDbContext _context;

        public PostcodeLookupBuilder(MatchingDbContext context)
        {
            _context = context;
        }

        public PostcodeLookup CreatePostcodeLookup()
        {
            var postcode = new PostcodeLookup
            {
                Postcode = "PO1 TST",
                LepCode = "LEP000001",
                CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
            };

            _context.Add(postcode);

            _context.SaveChanges();

            return postcode;
        }

        public void ClearData()
        {
            var postcode = _context.PostcodeLookup.Where(q => q.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests").ToList();
            if (!postcode.IsNullOrEmpty()) _context.PostcodeLookup.RemoveRange(postcode);

            _context.SaveChanges();
        }
    }
}