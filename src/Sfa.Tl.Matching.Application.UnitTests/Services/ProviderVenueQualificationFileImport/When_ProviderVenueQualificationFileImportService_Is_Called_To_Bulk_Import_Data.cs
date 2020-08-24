using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenueQualificationFileImport.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenueQualificationFileImport
{
    public class When_ProviderVenueQualificationFileImportService_Is_Called_To_Bulk_Import_Data
    {
        private readonly IProviderVenueQualificationReader _fileReader;
        private readonly IProviderVenueQualificationService _providerVenueQualificationService;
        private readonly IRepository<FunctionLog> _functionLogRepository;
        private readonly int _result;

        public When_ProviderVenueQualificationFileImportService_Is_Called_To_Bulk_Import_Data()
        {
            var logger = Substitute.For<ILogger<IProviderVenueQualificationFileImportService>>();
            _fileReader = Substitute.For<IProviderVenueQualificationReader>();
            _providerVenueQualificationService = Substitute.For<IProviderVenueQualificationService>();
            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();
            
            var fileImportDto = new ValidProviderVenueQualificationFileImportDtoBuilder().Build();
            var readResultDto = new ValidProviderVenueQualificationReadResultDtoBuilder().Build();
            var updateResultDtoList = new ValidProviderVenueQualificationUpdateResultsDtoBuilder().Build();

            _fileReader.ReadData(fileImportDto)
                .Returns(readResultDto);

            _providerVenueQualificationService.Update(readResultDto.ProviderVenueQualifications)
                .Returns(updateResultDtoList);

            var service = new ProviderVenueQualificationFileImportService(logger, _fileReader,
                _providerVenueQualificationService, _functionLogRepository);

            _result = service.BulkImportAsync(fileImportDto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_As_Expected()
        {
            _result.Should().Be(1);
        }

        [Fact]
        public void ProviderVenueQualificationReader_ReadData_Is_Called_Exactly_Once()
        {
            _fileReader
                .Received(1)
                .ReadData(Arg.Any<ProviderVenueQualificationFileImportDto>());
        }
        
        [Fact]
        public void ProviderVenueQualificationService_Update_Is_Called_Exactly_Once()
        {
            _providerVenueQualificationService
                .Received(1)
                .Update(Arg.Any<IEnumerable<ProviderVenueQualificationDto>>());
        }

        [Fact]
        public void FunctionLogRepository_Create_Is_Not_Called()
        {
            _functionLogRepository
                .DidNotReceiveWithAnyArgs()
                .CreateAsync(Arg.Any<FunctionLog>());
        }
    }
}
