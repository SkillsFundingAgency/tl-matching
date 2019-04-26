using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderFeedbackService : IProviderFeedbackService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Provider> _repository;

        public ProviderFeedbackService(IMapper mapper, IRepository<Provider> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public Task UpdateProviderFeedback(SaveProviderFeedbackViewModel viewModel)
        {
            var providers = _mapper.Map<IList<Provider>>(viewModel.Providers);

            return _repository.UpdateManyWithSpecifedColumnsOnly(providers, x => x.IsFundedForNextYear);
        }
    }
}