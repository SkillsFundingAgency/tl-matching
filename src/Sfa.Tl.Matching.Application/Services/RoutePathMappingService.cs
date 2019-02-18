using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Services
{
    public class RoutePathMappingService : IRoutePathMappingService
    {
        private readonly ILogger<RoutePathMappingService> _logger;
        private readonly IFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto> _fileReader;
        private readonly IMapper _mapper;
        private readonly IRepository<RoutePathMapping> _routePathMappingRepository;

        public RoutePathMappingService(
            ILogger<RoutePathMappingService> logger,
            IMapper mapper,
            IFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto> fileReader,
            IRepository<RoutePathMapping> routePathMappingRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _fileReader = fileReader;
            _routePathMappingRepository = routePathMappingRepository;
        }

        public async Task<int> ImportQualificationPathMapping(QualificationRoutePathMappingFileImportDto fileImportDto)
        {
            _logger.LogInformation("Processing Qualification Path Mapping.");

            var import = _fileReader.ValidateAndParseFile(fileImportDto);

            var createdRecords = 0;
            if (import != null && import.Any())
            {
                var routePathMappings = _mapper.Map<IList<RoutePathMapping>>(import);
                createdRecords = await _routePathMappingRepository.CreateMany(routePathMappings);
            }

            return createdRecords;
        }

        public void IndexQualificationPathMapping()
        {
            throw new NotImplementedException();
        }
    }
}
