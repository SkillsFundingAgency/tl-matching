using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class QualificationService : IQualificationService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Qualification> _qualificationRepository;
        private readonly IRepository<QualificationRoutePathMapping> _qualificationRoutePathMappingRepository;
        private readonly IRepository<LearningAimReference> _learningAimReferenceRepository;

        public QualificationService(IMapper mapper,
            IRepository<Qualification> qualificationRepository,
            IRepository<QualificationRoutePathMapping> qualificationRoutePathMappingRepository,
            IRepository<LearningAimReference> learningAimReferenceRepository)
        {
            _mapper = mapper;
            _qualificationRepository = qualificationRepository;
            _qualificationRoutePathMappingRepository = qualificationRoutePathMappingRepository;
            _learningAimReferenceRepository = learningAimReferenceRepository;
        }

        public async Task<int> CreateQualificationAsync(MissingQualificationViewModel viewModel)
        {
            var qualification = _mapper.Map<Qualification>(viewModel);

            var qualificationRoutePathMappings = viewModel
                .Routes?
                .Where(r => r.IsSelected)
                .Select(route => new QualificationRoutePathMapping
                {
                    RouteId = route.Id,
                    Source = viewModel.Source,
                    Qualification = qualification
                }).ToList();

            if (qualificationRoutePathMappings?.Count > 0)
            {
                await _qualificationRoutePathMappingRepository.CreateMany(qualificationRoutePathMappings);
            }

            return qualification.Id;
        }

        public async Task<QualificationDetailViewModel> GetQualificationAsync(string larId)
        {
            var qualification = await _qualificationRepository.GetSingleOrDefault(p => p.LarsId == larId);
            return _mapper.Map<Qualification, QualificationDetailViewModel>(qualification);
        }

        public async Task<string> GetLarTitleAsync(string larId)
        {
            var lar = await _learningAimReferenceRepository.GetSingleOrDefault(l => l.LarId == larId);
            return lar?.Title;
        }

        public Task UpdateQualificationAsync(SaveQualificationViewModel viewModel)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> IsValidOfqualLarIdAsync(string larId)
        {
            var lar = await _learningAimReferenceRepository.GetSingleOrDefault(l => l.LarId == larId);
            return lar != null;
        }

        public async Task<bool> IsValidLarIdAsync(string larId)
        {
            return await Task.FromResult(larId?.Length == 8);
        }
    }
}
