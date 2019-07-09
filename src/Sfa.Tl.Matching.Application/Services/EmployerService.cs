using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class EmployerService : IEmployerService
    {
        private readonly IRepository<Employer> _employerRepository;
        private readonly IOpportunityRepository _opportunityRepository;

        public EmployerService(IRepository<Employer> employerRepository,
            IOpportunityRepository opportunityRepository)
        {
            _employerRepository = employerRepository;
            _opportunityRepository = opportunityRepository;
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
                oi => new FindEmployerViewModel
                {
                    OpportunityItemId = opportunityItemId,
                    OpportunityId = opportunityId,
                    CompanyName = oi.Employer.CompanyName,
                    PreviousCompanyName = oi.Employer.CompanyName,
                    AlsoKnownAs = oi.Employer.AlsoKnownAs,
                    SelectedEmployerId = oi.EmployerId ?? 0,
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
    }
}