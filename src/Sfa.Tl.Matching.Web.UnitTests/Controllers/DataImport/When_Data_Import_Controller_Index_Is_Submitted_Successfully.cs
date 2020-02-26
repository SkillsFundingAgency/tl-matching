using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.DataImport
{
    public class When_Data_Import_Controller_Index_Is_Submitted_Successfully
    {
        private readonly IActionResult _result;
        private readonly DataUploadDto _dataUploadDto;
        private readonly IDataBlobUploadService _dataBlobUploadService;

        public When_Data_Import_Controller_Index_Is_Submitted_Successfully()
        {
            _dataUploadDto = new DataUploadDto();
            var formFile = Substitute.For<IFormFile>();

            var viewModel = new DataImportParametersViewModel
            {
                SelectedImportType = DataImportType.LearningAimReference,
                File = formFile
            };
            var mapper = Substitute.For<IMapper>();
            mapper.Map<DataUploadDto>(viewModel).Returns(_dataUploadDto);

            _dataBlobUploadService = Substitute.For<IDataBlobUploadService>();
            formFile.ContentType.Returns("application/vnd.ms-excel");

            var dataImportController = new DataImportController(mapper, _dataBlobUploadService);
            var controllerWithClaims = new ClaimsBuilder<DataImportController>(dataImportController)
                .AddUserName("username")
                .Build();

            _result = controllerWithClaims.Index(viewModel).Result;
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
            _dataBlobUploadService.ReceivedWithAnyArgs(1).UploadAsync(_dataUploadDto);
    }
}