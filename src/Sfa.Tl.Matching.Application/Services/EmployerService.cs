using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.Event;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class EmployerService : IEmployerService
    {
        private readonly IRepository<Employer> _employerRepository;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CrmEmployerEventBase> _employerValidator;

        public EmployerService(IRepository<Employer> employerRepository,
                               IOpportunityRepository opportunityRepository,
                               IMapper mapper,
                               IValidator<CrmEmployerEventBase> employerValidator
                               )
        {
            _employerRepository = employerRepository;
            _opportunityRepository = opportunityRepository;
            _mapper = mapper;
            _employerValidator = employerValidator;
        }

        public async Task<bool> ValidateCompanyNameAndId(int employerId, string companyName)
        {
            if (employerId == 0 || string.IsNullOrEmpty(companyName)) return false;

            var employer = await _employerRepository.GetSingleOrDefault(
                e => e.Id == employerId && companyName.ToLetterOrDigit() == e.CompanyNameSearch,
                e => e.Id);

            return employer > 0;
        }

        public IEnumerable<EmployerSearchResultDto> Search(string companyName)
        {
            var searchResults = _employerRepository
                .GetMany(e => EF.Functions.Like(e.CompanyNameSearch, $"%{companyName.ToLetterOrDigit()}%"))
                .OrderBy(e => e.CompanyName)
                .Select(e => new EmployerSearchResultDto
                {
                    Id = e.Id,
                    CompanyName = e.CompanyName,
                    AlsoKnownAs = e.AlsoKnownAs
                });

            return searchResults;
        }

        public async Task<FindEmployerViewModel> GetOpportunityEmployerAsync(int opportunityId, int opportunityItemId)
        {
            return await _opportunityRepository.GetSingleOrDefault(
                o => o.Id == opportunityId,
                o => new FindEmployerViewModel
                {
                    OpportunityItemId = opportunityItemId,
                    OpportunityId = opportunityId,
                    CompanyName = o.Employer.CompanyName,
                    PreviousCompanyName = o.Employer.CompanyName,
                    AlsoKnownAs = o.Employer.AlsoKnownAs,
                    SelectedEmployerId = o.EmployerId ?? 0,
                });
        }

        public async Task<EmployerDetailsViewModel> GetOpportunityEmployerDetailAsync(int opportunityId, int opportunityItemId)
        {
            var employerId = await _opportunityRepository.GetSingleOrDefault(
                opportunity => opportunity.Id == opportunityId,
                o => o.EmployerId);

            if (employerId == null || employerId <= 0)
                throw new InvalidOperationException("Unable to Find any Employer Details for current Opportunity. Please go back to Find Employer Screen and select an employer");

            //1 first try getting from current Opportunity if its not null
            var employerDetails = await _opportunityRepository.GetSingleOrDefault(
                    o => o.Id == opportunityId &&
                         !string.IsNullOrEmpty(o.EmployerContact) &&
                         !string.IsNullOrEmpty(o.EmployerContactEmail) &&
                         !string.IsNullOrEmpty(o.EmployerContactPhone),
                    o => new EmployerDetailsViewModel
                    {
                        OpportunityItemId = opportunityItemId,
                        OpportunityId = o.Id,
                        CompanyName = o.Employer.CompanyName,
                        CompanyNameAka = o.Employer.AlsoKnownAs,
                        EmployerContact = o.EmployerContact,
                        EmployerContactEmail = o.EmployerContactEmail,
                        EmployerContactPhone = o.EmployerContactPhone
                    },
                    o => o.CreatedOn,
                    false);

            if (employerDetails != null) return employerDetails;

            //2 then try and find from any previous completed Opportunities using employerId
            employerDetails = await _opportunityRepository.GetFirstOrDefault(
                    o => o.EmployerId == employerId &&
                        !string.IsNullOrEmpty(o.EmployerContact) &&
                        !string.IsNullOrEmpty(o.EmployerContactEmail) &&
                        !string.IsNullOrEmpty(o.EmployerContactPhone) &&
                        o.OpportunityItem.Any(oi => oi.OpportunityType == OpportunityType.Referral.ToString() && oi.IsCompleted && oi.IsCompleted),
                    o => new EmployerDetailsViewModel
                    {
                        OpportunityItemId = opportunityItemId,
                        OpportunityId = opportunityId,
                        CompanyName = o.Employer.CompanyName,
                        CompanyNameAka = o.Employer.AlsoKnownAs,
                        EmployerContact = o.EmployerContact,
                        EmployerContactEmail = o.EmployerContactEmail,
                        EmployerContactPhone = o.EmployerContactPhone
                    });

            if (employerDetails != null) return employerDetails;

            //3 Finally we cant find employer details in existing Opportunities so now try to load it from Employer Table 
            return await _employerRepository.GetSingleOrDefault(
                    e => e.Id == employerId,
                    e => new EmployerDetailsViewModel
                    {
                        OpportunityItemId = opportunityItemId,
                        OpportunityId = opportunityId,
                        CompanyName = e.CompanyName,
                        CompanyNameAka = e.AlsoKnownAs,
                        EmployerContact = e.PrimaryContact,
                        EmployerContactEmail = e.Email,
                        EmployerContactPhone = e.Phone
                    });
        }

        public async Task<int> GetInProgressEmployerOpportunityCountAsync(string username)
        {
            var savedCount = await _opportunityRepository.Count(o => o.OpportunityItem.Any(oi => oi.IsSaved &&
                                                                                             !oi.IsCompleted)
                                                                 && o.CreatedBy == username);

            return savedCount;
        }

        public async Task<SavedEmployerOpportunityViewModel> GetSavedEmployerOpportunitiesAsync(string username)
        {
            var employerOpportunities = await _opportunityRepository.GetMany(o => o.OpportunityItem.Any(oi => oi.IsSaved
                                                                                                              && !oi.IsCompleted)
                                                                                  && o.CreatedBy == username)
                .Select(eo => new EmployerOpportunityViewModel
                {
                    Name = eo.Employer.CompanyName,
                    OpportunityId = eo.Id,
                    LastUpdated = eo.ModifiedOn != null ? eo.ModifiedOn.Value.GetTimeWithDate("on") : eo.CreatedOn.GetTimeWithDate("on")
                }).ToListAsync();

            var viewModel = new SavedEmployerOpportunityViewModel
            {
                EmployerOpportunities = employerOpportunities
            };

            return viewModel;
        }

        public async Task<RemoveEmployerDto> GetConfirmDeleteEmployerOpportunity(int opportunityId, string username)
        {
            var opportunityCount = _opportunityRepository.GetEmployerOpportunityCount(opportunityId);
            var employerCount = _opportunityRepository.GetMany(o =>
                o.OpportunityItem.Any(oi => oi.IsSaved && !oi.IsCompleted)
                && o.CreatedBy == username).ToList();

            var removeEmployerDto =
                await _opportunityRepository.GetSingleOrDefault(op => op.Id == opportunityId,
                    op => new RemoveEmployerDto
                    {
                        OpportunityCount = opportunityCount,
                        EmployerName = op.Employer.CompanyName,
                        EmployerCount = employerCount.Count
                    });

            return removeEmployerDto;
        }

        public async Task<string> GetEmployerOpportunityOwnerAsync(int employerId)
        {
            var opportunity = await _opportunityRepository.GetFirstOrDefault(
                o => o.EmployerId == employerId
                     && o.OpportunityItem.Any(oi => oi.IsSaved &&
                                                    !oi.IsCompleted));
            return opportunity?.CreatedBy;
        }

        public async Task<int> HandleEmployerCreatedAsync(string payload)
        {
            var createdEvent = JsonConvert.DeserializeObject<CrmEmployerCreatedEvent>(payload, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore });

            return await CreateOrUpdateEmployerAsync(createdEvent);
        }

        public async Task<int> HandleEmployerUpdatedAsync(string payload)
        {
            var updatedEvent = JsonConvert.DeserializeObject<CrmEmployerUpdatedEvent>(payload, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore });

            return await CreateOrUpdateEmployerAsync(updatedEvent);
        }
        public async Task<int> HandleContactUpdatedAsync(string payload)
        {
            var createdEvent = JsonConvert.DeserializeObject<CrmContactUpdatedEvent>(payload, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore });

            return await CreateOrUpdateContactAsync(createdEvent);
        }

        private async Task<int> CreateOrUpdateEmployerAsync(CrmEmployerEventBase employerData)
        {
            var validationResult = await _employerValidator.ValidateAsync(employerData);

            if (!validationResult.IsValid) return -1;

            var existingEmployer = await _employerRepository.GetSingleOrDefault(emp => emp.CrmId == employerData.accountid.ToGuid());

            if (existingEmployer == null)
            {
                var employer = _mapper.Map<Employer>(employerData);
                return await _employerRepository.Create(employer);
            }

            existingEmployer = _mapper.Map(employerData, existingEmployer);
            await _employerRepository.Update(existingEmployer);
            return 1;

        }
        private async Task<int> CreateOrUpdateContactAsync(CrmContactEventBase employerData)
        {
            if (employerData.parentcustomerid == null) return -1;

            var existingEmployer = await _employerRepository.GetSingleOrDefault(emp => emp.CrmId == employerData.parentcustomerid.id.ToGuid());

            if (existingEmployer == null) return -1;

            existingEmployer.PrimaryContact = employerData.fullname;
            existingEmployer.Phone = employerData.telephone1;
            existingEmployer.Email = employerData.emailaddress1;
            await _employerRepository.Update(existingEmployer);

            return 1;
        }
    }
}