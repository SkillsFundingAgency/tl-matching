using System;
using System.Linq;
using Castle.Core.Internal;
using Sfa.Tl.Matching.Data;

namespace Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.Builders
{
    public class EmployerBuilder
    {
        private readonly MatchingDbContext _context;

        public EmployerBuilder(MatchingDbContext context)
        {
            _context = context;
        }

        public Domain.Models.Employer CreateEmployer(Guid crmId)
        {
            var employer = new Domain.Models.Employer
            {
                AlsoKnownAs = "test aka",
                Aupa = "test aupa",
                CompanyName = "test company name",
                CrmId = Guid.NewGuid(),
                Email = "test@test.com",
                Owner = "test owner",
                Phone = "01234567890",
                PrimaryContact = "test contact",
                CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
            };

            _context.Add(employer);
            _context.SaveChanges();

            return employer;
        }

        public void ClearData()
        {
            var employer = _context.Employer.Where(e => e.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests").ToList();
            if(!employer.IsNullOrEmpty()) _context.Employer.RemoveRange(employer);

            _context.SaveChanges();
        }
    }
}