using NUnit.Framework;
using Sfa.Tl.Matching.Domain.Enums;
using Sfa.Tl.Matching.Web.Mappers;

namespace Sfa.Tl.Matching.Web.UnitTests.Mappers.FileUploadViewModel
{
    public class When_ViewModel_Is_Populated_Successfully
    {
        private ViewModels.FileUploadViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            var mapper = new FileUploadViewModelMapper();
            _viewModel = mapper.Populate();
        }

        [Test]
        public void Then_Is_Not_Null() =>
            Assert.NotNull(_viewModel);

        [Test]
        public void Then_File_Type_View_Models_Is_NotNull() =>
            Assert.NotNull(_viewModel.FileTypeViewModels);

        [Test]
        public void Then_File_Uploa_dType_Enum_Id_Is_Mapped_Correctly() =>
            Assert.AreEqual((int)FileUploadType.Employer, _viewModel.FileTypeViewModels[0].Id);

        [Test]
        public void Then_File_Upload_Type_Enum_Name_Is_Mapped_Correctly() =>
            Assert.AreEqual(FileUploadType.Employer.ToString(), _viewModel.FileTypeViewModels[0].Name);
    }
}