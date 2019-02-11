using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.DataImport
{
    public class When_Model_Validation_No_File_Type
    {
        private readonly List<ValidationResult> _results = new List<ValidationResult>();


        public void Setup()
        {
            var viewModel = new DataUploadDto();
            var validationContext = new ValidationContext(viewModel, null, null);
            Validator.TryValidateObject(viewModel, validationContext, _results, true);
        }

        [Fact]
        public void Then_Model_Has_1_Error() =>
            Assert.Equal(1, _results.Count);

        [Fact]
        public void Then_Model_State_Has_Correct_Error_Message() =>
            Assert.Equal("A file type must be selected", _results[0].ErrorMessage);
    }
}