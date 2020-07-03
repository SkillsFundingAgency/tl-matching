using AutoMapper;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Services
{
    public class QualificationRouteMappingService : IQualificationRouteMappingService
    {
        private readonly IRepository<QualificationRouteMapping> _repository;
        private readonly IMapper _mapper;

        public QualificationRouteMappingService(IMapper mapper, IRepository<QualificationRouteMapping> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<int> CreateQualificationRouteMappingAsync(QualificationRouteMappingViewModel viewModel)
        {
            var qualificationRouteMapping = _mapper.Map<QualificationRouteMappingViewModel, QualificationRouteMapping>(viewModel);
            var qualificationRouteMappingId = await _repository.CreateAsync(qualificationRouteMapping);
            return qualificationRouteMappingId;
        }

        public async Task<QualificationRouteMappingViewModel> GetQualificationRouteMappingAsync(int routeId, int qualificationId)
        {
            var qualificationRouteMapping = await _repository.GetSingleOrDefaultAsync(rm => rm.RouteId == routeId && rm.QualificationId == qualificationId);
            
            if (qualificationRouteMapping == null)
            {
                return null;
            }

            var qualificationRouteMappingVm = _mapper.Map<QualificationRouteMapping, QualificationRouteMappingViewModel>(qualificationRouteMapping);
            return qualificationRouteMappingVm;
        }
    }
}
