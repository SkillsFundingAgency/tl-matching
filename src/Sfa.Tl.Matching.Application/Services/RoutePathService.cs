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
    public class RoutePathService : IRoutePathService
    {
        private readonly ILogger<RoutePathService> _logger;
        private readonly IFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto> _fileReader;
        private readonly IMapper _mapper;
        private readonly IRepository<Route> _routeRepository;
        private readonly IRepository<Path> _pathRepository;
        private readonly IRepository<RoutePathMapping> _routePathMappingRepository;

        public RoutePathService(
            ILogger<RoutePathService> logger,
            IMapper mapper,
            IFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto> fileReader,
            IRepository<Route> routeRepository,
            IRepository<Path> pathRepository,
            IRepository<RoutePathMapping> routePathMappingRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _fileReader = fileReader;
            _pathRepository = pathRepository;
            _routeRepository = routeRepository;
            _routePathMappingRepository = routePathMappingRepository;
        }

        public IQueryable<Path> GetPaths()
        {
            return _pathRepository.GetMany(x => true).GetAwaiter().GetResult();
        }

        public IQueryable<Route> GetRoutes()
        {
            return _routeRepository.GetMany(x => true).GetAwaiter().GetResult();

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
