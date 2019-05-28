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

        public QualificationService(IMapper mapper,
            IRepository<Qualification> qualificationRepository)
        {
            _mapper = mapper;
            _qualificationRepository = qualificationRepository;
        }

        public async Task<int> CreateQualificationAsync(AddQualificationViewModel viewModel)
        {
            var qualification = _mapper.Map<Qualification>(viewModel);
            return await _qualificationRepository.Create(qualification);
        }

        public async Task<QualificationDetailViewModel> GetQualificationAsync(string larId)
        {
            var qualification = await _qualificationRepository.GetSingleOrDefault(p => p.LarsId == larId);
            return _mapper.Map<Qualification, QualificationDetailViewModel>(qualification);
        }

        public async Task<bool> IsValidLarIdAsync(string larId)
        {
            return await Task.FromResult(larId?.Length == 8);
        }
    }
}
