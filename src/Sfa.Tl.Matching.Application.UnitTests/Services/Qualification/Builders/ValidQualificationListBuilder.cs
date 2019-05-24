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
                Title = "Qualification 1",
                ShortTitle = "Short Title 1",
                CreatedBy = "CreatedBy",
                ModifiedBy = "ModifiedBy"
            },
            new Domain.Models.Qualification
            {
                Id = 2,
                LarsId = "60165522",
                Title = "Qualification 2",
                ShortTitle = "Short Title 2",
                CreatedBy = "CreatedBy",
                ModifiedBy = "ModifiedBy"
            }
        };
    }
}
