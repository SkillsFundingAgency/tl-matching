using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.Services;
using Sfa.Tl.Matching.Web.ViewModels;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.DataImport
{
    public class When_Data_Import_Controller_Index_Is_Loaded
    {
        private DataImportController _dataImportController;
        private IDataImportViewModelMapper _viewModelMapper;
        private IActionResult _result;

        [SetUp]
        public void Setup()
        {
            var viewModel = new DataImportViewModel();
            viewModel.DataImportTypeViewModels.Add(new DataImportTypeViewModel
            {
                Id = 1,
                Name = "DataImportTypeName"
            });

            _viewModelMapper = Substitute.For<IDataImportViewModelMapper>();
            _viewModelMapper.Populate().Returns(viewModel);

            var uploadService = Substitute.For<IUploadService>();

            _dataImportController = new DataImportController(_viewModelMapper, uploadService);
            _result = _dataImportController.Index();
        }

        [Test]
        public void Then_Result_Is_Not_Null() =>
            Assert.NotNull(_result);

        [Test]
        public void Then_View_Result_Is_Returned() =>
            Assert.IsAssignableFrom<ViewResult>(_result);

        [Test]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            Assert.NotNull(viewResult?.Model);
        }

        [Test]
        public void Then_Data_Import_Type_Is_Not_Null()
        {
            var viewModel = GetViewModel();
            Assert.NotNull(viewModel.DataImportTypeViewModels);
        }

        [Test]
        public void Then_Mapper_Populate_Is_Called_Exactly_Once() =>
            _viewModelMapper.Received(1).Populate();

        [Test]
        public void Then_Data_Import_Type_Contains_Data()
        {
            var viewModel = GetViewModel();
            Assert.Greater(viewModel.DataImportTypeViewModels.Count, 0);
        }

        private DataImportViewModel GetViewModel()
        {
            var viewResult = _result as ViewResult;
            var viewModel = viewResult?.Model as DataImportViewModel;

            return viewModel;
        }
    }
}