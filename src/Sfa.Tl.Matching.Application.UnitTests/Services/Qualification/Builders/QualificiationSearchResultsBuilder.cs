using System.Collections.Generic;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Qualification.Builders
{
    public class QualificiationSearchResultsBuilder
    {
        private readonly IEnumerable<Domain.Models.Qualification> _searchResults;

        public QualificiationSearchResultsBuilder()
        {
            _searchResults = new List<Domain.Models.Qualification>
            {
                new Domain.Models.Qualification
                {
                    Id = 1,
                    ShortTitle = "sport and enterprise in the community"
                },
                new Domain.Models.Qualification
                {
                    Id = 2,
                    ShortTitle = "sport and enterprise in the community"
                }
            };
        }

        public IEnumerable<Domain.Models.Qualification> Build() =>
            _searchResults;
    }
}