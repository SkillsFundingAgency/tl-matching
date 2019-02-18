using System;
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
        private readonly IRepository<Opportunity> _repository;

        public OpportunityService(
            IMapper mapper,
            IRepository<Opportunity> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<int> CreateOpportunity(OpportunityDto opportunityDto)
        {
            var opportunity = _mapper.Map<Opportunity>(opportunityDto);
            return await _repository.Create(opportunity);
        }

        public void UpdateOpportunity()
        {
            throw new NotImplementedException();
        }
    }
}