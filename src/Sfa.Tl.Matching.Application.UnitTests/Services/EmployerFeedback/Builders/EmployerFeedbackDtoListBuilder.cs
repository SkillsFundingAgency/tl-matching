using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.EmployerFeedback.Builders
{
    public class EmployerFeedbackDtoListBuilder
    {
        public IList<EmployerFeedbackDto> BuildSingleEmployer() => new List<EmployerFeedbackDto>
        {
            new EmployerFeedbackDto
            {
                OpportunityItemId = 1,
                EmployerContact = "Employer Contact",
                EmployerContactEmail = "employer.contact@employer.co.uk",
                EmployerCrmId = new Guid("11111111-1111-1111-1111-111111111111"),
                Placements = 1,
                Postcode = "CV1 2WT",
                Route = "Route",
                Town = "Town",
                ModifiedOn = new DateTime(2019, 12, 12)
            },
            new EmployerFeedbackDto
            {
                OpportunityItemId = 2,
                EmployerContact = "Employer Contact",
                EmployerContactEmail = "employer.contact@employer.co.uk",
                EmployerCrmId = new Guid("11111111-1111-1111-1111-111111111111"),
                JobRole = "Job Role",
                Placements = 3,
                Postcode = "CV2 3WT",
                Route = "Another Route",
                Town = "Another Town",
                ModifiedOn = new DateTime(2019, 12, 1)
            }
        };
    }
}