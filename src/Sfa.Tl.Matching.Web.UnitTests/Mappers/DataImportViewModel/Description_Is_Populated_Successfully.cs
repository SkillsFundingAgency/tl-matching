using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Mappers.DataUploadDto
{
    public class When_Description_Is_Populated_Successfully
    {
        private Models.Dto.DataUploadDto _viewModel;


        public When_Description_Is_Populated_Successfully()
        {
            //var mapper = new DataImportViewModelMapper();
            //_viewModel = mapper.GetImportTypeSelectList();
        }

        [Fact]
        public void Then_Is_Not_Null() =>
            Assert.NotNull(_viewModel);

        //[Fact]
        //public void Then_File_Type_View_Models_Is_Not_Null() =>
        //    Assert.NotNull(_viewModel.DataImportTypeViewModels);

        //[Fact]
        //public void Then_DataImportType_Name_Is_Mapped_Correctly() =>
        //    Assert.Equal(DataImportType.RouteAndPathway.Humanize(), _viewModel.DataImportTypeViewModels[6].Name);
    }
}