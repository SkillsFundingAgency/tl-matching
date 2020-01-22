using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.DataImport
{
    public class When_Data_Import_Controller_Index_Is_Submitted_With_No_File : IClassFixture<DataImportControllerFixture>
    {
        private readonly DataImportControllerFixture _fixture;
        private readonly IActionResult _result;
        
        public When_Data_Import_Controller_Index_Is_Submitted_With_No_File(DataImportControllerFixture fixture)
        {
            _fixture = fixture;
            
            var viewModel = new DataImportParametersViewModel
            {
                SelectedImportType = DataImportType.LearningAimReference
            };
            
            _result = _fixture.Sut.Index(viewModel).Result;
        }
        
        [Fact]
        public void Then_Model_State_Has_File_Error()
        {
            _result.Should().BeAssignableTo<ViewResult>();

            _fixture.Sut.ViewData.ModelState.Should().ContainSingle();

            _fixture.Sut.ViewData.ModelState.ContainsKey("file").Should().BeTrue();
            var modelStateEntry = _fixture.Sut.ViewData.ModelState["file"];
            modelStateEntry.Errors[0].ErrorMessage.Should().Be("You must select a file");
        }

        [Fact]
        public void Then_Service_Upload_Is_Not_Called() =>
            _fixture.DataUploadService.DidNotReceive().UploadAsync(_fixture.Dto);
    }
}