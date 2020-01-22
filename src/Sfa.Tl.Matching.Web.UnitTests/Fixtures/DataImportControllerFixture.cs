using AutoMapper;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;

namespace Sfa.Tl.Matching.Web.UnitTests.Fixtures
{
    public class DataImportControllerFixture
    {
        public DataImportController Sut { get; }
        internal IMapper Mapper;
        internal IDataBlobUploadService DataUploadService;
        internal DataUploadDto Dto;

        public DataImportControllerFixture()
        {
            Mapper = Substitute.For<IMapper>();
            DataUploadService = Substitute.For<IDataBlobUploadService>();

            Dto = new DataUploadDto();

            Sut = new DataImportController(Mapper, DataUploadService);
        }

        public DataImportController ControllerWithClaims => new ClaimsBuilder<DataImportController>(Sut)
                                                                    .AddUserName("username")
                                                                    .Build();

    }
}
