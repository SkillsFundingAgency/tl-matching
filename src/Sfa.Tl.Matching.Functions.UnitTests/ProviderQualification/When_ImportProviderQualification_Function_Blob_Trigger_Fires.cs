//using System.IO;
//using System.Threading.Tasks;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Extensions.Logging;

//using NSubstitute;
//using Sfa.Tl.Matching.Application.Interfaces;
//using Xunit;

//namespace Sfa.Tl.Matching.Functions.UnitTests.ProviderQualification
//{
//    public class When_ImportProviderQualification_Function_Blob_Trigger_Fires
//    {
//        private Stream _blobStream;
//        private ExecutionContext _context;
//        private ILogger _logger;
//        private IProviderQualificationService _providerQualificationService;


//        public async Task OneTimeSetup()
//        {
//            _blobStream = new MemoryStream();
//            _context = new ExecutionContext();
//            _logger = Substitute.For<ILogger>();
//            _providerQualificationService = Substitute.For<IProviderQualificationService>();
//            await Functions.ProviderQualification.ImportProviderQualification(_blobStream, "test", _context, _logger, _providerQualificationService);
//        }

//        [Fact]
//        public void ImportProviderQualification_Is_Called_Exactly_Once()
//        {
//            _providerQualificationService
//                .Received(1)
//                .ImportProviderQualification(
//                    Arg.Is(_blobStream));
//        }
//    }
//}
