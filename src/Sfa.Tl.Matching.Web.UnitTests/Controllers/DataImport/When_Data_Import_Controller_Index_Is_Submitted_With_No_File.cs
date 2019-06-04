using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.DataImport
{
    public class When_Data_Import_Controller_Index_Is_Submitted_With_No_File
    {
        private readonly IActionResult _result;
        private readonly DataImportController _dataImportController;
        private readonly DataUploadDto _dataUploadDto;
        private readonly IDataBlobUploadService _dataBlobUploadService;

        public When_Data_Import_Controller_Index_Is_Submitted_With_No_File()
        {
            _dataUploadDto = new DataUploadDto();

            var mapper = Substitute.For<IMapper>();
            _dataBlobUploadService = Substitute.For<IDataBlobUploadService>();
            _dataImportController = new DataImportController(mapper, _dataBlobUploadService);

            var viewModel = new DataImportParametersViewModel
            {
                SelectedImportType = DataImportType.Employer
            };
            _result = _dataImportController.Index(viewModel).Result;
        }

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_Model_State_Has_1_Error() =>
            _dataImportController.ViewData.ModelState.Should().ContainSingle();

        [Fact]
        public void Then_Model_State_Has_File_Key() =>
            _dataImportController.ViewData.ModelState.ContainsKey("file").Should().BeTrue();

        [Fact]
        public void Then_Model_State_Has_File_Error()
        {
            var modelStateEntry = _dataImportController.ViewData.ModelState["file"];
            modelStateEntry.Errors[0].ErrorMessage.Should().Be("You must select a file");
        }

        [Fact]
        public void Then_Service_Upload_Is_Not_Called() =>
            _dataBlobUploadService.DidNotReceive().Upload(_dataUploadDto);
    }
}