using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.EmployerFeedback.Builders
{
    public class EmployerFeedbackDtoListBuilder
    {
        public IList<EmployerFeedbackDto> Build() => new List<EmployerFeedbackDto>
        {
            new EmployerFeedbackDto
            {
                OpportunityId = 1,
                OpportunityItemId = 2,
                EmployerContact = "Employer Contact",
                EmployerContactEmail = "primary.contact@employer.co.uk"
            }
        };
    }
}
