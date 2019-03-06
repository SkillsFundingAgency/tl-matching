using System.Threading.Tasks;
using AutoMapper;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class OpportunityService : IOpportunityService
    {
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IRepository<Opportunity> _opportunityRepository;
        private readonly IRepository<ProvisionGap> _provisionGapRepository;
        private readonly IRepository<Referral> _referralRepository;

        public OpportunityService(
            IMapper mapper,
            IDateTimeProvider dateTimeProvider,
            IRepository<Opportunity> opportunityRepository,
            IRepository<ProvisionGap> provisionGapRepository,
            IRepository<Referral> referralRepository)
        {
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
            _opportunityRepository = opportunityRepository;
            _provisionGapRepository = provisionGapRepository;
            _referralRepository = referralRepository;
        }

        public async Task<int> CreateOpportunity(OpportunityDto dto)
        {
            var opportunity = _mapper.Map<Opportunity>(dto);

            return await _opportunityRepository.Create(opportunity);
        }

        public async Task<OpportunityDto> GetOpportunity(int id)
        {
            var opportunity = await _opportunityRepository.GetSingleOrDefault(o => o.Id == id);
            var dto = _mapper.Map<Opportunity, OpportunityDto>(opportunity);

            return dto;
        }

        public async Task<OpportunityDto> GetOpportunityWithRoute(int id)
        {
            var opportunity = await _opportunityRepository.GetSingleOrDefault(o => o.Id == id,
                opp => opp.Route);
            var dto = _mapper.Map<Opportunity, OpportunityDto>(opportunity);

            return dto;
        }
        
        public async Task SavePlacementInformation(PlacementInformationViewModel dto)
        {
            var opportunity = await _opportunityRepository.GetSingleOrDefault(o => o.Id == dto.OpportunityId);
            var updatedOpportunity = _mapper.Map(dto, opportunity);

            updatedOpportunity.ModifiedOn = _dateTimeProvider.UtcNow();

            await _opportunityRepository.Update(updatedOpportunity);
        }

        public async Task UpdateOpportunity(OpportunityDto dto)
        {
            dto.ModifiedOn = _dateTimeProvider.UtcNow();

            var trackedEntity = await _opportunityRepository.GetSingleOrDefault(o => o.Id == dto.Id);

            _mapper.Map(dto, trackedEntity);

            await _opportunityRepository.Update(trackedEntity);
        }

        public Task<int> CreateProvisionGap(CheckAnswersGapViewModel dto)
        {
            var provisionGap = _mapper.Map<ProvisionGap>(dto);

            return _provisionGapRepository.Create(provisionGap);
        }

        public Task<int> CreateReferral(CheckAnswersViewModel dto)
        {
            var referral = _mapper.Map<Referral>(dto);

            return _referralRepository.Create(referral);
        }
    }
}