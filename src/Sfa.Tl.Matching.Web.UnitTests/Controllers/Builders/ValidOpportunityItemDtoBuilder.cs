using System;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders
{
    internal class ValidOpportunityItemDtoBuilder
    {
        public OpportunityItemDto Build() => new OpportunityItemDto
        {
            Id = 1,
            OpportunityId = 1,
            SearchRadius = 3,
            RouteId = 1,
            RouteName = "RouteName",
            JobRole = "JobRole",
            PlacementsKnown = true,
            Placements = 2,
            Postcode = "AA1 1AA",
            ModifiedBy = "ModifiedBy",
            SearchResultProviderCount = 20
        };
    }
}