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
        private readonly IRepository<LearningAimsReference> _learningAimsReferenceRepository;

        public QualificationService(IMapper mapper,
            IRepository<Qualification> qualificationRepository,
            IRepository<QualificationRoutePathMapping> qualificationRoutePathMappingRepository,
            IRepository<LearningAimsReference> learningAimsReferenceRepository)
        {
            _mapper = mapper;
            _qualificationRepository = qualificationRepository;
            _qualificationRoutePathMappingRepository = qualificationRoutePathMappingRepository;
            _learningAimsReferenceRepository = learningAimsReferenceRepository;
        }

        public async Task<int> CreateQualificationAsync(MissingQualificationViewModel viewModel)
        {
            var qualification = _mapper.Map<Qualification>(viewModel);
            var qualificationId = await _qualificationRepository.Create(qualification);

            //This doesn't work - CrateMany tries to add records twice
            //var mappings = viewModel
            //    .Routes?
            //    .Where(r => r.IsSelected)
            //    .Select(route => new QualificationRoutePathMapping
            //    {
            //        QualificationId = qualificationId,
            //        RouteId = route.Id,
            //        Source = viewModel.Source,
            //        Qualification = null
            //    }).ToList();

            //await _qualificationRoutePathMappingRepository.CreateMany(mappings);

            if (viewModel.Routes != null)
            {
                foreach (var route in viewModel.Routes.Where(r => r.IsSelected))
                {
                    await _qualificationRoutePathMappingRepository.Create(
                        new QualificationRoutePathMapping
                        {
                            QualificationId = qualificationId,
                            RouteId = route.Id,
                            Source = viewModel.Source,
                            Qualification = null
                        });
                }
            }

            return qualificationId;
        }

        public async Task<QualificationDetailViewModel> GetQualificationAsync(string larId)
        {
            var qualification = await _qualificationRepository.GetSingleOrDefault(p => p.LarsId == larId);
            return _mapper.Map<Qualification, QualificationDetailViewModel>(qualification);
        }

        public async Task<string> GetLarTitleAsync(string larId)
        {
            var lar = await _learningAimsReferenceRepository.GetSingleOrDefault(l => l.LarId == larId);
            return lar?.Title;
        }

        public async Task<bool> IsValidOfqualLarIdAsync(string larId)
        {
            var lar = await _learningAimsReferenceRepository.GetSingleOrDefault(l => l.LarId == larId);
            return lar != null;
        }

        public async Task<bool> IsValidLarIdAsync(string larId)
        {
            return await Task.FromResult(larId?.Length == 8);
        }
    }
}
