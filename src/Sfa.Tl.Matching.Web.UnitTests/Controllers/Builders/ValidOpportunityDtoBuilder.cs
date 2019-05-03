using System;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders
{
    internal class ValidOpportunityDtoBuilder
    {
        public OpportunityDto Build() => new OpportunityDto
        {
            Id = 1,
            SearchRadius = 3,
            RouteId = 1,
            RouteName = "RouteName",
            JobTitle = "JobTitle",
            PlacementsKnown = true,
            Placements = 2,
            Postcode = "AA1 1AA",
            EmployerName = "EmployerName",
            EmployerContact = "EmployerContact",
            EmployerContactEmail = "EmployerContactEmail",
            EmployerContactPhone = "EmployerContactPhone",
            EmployerCrmId = new Guid("65021261-8C70-4C4F-954F-4E5282250A85"),
            UserEmail = "email@address.com",
            ModifiedBy = "ModifiedBy",
            SearchResultProviderCount = 20
        };
    }
}