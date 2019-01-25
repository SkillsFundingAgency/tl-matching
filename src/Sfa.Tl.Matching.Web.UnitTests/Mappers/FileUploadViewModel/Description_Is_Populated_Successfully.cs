using Humanizer;
using NUnit.Framework;
using Sfa.Tl.Matching.Domain.Enums;
using Sfa.Tl.Matching.Web.Mappers;

namespace Sfa.Tl.Matching.Web.UnitTests.Mappers.FileUploadViewModel
{
    public class Description_Is_Populated_Successfully
    {
        private ViewModels.FileUploadViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            var mapper = new FileUploadViewModelMapper();
            _viewModel = mapper.Populate();
        }

        [Test]
        public void Is_Not_Null() =>
            Assert.NotNull(_viewModel);

        [Test]
        public void File_Type_View_Models_Is_Not_Null() =>
            Assert.NotNull(_viewModel.FileTypeViewModels);

        [Test]
        public void FileUploadType_Name_Is_Mapped_Correctly() =>
            Assert.AreEqual(FileUploadType.RouteAndPathway.Humanize(), _viewModel.FileTypeViewModels[6].Name);
    }
}