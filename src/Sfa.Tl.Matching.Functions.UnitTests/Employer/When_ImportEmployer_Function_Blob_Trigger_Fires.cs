using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.Employer
{
    public class When_ImportEmployer_Function_Blob_Trigger_Fires
    {
        public When_ImportEmployer_Function_Blob_Trigger_Fires()
        {
            _blobStream = new MemoryStream();
            var context = new ExecutionContext();
            var logger = Substitute.For<ILogger>();
            _employerService = Substitute.For<IEmployerService>();
            Functions.Employer.ImportEmployer(
                _blobStream,
                "test",
                context,
                logger,
                _employerService).GetAwaiter().GetResult();
        }

        private readonly Stream _blobStream;
        private readonly IEmployerService _employerService;

        [Fact]
        public void ImportEmployer_Is_Called_Exactly_Once()
        {
            _employerService
                .Received(1)
                .ImportEmployer(Arg.Is<EmployerFileImportDto>(dto => dto.FileDataStream == _blobStream));
        }
    }
}