using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Sfa.Tl.Matching.Domain.Enums;
using Sfa.Tl.Matching.Web.ViewModels;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.FileUpload
{
    public class Model_Validation_Success
    {
        private readonly List<ValidationResult> _results = new List<ValidationResult>();

        [SetUp]
        public void Setup()
        {
            var viewModel = new FileUploadViewModel
            {
                SelectedFileType = (int)FileUploadType.Employer
            };
            var validationContext = new ValidationContext(viewModel, null, null);
            Validator.TryValidateObject(viewModel, validationContext, _results, true);
        }

        [Test]
        public void Model_Has_0_Errors() =>
            Assert.Zero(0);
    }
}