using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class RoutePathService : IRoutePathService
    {
        private readonly ILogger<RoutePathService> _logger;
        private readonly IDataImportService<RoutePathMappingDto> _dataImportService;
        private readonly IMapper _mapper;
        private readonly IRoutePathRepository _repository;
        private readonly IRepository<RoutePathMapping> _routePathMappingRepository;

        public RoutePathService(
            ILogger<RoutePathService> logger,
            IMapper mapper,
            IDataImportService<RoutePathMappingDto> dataImportService,
            IRoutePathRepository repository,
            IRepository<RoutePathMapping> routePathMappingRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _dataImportService = dataImportService;
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

        public void ImportQualificationPathMapping(System.IO.Stream stream)
        {
            _logger.LogInformation("Processing Qualification Path Mapping.");

            var import = _dataImportService.Import(stream, DataImportType.RouteAndPathway);

            if (import != null)
            {
                var routePathMappings = _mapper.Map<IEnumerable<RoutePathMapping>>(import);
                _routePathMappingRepository.CreateMany(routePathMappings);
            }
        }

        public void IndexQualificationPathMapping()
        {
            throw new System.NotImplementedException();
        }
    }
}
