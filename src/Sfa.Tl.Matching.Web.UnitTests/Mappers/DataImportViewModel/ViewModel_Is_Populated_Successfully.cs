//using NUnit.Framework;
//using Sfa.Tl.Matching.Models;
//using Sfa.Tl.Matching.Models.Enums;
//using Sfa.Tl.Matching.Models.ViewModel;

//namespace Sfa.Tl.Matching.Web.UnitTests.Mappers.DataImportViewModel
//{
//    public class When_ViewModel_Is_Populated_Successfully
//    {
//        private SelectedImportDataViewModel _viewModel;

//        [SetUp]
//        public void Setup()
//        {
//            var mapper = new DataImportViewModelMapper();
//            _viewModel = mapper.Populate();
//        }

//        [Test]
//        public void Then_Is_Not_Null() =>
//            Assert.NotNull(_viewModel);

//        [Test]
//        public void Then_File_Type_View_Models_Is_NotNull() =>
//            Assert.NotNull(_viewModel.DataImportTypeViewModels);

//        [Test]
//        public void Then_Data_Import_Type_Enum_Id_Is_Mapped_Correctly() =>
//            Assert.AreEqual((int)DataImportType.Employer, _viewModel.DataImportTypeViewModels[0].Id);

//        [Test]
//        public void Then_Data_Import_Type_Enum_Name_Is_Mapped_Correctly() =>
//            Assert.AreEqual(DataImportType.Employer.ToString(), _viewModel.DataImportTypeViewModels[0].Name);
//    }
//}