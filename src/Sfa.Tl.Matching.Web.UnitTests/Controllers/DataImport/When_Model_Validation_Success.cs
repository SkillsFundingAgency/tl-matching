using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.DataImport
{
    public class When_Model_Validation_Success
    {
        private readonly List<ValidationResult> _results = new List<ValidationResult>();


        //public void Setup()
        //{
        //    var viewModel = new DataUploadDto
        //    {
        //        SelectedDataImportType = (int)DataImportType.Employer
        //    };
        //    var validationContext = new ValidationContext(viewModel, null, null);
        //    Validator.TryValidateObject(viewModel, validationContext, _results, true);
        //}

        //[Fact]
        //public void Then_Model_Has_0_Errors() =>
        //    Assert.Zero(0);
    }
}