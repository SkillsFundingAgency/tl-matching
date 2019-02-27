using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Web.Mappers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Mappers
{
    public class When_SearchParametersViewModelMapper_Maps_Route
    {
        private readonly IQueryable<Route> _routes;
        private readonly IList<SelectListItem> _result;

        public When_SearchParametersViewModelMapper_Maps_Route()
        {
            _routes = new List<Route>
                {
                    new Route { Id = 1, Name = "Route 1" },
                    new Route { Id = 2, Name = "Route 2" }
                }
                .AsQueryable();

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(SearchParametersViewModelMapper).Assembly));
            IMapper mapper = new Mapper(config);
            _result = mapper.Map<SelectListItem[]>(_routes);
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_Result_Should_Have_Two_Items()
        {
            _result.Count.Should().Be(2);
        }

        [Fact]
        public void Then_Result_Should_Value_Should_Be_Mapped_From_Id()
        {
            _result.First().Value.Should().Be(_routes.First().Id.ToString());
        }

        [Fact]
        public void Then_Result_Should_Text_Should_Be_Mapped_From_Name()
        {
            _result.First().Text.Should().Be(_routes.First().Name);
        }

    }
}
