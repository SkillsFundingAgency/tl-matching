using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.Services;
using Sfa.Tl.Matching.Web.ViewModels;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.FileUpload
{
    public class Index_Page_Is_Loaded
    {
        private FileUploadController _fileUploadController;
        private IFileUploadViewModelMapper _viewModelMapper;
        private IActionResult _result;

        [SetUp]
        public void Setup()
        {
            var viewModel = new FileUploadViewModel();
            viewModel.FileTypeViewModels.Add(new FileTypeViewModel
            {
                Id = 1,
                Name = "FileTypeName"
            });

            _viewModelMapper = Substitute.For<IFileUploadViewModelMapper>();
            _viewModelMapper.Populate().Returns(viewModel);

            var uploadService = Substitute.For<IUploadService>();

            _fileUploadController = new FileUploadController(_viewModelMapper, uploadService);
            _result = _fileUploadController.Index();
        }

        [Test]
        public void Result_Is_Not_Null() =>
            Assert.NotNull(_result);

        [Test]
        public void View_Result_Is_Returned() =>
            Assert.IsAssignableFrom<ViewResult>(_result);

        [Test]
        public void Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            Assert.NotNull(viewResult.Model);
        }

        [Test]
        public void File_Upload_Type_Is_Not_Null()
        {
            var viewModel = GetViewModel();
            Assert.NotNull(viewModel.FileTypeViewModels);
        }

        [Test]
        public void View_Model_Mapper_Populate_Is_Called_Exactly_Once() =>
            _viewModelMapper.Received(1).Populate();

        [Test]
        public void File_Upload_Type_Contains_Data()
        {
            var viewModel = GetViewModel();
            Assert.Greater(viewModel.FileTypeViewModels.Count, 0);
        }

        private FileUploadViewModel GetViewModel()
        {
            var viewResult = _result as ViewResult;
            var viewModel = viewResult.Model as FileUploadViewModel;

            return viewModel;
        }
    }
}