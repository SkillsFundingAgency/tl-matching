using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.EmployerFeedback.Builders
{
    public class EmployerFeedbackDtoListBuilder
    {
        private readonly List<EmployerFeedbackDto> _employerFeedbackDtos;

        public EmployerFeedbackDtoListBuilder()
        {
            _employerFeedbackDtos = new List<EmployerFeedbackDto>
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
                    EmployerContact = "Old Employer Contact",
                    EmployerContactEmail = "old.employer.contact@employer.co.uk",
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

        public EmployerFeedbackDtoListBuilder AddAnotherEmployer()
        {
            _employerFeedbackDtos.Add(new EmployerFeedbackDto
            {
                OpportunityItemId = 3,
                EmployerContact = "Another Employer Contact",
                EmployerContactEmail = "another.employer.contact@employer.co.uk",
                EmployerCrmId = new Guid("22222222-2222-2222-2222-222222222222"),
                JobRole = "Another Job Role",
                Placements = 3,
                Postcode = "CV1 4WT",
                Route = "Another Route",
                Town = "Another Town",
                ModifiedOn = new DateTime(2019, 12, 1)
            });

            return this;
        }

        public List<EmployerFeedbackDto> Build() => _employerFeedbackDtos;
    }
}