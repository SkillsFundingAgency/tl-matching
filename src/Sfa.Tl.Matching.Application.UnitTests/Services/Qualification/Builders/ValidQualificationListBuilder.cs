using System.Collections.Generic;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Qualification.Builders
{
    public class ValidQualificationListBuilder
    {
        public IList<Domain.Models.Qualification> Build() => new List<Domain.Models.Qualification>
        {
            new Domain.Models.Qualification
            {
                Id = 1,
                LarsId = "10042982",
                Title = "Level 2 Diploma in Sport and Enterprise in the Community",
                ShortTitle = "sport and enterprise in the community",
                QualificationSearch = "SportEnterpriseCommunitysportenterprisecommunity",
                ShortTitleSearch = "sportenterprisecommunity"
            },
            new Domain.Models.Qualification
            {
                Id = 2,
                LarsId = "10017884",
                Title = "Level 3 Diploma in Sport and Enterprise in the Community",
                ShortTitle = "sport and enterprise in the community",
                QualificationSearch = "SportEnterpriseCommunitysportenterprisecommunity",
                ShortTitleSearch = "sportenterprisecommunity"
            },
            new Domain.Models.Qualification
            {
                Id = 3,
                LarsId = "60163239",
                Title = "Level 2 in Applied Scientific Reasoning",
                ShortTitle = "applied science and technology",
                QualificationSearch = "appliedscientificreasoningappliedsciencetechnology",
                ShortTitleSearch = "appliedsciencetechnology"
            }
        };
    }
}
