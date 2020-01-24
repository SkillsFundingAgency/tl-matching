using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Opportunity_Controller_Download_Opportunity_Spreadsheet_Is_Called : IClassFixture<OpportunityControllerFixture>
    {
        private readonly OpportunityControllerFixture _fixture;
        private readonly IActionResult _result;

        public When_Opportunity_Controller_Download_Opportunity_Spreadsheet_Is_Called(OpportunityControllerFixture fixture)
        {
            _fixture = fixture;
            
            _fixture.OpportunityService.GetOpportunitySpreadsheetDataAsync(1).Returns(
                new FileDownloadDto()
                {
                    FileName = "test_file.xlsx",
                    ContentType = "application/file",
                    FileContent = new byte[] { 01, 02 }
                });

            _result = _fixture.Sut.DownloadOpportunitySpreadsheetAsync(_fixture.OpportunityId).GetAwaiter().GetResult();
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
        public void Get_Opportunity_Spreadsheet_Data_Is_Called_Exactly_Once_In_Correct_Order()
        {
            _fixture.OpportunityService
                .Received(3)
                .GetOpportunitySpreadsheetDataAsync(_fixture.OpportunityId);
        }
    }
}