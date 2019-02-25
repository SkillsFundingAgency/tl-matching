using System.Collections.Generic;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer.Builders
{
    public class SearchResultsBuilder
    {
        private readonly IEnumerable<Domain.Models.Employer> _searchResults;

        public SearchResultsBuilder()
        {
            _searchResults = new List<Domain.Models.Employer>
            {
                new Domain.Models.Employer
                {
                    Id = 1,
                    CompanyName = "Z Company",
                    AlsoKnownAs = "Z Also Known As"
                },
                new Domain.Models.Employer
                {
                    Id = 2,
                    CompanyName = "Company",
                    AlsoKnownAs = "Also Known As"
                },
                new Domain.Models.Employer
                {
                    Id = 3,
                    CompanyName = "Another Company",
                    AlsoKnownAs = "Another Also Known As"
                }
            };
        }

        public IEnumerable<Domain.Models.Employer> Build() =>
            _searchResults;
    }
}