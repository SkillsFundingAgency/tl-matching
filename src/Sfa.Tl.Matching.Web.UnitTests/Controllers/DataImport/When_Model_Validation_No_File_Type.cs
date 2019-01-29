using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Sfa.Tl.Matching.Web.ViewModels;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.DataImport
{
    public class When_Model_Validation_No_File_Type
    {
        private readonly List<ValidationResult> _results = new List<ValidationResult>();

        [SetUp]
        public void Setup()
        {
            var viewModel = new DataImportViewModel();
            var validationContext = new ValidationContext(viewModel, null, null);
            Validator.TryValidateObject(viewModel, validationContext, _results, true);
        }

        [Test]
        public void Then_Model_Has_1_Error() =>
            Assert.AreEqual(1, _results.Count);

        [Test]
        public void Then_Model_State_Has_Correct_Error_Message() =>
            Assert.AreEqual("A file type must be selected", _results[0].ErrorMessage);
    }
}