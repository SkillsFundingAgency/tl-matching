using Humanizer;
using NUnit.Framework;
using Sfa.Tl.Matching.Core.Enums;
using Sfa.Tl.Matching.Web.Mappers;

namespace Sfa.Tl.Matching.Web.UnitTests.Mappers.FileUploadViewModel
{
    public class DescriptionPopulateSuccess
    {
        private ViewModels.FileUploadViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            var mapper = new FileUploadViewModelMapper();
            _viewModel = mapper.Populate();
        }

        [Test]
        public void IsNotNull() =>
            Assert.NotNull(_viewModel);

        [Test]
        public void FileTypeViewModelsIsNotNull() =>
            Assert.NotNull(_viewModel.FileTypeViewModels);

        [Test(Description = "FileUploadType name is mapped correctly to the View Model")]
        public void FileTypeViewModelsEnumName() =>
            Assert.AreEqual(FileUploadType.RouteAndPathway.Humanize(), _viewModel.FileTypeViewModels[6].Name);
    }
}