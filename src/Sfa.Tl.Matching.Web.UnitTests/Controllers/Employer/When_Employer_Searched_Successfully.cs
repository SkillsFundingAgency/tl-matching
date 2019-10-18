using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
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
            var referralService = Substitute.For<IReferralService>();

            _employerService.Search(query).Returns(new List<EmployerSearchResultDto>
            {
                new EmployerSearchResultDto
                {
                    CompanyName = "CompanyName1",
                    AlsoKnownAs = "AlsoKnownAs1"
                },
                new EmployerSearchResultDto
                {
                    CompanyName = "CompanyName2",
                    AlsoKnownAs = "AlsoKnownAs2"
                }
            });
            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);

            var employerController = new EmployerController(_employerService, null, referralService, mapper);

            _result = employerController.SearchEmployer(query);
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
        public void Then_First_Item_CompanyName_Is_Correct()
        {
            var resultList = GetResultList();
            resultList[0].CompanyName.Should().Be("CompanyName1");
        }

        [Fact]
        public void Then_First_Item_AlsoKnownAs_Is_Correct()
        {
            var resultList = GetResultList();
            resultList[0].AlsoKnownAs.Should().Be("AlsoKnownAs1");
        }

        [Fact]
        public void Then_Second_Item_CompanyName_Is_Correct()
        {
            var resultList = GetResultList();
            resultList[1].CompanyName.Should().Be("CompanyName2");
        }

        [Fact]
        public void Then_Second_Item_AlsoKnownAs_Is_Correct()
        {
            var resultList = GetResultList();
            resultList[1].AlsoKnownAs.Should().Be("AlsoKnownAs2");
        }

        [Fact]
        public void Then_Search_Is_Called_Exactly_Once()
        {
            _employerService.Received(1).Search("Employer");
        }

        private List<EmployerSearchResultDto> GetResultList()
        {
            var viewResult = _result as OkObjectResult;
            var viewModel = viewResult?.Value as List<EmployerSearchResultDto>;

            return viewModel;
        }
    }
}