using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_Searched_Successfully
    {
        private readonly IActionResult _result;
        private readonly IEmployerService _employerService;

        public When_Employer_Searched_Successfully()
        {
            const string query = "Employer";
            _employerService = Substitute.For<IEmployerService>();
            _employerService.Search(query).Returns(new List<EmployerSearchResultDto>
            {
                new EmployerSearchResultDto
                {
                    EmployerName = "EmployerName1",
                    AlsoKnownAs = "AlsoKnownAs1"
                },
                new EmployerSearchResultDto
                {
                    EmployerName = "EmployerName2",
                    AlsoKnownAs = "AlsoKnownAs2"
                }
            });
            var employerController = new EmployerController(_employerService, null);

            _result = employerController.Search(query).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_OkObjectResult_Is_Returned() =>
            _result.Should().BeAssignableTo<OkObjectResult>();

        [Fact]
        public void Then_Value_Is_Not_Null()
        {
            var viewResult = _result as OkObjectResult;
            viewResult?.Value.Should().NotBeNull();
        }

        [Fact]
        public void Then_First_Item_Is_Correct()
        {
            var resultList = GetResultList();
            resultList[0].Should().Be("EmployerName1 (AlsoKnownAs1)");
        }

        [Fact]
        public void Then_Second_Item_Is_Correct()
        {
            var resultList = GetResultList();
            resultList[1].Should().Be("EmployerName2 (AlsoKnownAs2)");
        }

        [Fact]
        public void Then_Search_Is_Called_Exactly_Once()
        {
            _employerService.Received(1).Search("Employer");
        }

        private List<string> GetResultList()
        {
            var viewResult = _result as OkObjectResult;
            var viewModel = viewResult?.Value as List<string>;

            return viewModel;
        }
    }
}