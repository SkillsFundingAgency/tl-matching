using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.EqualityComparer;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class QualificationService : IQualificationService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Qualification> _qualificationRepository;
        private readonly IRepository<QualificationRouteMapping> _qualificationRouteMappingRepository;
        private readonly IRepository<LearningAimReference> _learningAimReferenceRepository;

        public QualificationService(IMapper mapper,
            IRepository<Qualification> qualificationRepository,
            IRepository<QualificationRouteMapping> qualificationRouteMappingRepository,
            IRepository<LearningAimReference> learningAimReferenceRepository)
        {
            _mapper = mapper;
            _qualificationRepository = qualificationRepository;
            _qualificationRouteMappingRepository = qualificationRouteMappingRepository;
            _learningAimReferenceRepository = learningAimReferenceRepository;
        }

        public async Task<int> CreateQualificationEntityAsync(MissingQualificationViewModel viewModel)
        {
            var qualification = _mapper.Map<Qualification>(viewModel);
            var qualificationId = await _qualificationRepository.CreateAsync(qualification);
            return qualificationId;
        }

        public async Task<int> CreateQualificationAsync(MissingQualificationViewModel viewModel)
        {
            var qualification = _mapper.Map<Qualification>(viewModel);

            var qualificationRouteMappings = viewModel
                .Routes?
                .Where(r => r.IsSelected)
                .Select(route => new QualificationRouteMapping
                {
                    RouteId = route.Id,
                    Source = viewModel.Source,
                    Qualification = qualification
                }).ToList();

            if (qualificationRouteMappings?.Count > 0)
            {
                await _qualificationRouteMappingRepository.CreateManyAsync(qualificationRouteMappings);
            }

            return qualification.Id;
        }

        public async Task<QualificationSearchResultViewModel> GetQualificationByIdAsync(int id)
        {
            return await _qualificationRepository.GetSingleOrDefaultAsync(
                p => p.Id == id,
                q => new QualificationSearchResultViewModel
                {
                    QualificationId = q.Id,
                    LarId = q.LarId,
                    ShortTitle = q.ShortTitle,
                    Title = q.Title,
                    RouteIds = q.QualificationRouteMapping.Select(m => m.Id).ToList()
                });
        }

        public async Task<QualificationDetailViewModel> GetQualificationAsync(string larId)
        {
            var qualification = await _qualificationRepository.GetSingleOrDefaultAsync(p => p.LarId == larId);
            return _mapper.Map<Qualification, QualificationDetailViewModel>(qualification);
        }

        public async Task<string> GetLarTitleAsync(string larId)
        {
            var lar = await _learningAimReferenceRepository.GetSingleOrDefaultAsync(l => l.LarId == larId);
            return lar?.Title;
        }

        public async Task<QualificationSearchViewModel> SearchQualificationAsync(string searchTerm)
        {
            var qualificationSearch = searchTerm.ToQualificationSearch();
            if (string.IsNullOrEmpty(qualificationSearch))
                return new QualificationSearchViewModel
                {
                    SearchTerms = searchTerm,
                    HasTooManyResults = true
                };

            var results = await _qualificationRepository.GetManyAsync(q => EF.Functions.Like(q.QualificationSearch, $"%{qualificationSearch}%"))
                .Join(_qualificationRouteMappingRepository.GetManyAsync(), qualification => qualification.Id, qualificationRouteMapping => qualificationRouteMapping.QualificationId, (qualification, qualificationRouteMapping) => new { qualification, qualificationRouteMapping })
                .Select(q => new
                {
                    q.qualification.Id,
                    q.qualification.Title,
                    q.qualification.ShortTitle,
                    q.qualification.LarId,
                    q.qualificationRouteMapping.RouteId
                })
                .OrderBy(q => q.Id)
                .ToListAsync();

            var searchResult = results
                .GroupBy(grp => new { grp.Id, grp.LarId, grp.ShortTitle, grp.Title })
                .Select(grp => new QualificationSearchResultViewModel
                {
                    QualificationId = grp.Key.Id,
                    Title = grp.Key.Title,
                    ShortTitle = grp.Key.ShortTitle,
                    LarId = grp.Key.LarId,
                    RouteIds = grp.Select(g => g.RouteId).ToList()

                })
                .OrderBy(q => q.Title.IndexOf(qualificationSearch, StringComparison.Ordinal))
                .ToList();

            return new QualificationSearchViewModel
            {
                ResultCount = searchResult.Count,
                SearchTerms = searchTerm,
                Results = searchResult.Take(50).ToList()
            };
        }

        public async Task<IList<QualificationShortTitleSearchResultViewModel>> SearchShortTitleAsync(string shortTitle)
        {
            var shortTitleSearch = shortTitle.ToQualificationSearch();

            if (string.IsNullOrEmpty(shortTitleSearch))
                return new List<QualificationShortTitleSearchResultViewModel>();

            var searchResults = await _qualificationRepository
                .GetManyAsync(q => EF.Functions.Like(q.ShortTitleSearch, $"%{shortTitleSearch}%"))
                .Select(q => new QualificationShortTitleSearchResultViewModel
                {
                    ShortTitle = q.ShortTitle
                })
                .Distinct()
                .OrderBy(q => q.ShortTitle.IndexOf(shortTitleSearch, StringComparison.Ordinal))
                .ToListAsync();

            return searchResults;
        }

        public async Task UpdateQualificationAsync(SaveQualificationViewModel viewModel)
        {
            var qualification = await _qualificationRepository.GetSingleOrDefaultAsync(v => v.Id == viewModel.QualificationId);
            qualification = _mapper.Map(viewModel, qualification);
            await _qualificationRepository.UpdateAsync(qualification);

            var existingMappings = _qualificationRouteMappingRepository
                .GetManyAsync(r => r.QualificationId == viewModel.QualificationId)
                .ToList();

            var comparer = new QualificationRouteMappingEqualityComparer();
            var newMappings = _mapper.Map<IList<QualificationRouteMapping>>(viewModel);

            var toBeAdded = newMappings.Except(existingMappings, comparer).ToList();

            var same = existingMappings.Intersect(newMappings, comparer).ToList();
            var toBeDeleted = existingMappings.Except(same).ToList();

            QualificationRouteMapping Find(QualificationRouteMapping qrpm) =>
                existingMappings.First(r => r.Id == qrpm.Id);

            var deleteMappings = toBeDeleted.Select(Find).ToList();
            await _qualificationRouteMappingRepository.DeleteManyAsync(deleteMappings);

            foreach (var toBeAddedItem in toBeAdded)
            {
                await _qualificationRouteMappingRepository.CreateAsync(toBeAddedItem);
            }
        }

        public async Task<bool> IsValidOfqualLarIdAsync(string larId)
        {
            var lar = await _learningAimReferenceRepository.GetSingleOrDefaultAsync(l => l.LarId == larId);
            return lar != null;
        }

        public async Task<bool> IsValidLarIdAsync(string larId)
        {
            return await Task.FromResult(larId?.Length == 8);
        }

        public async Task<int> UpdateQualificationsSearchColumnsAsync()
        {
            var qualificationsFromDb = _qualificationRepository.GetManyAsync()
                .Where(q => string.IsNullOrEmpty(q.ShortTitleSearch) || string.IsNullOrEmpty(q.QualificationSearch))
                .ToList();

            if (qualificationsFromDb.Count > 0)
            {
                foreach (var qualification in qualificationsFromDb)
                {
                    qualification.ShortTitleSearch = GetSearchTerm(qualification.ShortTitle);
                    qualification.QualificationSearch = GetSearchTerm(qualification.Title, qualification.ShortTitle);
                    qualification.ModifiedOn = DateTime.UtcNow;
                    qualification.ModifiedBy = "System";
                }

                await _qualificationRepository.UpdateManyAsync(qualificationsFromDb);
            }

            return qualificationsFromDb.Count;
        }

        private static string GetSearchTerm(params string[] searchTerms)
        {
            var searchTerm = new StringBuilder();
            foreach (var term in searchTerms)
                searchTerm.Append(term.ToQualificationSearch());

            return searchTerm.ToString();
        }
    }
}