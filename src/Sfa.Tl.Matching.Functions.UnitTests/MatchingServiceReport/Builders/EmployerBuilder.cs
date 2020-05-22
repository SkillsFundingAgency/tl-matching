using System;
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Functions.UnitTests.MatchingServiceReport.Builders
{
    public class EmployerBuilder
    {
        public IList<Domain.Models.Employer> BuildList() => new List<Domain.Models.Employer>
        {
            new Domain.Models.Employer
            {
                Id = 1,
                CrmId = Guid.Parse("2007979A-D731-4944-8A63-CD5238DC81A8"),
                CompanyName = "Company",
                AlsoKnownAs = "Also Known As",
                CompanyNameSearch = "CompanyAlsoKnownAs",
                Aupa = "Aware",
                PrimaryContact = "Employer Contact",
                Phone = "020 123 4567",
                Email = "employer.contact@employer.co.uk",
                Owner = "Owner",
                CreatedBy = "Sfa.Tl.Matching.Functions.UnitTests",
                CreatedOn = DateTime.UtcNow
            },
            new Domain.Models.Employer
            {
                Id = 2,
                CrmId = Guid.Parse("838E711C-1BA0-4106-B3C1-56EE8AC99E8A"),
                CompanyName = "Company Two",
                AlsoKnownAs = "Also Known As Two",
                CompanyNameSearch = "CompanyTwoAlsoKnownAsTwo",
                Aupa = "Active",
                PrimaryContact = "Employer Two Contact",
                Phone = "020 123 2222",
                Email = "employer.two.contact@employer.co.uk",
                Owner = "Owner",
                CreatedBy = "Sfa.Tl.Matching.Functions.UnitTests",
                CreatedOn = DateTime.UtcNow
            }
        };
    }
}
