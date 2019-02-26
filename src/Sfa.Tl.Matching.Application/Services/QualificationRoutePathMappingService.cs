using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Services
{
    public class QualificationRoutePathMappingService : IQualificationRoutePathMappingService
    {
        private readonly ILogger<QualificationRoutePathMappingService> _logger;
        private readonly IFileReader<QualificationRoutePathMappingFileImportDto, QualificationRoutePathMappingDto> _fileReader;
        private readonly IMapper _mapper;
        private readonly IRepository<QualificationRoutePathMapping> _routePathMappingRepository;

        public QualificationRoutePathMappingService(
            ILogger<QualificationRoutePathMappingService> logger,
            IMapper mapper,
            IFileReader<QualificationRoutePathMappingFileImportDto, QualificationRoutePathMappingDto> fileReader,
            IRepository<QualificationRoutePathMapping> routePathMappingRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _fileReader = fileReader;
            _routePathMappingRepository = routePathMappingRepository;
        }

        public void IndexQualificationPathMapping()
        {
            throw new NotImplementedException();
        }
    }
}
