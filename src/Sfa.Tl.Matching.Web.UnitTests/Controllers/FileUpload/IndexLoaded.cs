using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.ViewModels;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.FileUpload
{
    public class IndexLoaded
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

            _fileUploadController = new FileUploadController(_viewModelMapper);
            _result = _fileUploadController.Index();
        }

        [Test]
        public void ResultIsNotNull() =>
            Assert.NotNull(_result);

        [Test]
        public void ViewResultIsReturned() =>
            Assert.IsAssignableFrom<ViewResult>(_result);

        [Test]
        public void ModelIsNotNull()
        {
            var viewResult = _result as ViewResult;
            Assert.NotNull(viewResult.Model);
        }

        [Test]
        public void FileUploadTypeIsNotNull()
        {
            var viewModel = GetViewModel();
            Assert.NotNull(viewModel.FileTypeViewModels);
        }

        [Test]
        public void FileUploadTypeContainsData()
        {
            var viewModel = GetViewModel();
            Assert.Greater(viewModel.FileTypeViewModels.Count, 0);
        }

        [Test]
        public void ViewModelPopulateCalledExactlyOnce() =>
            _viewModelMapper.Received(1).Populate();

        private FileUploadViewModel GetViewModel()
        {
            var viewResult = _result as ViewResult;
            var viewModel = viewResult.Model as FileUploadViewModel;

            return viewModel;
        }
    }
}