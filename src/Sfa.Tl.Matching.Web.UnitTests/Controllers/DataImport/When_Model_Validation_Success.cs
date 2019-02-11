using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Sfa.Tl.Matching.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

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