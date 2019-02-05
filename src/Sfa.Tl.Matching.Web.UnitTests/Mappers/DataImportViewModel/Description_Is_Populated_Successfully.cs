//using Humanizer;
//using NUnit.Framework;
//using Sfa.Tl.Matching.Models;
//using Sfa.Tl.Matching.Models.Enums;
//using Sfa.Tl.Matching.Models.ViewModel;

//namespace Sfa.Tl.Matching.Web.UnitTests.Mappers.DataImportDto
//{
//    public class When_Description_Is_Populated_Successfully
//    {
//        private DataImportDto _viewModel;

//        [SetUp]
//        public void Setup()
//        {
//            var mapper = new DataImportViewModelMapper();
//            _viewModel = mapper.GetImportTypeSelectList();
//        }

//        [Test]
//        public void Then_Is_Not_Null() =>
//            Assert.NotNull(_viewModel);

//        [Test]
//        public void Then_File_Type_View_Models_Is_Not_Null() =>
//            Assert.NotNull(_viewModel.DataImportTypeViewModels);

//        [Test]
//        public void Then_DataImportType_Name_Is_Mapped_Correctly() =>
//            Assert.AreEqual(DataImportType.RouteAndPathway.Humanize(), _viewModel.DataImportTypeViewModels[6].Name);
//    }
//}