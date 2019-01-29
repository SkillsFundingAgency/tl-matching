//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using NSubstitute;
//using NUnit.Framework;
//using Sfa.Tl.Matching.Application.Interfaces;
//using Sfa.Tl.Matching.Models;
//using Sfa.Tl.Matching.Models.ViewModel;
//using Sfa.Tl.Matching.Web.Controllers;

//namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.DataImport
//{
//    public class When_Data_Import_Controller_Index_Is_Submitted_With_No_File
//    {
//        private IActionResult _result;
//        private IDataImportService<> _dataImportService;
//        private readonly IFormFile _formFile = null;
//        private DataImportController _dataImportController;
//        private SelectedImportDataViewModel _viewModel;

//        [SetUp]
//        public void Setup()
//        {
//            _viewModel = new SelectedImportDataViewModel();

//            var viewModelMapper = Substitute.For<IDataImportViewModelMapper>();
//            _dataImportService = Substitute.For<IDataImportService>();
            
//            _dataImportController = new DataImportController(viewModelMapper, _dataImportService);
           
//            _result = _dataImportController.Import(_formFile, _viewModel).Result;
//        }

//        [Test]
//        public void Then_View_Result_Is_Returned() =>
//             Assert.IsAssignableFrom<ViewResult>(_result);

//        [Test]
//        public void Then_Model_State_Has_1_Error() =>
//            Assert.AreEqual(1, _dataImportController.ViewData.ModelState.Count);

//        [Test]
//        public void Then_Model_State_Has_File_Key() =>
//            Assert.True(_dataImportController.ViewData.ModelState.ContainsKey("file"));

//        [Test]
//        public void Then_Model_State_Has_File_Error()
//        {
//            var modelStateEntry = _dataImportController.ViewData.ModelState["file"];
//            Assert.AreEqual("A file must be selected", modelStateEntry.Errors[0].ErrorMessage);
//        }

//        [Test]
//        public void Then_Service_Upload_Is_Not_Called() =>
//            _dataImportService.Received(0).Import(_formFile, _viewModel);
//    }
//}