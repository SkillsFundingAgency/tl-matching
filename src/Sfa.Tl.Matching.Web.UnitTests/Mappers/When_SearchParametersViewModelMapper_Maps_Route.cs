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
                    new() { Id = 1, Name = "Route 1" },
                    new() { Id = 2, Name = "Route 2" }
                }
                .AsQueryable();

            var config = new MapperConfiguration(c => c.AddMaps(typeof(SearchParametersViewModelMapper).Assembly));
            IMapper mapper = new Mapper(config);
            _result = mapper.Map<SelectListItem[]>(_routes);
        }

        [Fact]
        public void Then_Result_Should_Be_Mapped_Correctly()
        {
            _result.Should().NotBeNull();
            _result.Count.Should().Be(2);
            _result.First().Value.Should().Be(_routes.First().Id.ToString());
            _result.First().Text.Should().Be(_routes.First().Name);
        }
    }
}
