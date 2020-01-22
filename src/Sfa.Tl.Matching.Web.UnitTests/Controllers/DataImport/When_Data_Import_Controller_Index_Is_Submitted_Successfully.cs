using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.DataImport
{
    public class When_Data_Import_Controller_Index_Is_Submitted_Successfully : IClassFixture<DataImportControllerFixture>
    {
        private readonly DataImportControllerFixture _fixture;
        private readonly IActionResult _result;
        
        public When_Data_Import_Controller_Index_Is_Submitted_Successfully(DataImportControllerFixture fixture)
        {
            _fixture = fixture;
            var formFile = Substitute.For<IFormFile>();

            var viewModel = new DataImportParametersViewModel
            {
                SelectedImportType = DataImportType.LearningAimReference,
                File = formFile
            };
            
            _fixture.Mapper.Map<DataUploadDto>(viewModel).Returns(_fixture.Dto);

            formFile.ContentType.Returns("application/vnd.ms-excel");

            _result = _fixture.ControllerWithClaims.Index(viewModel).Result;
        }

        [Fact]
        public void Then_Model_State_Has_No_Errors()
        {
            _result.Should().BeAssignableTo<ViewResult>();
            
            var viewResult = (ViewResult)_result;
            viewResult.ViewData.ModelState.Should().BeEmpty();
        }

        [Fact]
        public void Then_Service_Upload_Is_Called_Exactly_Once() =>
            _fixture.DataUploadService.ReceivedWithAnyArgs(2).UploadAsync(_fixture.Dto);
    }
}