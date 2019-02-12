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
//        private readonly Stream _blobStream;
//        private readonly IProviderQualificationService _providerQualificationService;

//        public When_ImportProviderQualification_Function_Blob_Trigger_Fires()
//        {
//            _blobStream = new MemoryStream();
//            var context = new ExecutionContext();
//            var logger = Substitute.For<ILogger>();
//            _providerQualificationService = Substitute.For<IProviderQualificationService>();
//            Functions.ProviderQualification.ImportProviderQualification(
//                    _blobStream, 
//                    "test", 
//                    context, 
//                    logger, 
//                    _providerQualificationService).GetAwaiter().GetResult();
//        }

//        [Fact]
//        public void ImportProviderQualification_Is_Called_Exactly_Once()
//        {
//            _providerQualificationService
//                .Received(1)
//                .ImportProviderQualification(
//                    Arg.Is(Arg.Is<ProviderQualificationFileImportDto>(dto => dto.FileDataStream == _blobStream)));
//        }
//    }
//}

