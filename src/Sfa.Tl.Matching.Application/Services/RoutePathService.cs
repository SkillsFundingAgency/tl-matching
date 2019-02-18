using System.Linq;
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

        public RoutePathService(
            ILogger<RoutePathService> logger,
            IRepository<Route> routeRepository,
            IRepository<Path> pathRepository)
        {
            _logger = logger;
            _pathRepository = pathRepository;
            _routeRepository = routeRepository;
        }

        public IQueryable<Path> GetPaths()
        {
            return _pathRepository.GetMany(x => true).GetAwaiter().GetResult();
        }

        public IQueryable<Route> GetRoutes()
        {
            return _routeRepository.GetMany(x => true).GetAwaiter().GetResult();
        }
    }
}
