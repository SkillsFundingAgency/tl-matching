﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Command;
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
        private readonly IMessageQueueService _messageQueueService;

        public EmployerService(IRepository<Employer> employerRepository,
                               IOpportunityRepository opportunityRepository,
                               IMapper mapper,
                               IValidator<CrmEmployerEventBase> employerValidator,
                               IMessageQueueService messageQueueService
                               )
        {
            _employerRepository = employerRepository;
            _opportunityRepository = opportunityRepository;
            _mapper = mapper;
            _employerValidator = employerValidator;
            _messageQueueService = messageQueueService;
        }

        public async Task<bool> ValidateCompanyNameAndCrmIdAsync(Guid employerCrmId, string companyName)
        {
            if (employerCrmId == Guid.Empty || string.IsNullOrEmpty(companyName)) return false;

            var employer = await _employerRepository.GetSingleOrDefaultAsync(
                e => e.CrmId == employerCrmId && companyName.ToLetterOrDigit() == e.CompanyNameSearch,
                e => e.CrmId);

            return employer != Guid.Empty;
        }

        public IEnumerable<EmployerSearchResultDto> Search(string companyName)
        {
            var searchResults = _employerRepository
                .GetMany(e => EF.Functions.Like(e.CompanyNameSearch, $"%{companyName.ToLetterOrDigit()}%"))
                .OrderBy(e => e.CompanyName)
                .ProjectTo<EmployerSearchResultDto>(_mapper.ConfigurationProvider);

            return searchResults;
        }

        public async Task<FindEmployerViewModel> GetOpportunityEmployerAsync(int opportunityId, int opportunityItemId)
        {
            return await _opportunityRepository.GetSingleOrDefaultAsync(
                o => o.Id == opportunityId,
                o => new FindEmployerViewModel
                {
                    OpportunityItemId = opportunityItemId,
                    OpportunityId = opportunityId,
                    CompanyName = o.Employer.CompanyName,
                    PreviousCompanyName = o.Employer.CompanyName,
                    AlsoKnownAs = o.Employer.AlsoKnownAs,
                    SelectedEmployerCrmId = o.EmployerCrmId ?? Guid.Empty,
                });
        }

        public async Task<EmployerDetailsViewModel> GetOpportunityEmployerDetailAsync(int opportunityId, int opportunityItemId)
        {
            var employerCrmId = await _opportunityRepository.GetSingleOrDefaultAsync(
                opportunity => opportunity.Id == opportunityId,
                o => o.EmployerCrmId);

            if (employerCrmId == null || employerCrmId == Guid.Empty)
                throw new InvalidOperationException("Unable to Find any Employer Details for current Opportunity. Please go back to Find Employer Screen and select an employer");

            //1 first try getting from current Opportunity if its not null
            var employerDetails = await _opportunityRepository.GetSingleOrDefaultAsync(
                    o => o.Id == opportunityId &&
                         !string.IsNullOrEmpty(o.EmployerContact) &&
                         !string.IsNullOrEmpty(o.EmployerContactEmail) &&
                         !string.IsNullOrEmpty(o.EmployerContactPhone),
                    o => new EmployerDetailsViewModel
                    {
                        OpportunityItemId = opportunityItemId,
                        OpportunityId = o.Id,
                        CompanyName = o.Employer.CompanyName,
                        AlsoKnownAs = o.Employer.AlsoKnownAs,
                        PrimaryContact = o.EmployerContact,
                        Email = o.EmployerContactEmail,
                        Phone = o.EmployerContactPhone
                    },
                    o => o.CreatedOn,
                    false);

            if (employerDetails != null) return employerDetails;

            //2 then try and find from any previous completed Opportunities using employerCrmId
            employerDetails = await _opportunityRepository.GetFirstOrDefaultAsync(
                    o => o.EmployerCrmId == employerCrmId &&
                        !string.IsNullOrEmpty(o.EmployerContact) &&
                        !string.IsNullOrEmpty(o.EmployerContactEmail) &&
                        !string.IsNullOrEmpty(o.EmployerContactPhone) &&
                        o.OpportunityItem.Any(oi => oi.OpportunityType == OpportunityType.Referral.ToString() && oi.IsCompleted && oi.IsCompleted),
                    o => new EmployerDetailsViewModel
                    {
                        OpportunityItemId = opportunityItemId,
                        OpportunityId = opportunityId,
                        CompanyName = o.Employer.CompanyName,
                        AlsoKnownAs = o.Employer.AlsoKnownAs,
                        PrimaryContact = o.EmployerContact,
                        Email = o.EmployerContactEmail,
                        Phone = o.EmployerContactPhone
                    });

            if (employerDetails != null) return employerDetails;

            //3 Finally we cant find employer details in existing Opportunities so now try to load it from Employer Table 
            return await _employerRepository.GetSingleOrDefaultAsync(
                    e => e.CrmId == employerCrmId,
                    e => new EmployerDetailsViewModel
                    {
                        OpportunityItemId = opportunityItemId,
                        OpportunityId = opportunityId,
                        CompanyName = e.CompanyName,
                        AlsoKnownAs = e.AlsoKnownAs,
                        PrimaryContact = e.PrimaryContact,
                        Email = e.Email,
                        Phone = e.Phone
                    });
        }

        public async Task<int> GetInProgressEmployerOpportunityCountAsync(string username)
        {
            var savedCount = await _opportunityRepository.CountAsync(o => o.OpportunityItem.Any(oi => oi.IsSaved && !oi.IsCompleted) && o.CreatedBy == username);

            return savedCount;
        }

        public async Task<SavedEmployerOpportunityViewModel> GetSavedEmployerOpportunitiesAsync(string username)
        {
            var employerOpportunities = await _opportunityRepository
                .GetMany(o => o.OpportunityItem.Any(oi => oi.IsSaved && !oi.IsCompleted) && o.CreatedBy == username)
                .ProjectTo<EmployerOpportunityViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var viewModel = new SavedEmployerOpportunityViewModel
            {
                EmployerOpportunities = employerOpportunities
            };

            return viewModel;
        }

        public async Task<RemoveEmployerDto> GetConfirmDeleteEmployerOpportunityAsync(int opportunityId, string username)
        {
            var opportunityCount = _opportunityRepository.GetEmployerOpportunityCount(opportunityId);
            var employerCount = _opportunityRepository.GetMany(o => o.OpportunityItem.Any(oi => oi.IsSaved && !oi.IsCompleted) && o.CreatedBy == username).ToList();

            var removeEmployerDto = await _opportunityRepository.GetSingleOrDefaultAsync(
                    op => op.Id == opportunityId,
                    op => new RemoveEmployerDto
                    {
                        OpportunityCount = opportunityCount,
                        EmployerName = op.Employer.CompanyName,
                        EmployerCount = employerCount.Count
                    });

            return removeEmployerDto;
        }

        public async Task<string> GetEmployerOpportunityOwnerAsync(Guid employerCrmId)
        {
            var opportunity = await _opportunityRepository.GetFirstOrDefaultAsync(o => o.EmployerCrmId == employerCrmId && o.OpportunityItem.Any(oi => oi.IsSaved && !oi.IsCompleted));
            return opportunity?.CreatedBy;
        }

        public async Task<int> HandleEmployerCreatedAsync(string payload)
        {
            var createdEvent = JsonSerializer.Deserialize<CrmEmployerCreatedEvent>(payload,
                new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                });

            return await CreateOrUpdateEmployerAsync(createdEvent);
        }

        public async Task<int> HandleEmployerUpdatedAsync(string payload)
        {
            var updatedEvent = JsonSerializer.Deserialize<CrmEmployerUpdatedEvent>(payload,
                new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                });

            return await CreateOrUpdateEmployerAsync(updatedEvent);
        }

        public async Task<int> HandleContactUpdatedAsync(string payload)
        {
            var createdEvent = JsonSerializer.Deserialize<CrmContactUpdatedEvent>(payload,
                new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                });

            return await CreateOrUpdateContactAsync(createdEvent);
        }

        private async Task<int> CreateOrUpdateEmployerAsync(CrmEmployerEventBase employerData)
        {
            var validationResult = await _employerValidator.ValidateAsync(employerData);

            var isAupaMissing = false;
            if (!validationResult.IsValid)
            {
                isAupaMissing = IsAupaMissing(validationResult.Errors);
                if (!isAupaMissing) return -1;
            }

            if (isAupaMissing)
            {
                var existingReferrals = 
                    await _opportunityRepository.GetFirstOrDefaultAsync(o => 
                        o.EmployerCrmId == employerData.AccountId.ToGuid() && 
                        o.OpportunityItem.Any(oi => oi.Referral.Any()));

                if (existingReferrals == null) return -1;
                await AddMessageToQueueAsync(employerData);

                return -1;
            }

            var existingEmployer = await _employerRepository.GetSingleOrDefaultAsync(emp => emp.CrmId == employerData.AccountId.ToGuid());

            if (existingEmployer == null)
            {
                var employer = _mapper.Map<Employer>(employerData);
                employer.CreatedBy = "System";
                employer.CreatedOn = DateTime.UtcNow;
                return await _employerRepository.CreateAsync(employer);
            }

            existingEmployer = _mapper.Map(employerData, existingEmployer);
            existingEmployer.ModifiedBy = "System";
            existingEmployer.ModifiedOn = DateTime.UtcNow;
            await _employerRepository.UpdateAsync(existingEmployer);

            return 1;
        }

        private async Task AddMessageToQueueAsync(CrmEmployerEventBase employerData)
        {
            await _messageQueueService.PushEmployerAupaBlankEmailMessageAsync(new SendEmployerAupaBlankEmail
            {
                Name = employerData.Name,
                Owner = employerData.OwnerIdName,
                ContactEmail = employerData.ContactEmail,
                ContactPhone = employerData.ContactTelephone1,
                CrmId = new Guid(employerData.AccountId)
            });
        }

        private async Task<int> CreateOrUpdateContactAsync(CrmContactEventBase employerData)
        {
            if (employerData.ParentCustomerId == null) return -1;

            var existingEmployer = await _employerRepository.GetSingleOrDefaultAsync(emp => emp.CrmId == employerData.ParentCustomerId.Id.ToGuid());

            if (existingEmployer == null) return -1;

            existingEmployer.PrimaryContact = employerData.Fullname;
            existingEmployer.Phone = employerData.Telephone;
            existingEmployer.Email = employerData.EmailAddress;
            existingEmployer.ModifiedBy = "System";
            existingEmployer.ModifiedOn = DateTime.UtcNow;

            await _employerRepository.UpdateAsync(existingEmployer);

            return 1;
        }

        private static bool IsAupaMissing(IEnumerable<ValidationFailure> validationFailures)
        {
            return validationFailures.Any(vf => vf.PropertyName == "sfa_aupa"
                                                  && (vf.ErrorCode == "InvalidFormat"
                                                      || vf.ErrorCode == "MissingMandatoryData"));
        }
    }
}