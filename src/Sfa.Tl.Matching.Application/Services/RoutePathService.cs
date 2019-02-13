using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Path = Sfa.Tl.Matching.Domain.Models.Path;

namespace Sfa.Tl.Matching.Application.Services
{
    public class RoutePathService : IRoutePathService
    {
        private readonly ILogger<RoutePathService> _logger;
        private readonly IFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto> _fileReader;
        private readonly IMapper _mapper;
        private readonly IRoutePathRepository _repository;
        private readonly IRepository<RoutePathMapping> _routePathMappingRepository;

        public RoutePathService(
            ILogger<RoutePathService> logger,
            IMapper mapper,
            IFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto> fileReader,
            IRoutePathRepository repository,
            IRepository<RoutePathMapping> routePathMappingRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _fileReader = fileReader;
            _repository = repository;
            _routePathMappingRepository = routePathMappingRepository;
        }

        public IQueryable<Path> GetPaths()
        {
            return _repository.GetPaths();
        }

        public IQueryable<Route> GetRoutes()
        {
            return _repository.GetRoutes();
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
            throw new System.NotImplementedException();
        }
    }
}
