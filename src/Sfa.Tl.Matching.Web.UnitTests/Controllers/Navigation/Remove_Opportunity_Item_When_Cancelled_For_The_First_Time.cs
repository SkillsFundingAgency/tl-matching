using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Navigation
{
    public class Remove_Opportunity_Item_When_Cancelled_For_The_First_Time
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IActionResult _result;

        public Remove_Opportunity_Item_When_Cancelled_For_The_First_Time()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));

            var mapper = new Mapper(config);

            _opportunityService = Substitute.For<IOpportunityService>();


            var navigationController = new NavigationController(_opportunityService);

            _result = navigationController.RemoveOpportunityItemAndGetOpportunityBasket(1, 1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Remove_Opportunity_Item()
        {

        }
    }
}
