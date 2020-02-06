using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderProximity
{
    public class When_ProviderProximity_Controller_Download_Provider_Spreadsheet_Is_Called
    {
        private readonly IProviderProximityService _providerProximityService;
        private readonly IActionResult _result;
        private readonly ProviderProximitySearchParametersDto _searchParameters;

        public When_ProviderProximity_Controller_Download_Provider_Spreadsheet_Is_Called()
        {
            var locationService = Substitute.For<ILocationService>();
            _providerProximityService = Substitute.For<IProviderProximityService>();
            var routePathService = Substitute.For<IRoutePathService>();

            var postcode = "CV1 2WT";
            var filters = "Digital-Analog";

            _searchParameters = new ProviderProximitySearchParametersDto
            {
                Postcode = postcode,
                SelectedRouteNames = new List<string>(filters.Split('-'))
            };

            _providerProximityService.GetProviderProximitySpreadsheetDataAsync(
                    Arg.Any<ProviderProximitySearchParametersDto>())
                .Returns(
                    new FileDownloadDto
                    {
                        FileName = "test_file.xlsx",
                        ContentType = "application/file",
                        FileContent = new byte[] { 01, 02 }
                    });

            var providerProximityController = new ProviderProximityController(routePathService, _providerProximityService, locationService);
            _result = providerProximityController.DownloadProviderProximitySpreadsheetAsync(postcode, filters).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_FileResult()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<FileResult>();
            _result.Should().BeAssignableTo<FileContentResult>();
        }

        [Fact]
        public void Then_Result_Has_Correct_File_Details()
        {
            var fileResult = _result as FileContentResult;
            fileResult.Should().NotBeNull();
            fileResult?.ContentType.Should().Be("application/file");
            fileResult?.FileDownloadName.Should().Be("test_file.xlsx");
            fileResult?.FileContents.Should().NotBeNull();
        }

        [Fact]
        public void Get_Provider_Proximity_Spreadsheet_Data_Is_Called_Exactly_Once()
        {
            _providerProximityService
                .Received(1)
                .GetProviderProximitySpreadsheetDataAsync(
                    Arg.Is<ProviderProximitySearchParametersDto>(p =>
                            p.Postcode == _searchParameters.Postcode &&
                            p.SelectedRouteNames.Contains("Digital") &&
                            p.SelectedRouteNames.Contains("Analog")));
        }
    }
}