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
                new()
                {
                    OpportunityItemId = 1,
                    EmployerContact = "Employer Contact",
                    EmployerContactEmail = "employer.contact@employer.co.uk",
                    EmployerCrmId = new Guid("11111111-1111-1111-1111-111111111111"),
                    PlacementsKnown = true,
                    Placements = 1,
                    Postcode = "CV1 2WT",
                    Route = "Route",
                    Town = "Town",
                    ModifiedOn = new DateTime(2019, 12, 12)
                },
                new()
                {
                    OpportunityItemId = 2,
                    EmployerContact = "Old Employer Contact",
                    EmployerContactEmail = "old.employer.contact@employer.co.uk",
                    EmployerCrmId = new Guid("11111111-1111-1111-1111-111111111111"),
                    JobRole = "Job Role",
                    PlacementsKnown = true,
                    Placements = 3,
                    Postcode = "CV2 3WT",
                    Route = "Another Route",
                    Town = "Another Town",
                    ModifiedOn = new DateTime(2019, 12, 1)
                },
                new()
                {
                    OpportunityItemId = 3,
                    EmployerContact = "Old Employer Contact",
                    EmployerContactEmail = "old.employer.contact@employer.co.uk",
                    EmployerCrmId = new Guid("11111111-1111-1111-1111-111111111111"),
                    JobRole = "Another Job Role",
                    PlacementsKnown = false,
                    Placements = 3,
                    Postcode = "CV3 4WT",
                    Route = "And Another Route",
                    Town = "And Another Town",
                    ModifiedOn = new DateTime(2019, 12, 2)
                },
                new()
                {
                    OpportunityItemId = 4,
                    EmployerContact = "Old Employer Contact",
                    EmployerContactEmail = "old.employer.contact@employer.co.uk",
                    EmployerCrmId = new Guid("11111111-1111-1111-1111-111111111111"),
                    PlacementsKnown = true,
                    Placements = 7,
                    Postcode = "CV7 7WT",
                    Route = "And Another Route Again",
                    Town = "And Another Town Again",
                    ModifiedOn = new DateTime(2019, 12, 7)
                }
                ,
                new()
                {
                    OpportunityItemId = 5,
                    EmployerContact = "Old Employer Contact",
                    EmployerContactEmail = "old.employer.contact@employer.co.uk",
                    EmployerCrmId = new Guid("11111111-1111-1111-1111-111111111111"),
                    PlacementsKnown = false,
                    Postcode = "CV5 5WT",
                    Route = "And Another Route Again 2",
                    Town = "And Another Town Again 2",
                    ModifiedOn = new DateTime(2019, 12, 5)
                }
            };
        }

        public EmployerFeedbackDtoListBuilder AddAnotherEmployer()
        {
            _employerFeedbackDtos.Add(new EmployerFeedbackDto
            {
                OpportunityItemId = 6,
                EmployerContact = "Another Employer Contact",
                EmployerContactEmail = "another.employer.contact@employer.co.uk",
                EmployerCrmId = new Guid("22222222-2222-2222-2222-222222222222"),
                JobRole = "Another Job Role",
                PlacementsKnown = true,
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