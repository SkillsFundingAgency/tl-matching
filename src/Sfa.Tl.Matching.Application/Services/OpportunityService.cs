using System.Threading.Tasks;
using AutoMapper;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Services
{
    public class OpportunityService : IOpportunityService
    {
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IRepository<Opportunity> _repository;

        public OpportunityService(
            IMapper mapper,
            IDateTimeProvider dateTimeProvider,
            IRepository<Opportunity> repository)
        {
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
            _repository = repository;
        }

        public async Task<int> CreateOpportunity(OpportunityDto dto)
        {
            var opportunity = _mapper.Map<Opportunity>(dto);

            return await _repository.Create(opportunity);
        }

        public async Task<OpportunityDto> GetOpportunity(int id)
        {
            var opportunity = await _repository.GetSingleOrDefault(o => o.Id == id);
            var dto = _mapper.Map<Opportunity, OpportunityDto>(opportunity);

            return dto;
        }

        public async Task UpdateOpportunity(OpportunityDto dto)
        {
            dto.ModifiedOn = _dateTimeProvider.UtcNow();

            var opportunity = _mapper.Map<Opportunity>(dto);
            await _repository.Update(opportunity);
        }
    }
}