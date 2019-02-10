using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Functions.UnitTests.Employer

{
    public class When_ImportEmployer_Function_Blob_Trigger_Fires
    {
        private Stream _blobStream;
        private ExecutionContext _context;
        private ILogger _logger;
        private IEmployerService _employerService;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            _blobStream = new MemoryStream();
            _context = new ExecutionContext();
            _logger = Substitute.For<ILogger>();
            _employerService = Substitute.For<IEmployerService>();
            await Functions.Employer.ImportEmployer(_blobStream, "test", _context, _logger, _employerService);
        }

        [Test]
        public void ImportEmployer_Is_Called_Exactly_Once()
        {
            _employerService
                .Received(1)
                .ImportEmployer(
                    Arg.Is<EmployerFileImportDto>(dto => dto.FileDataStream == _blobStream));
        }
    }
}
