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
        private readonly IRepository<QualificationRoutePathMapping> _qualificationRoutePathMappingRepository;
        private readonly IRepository<LearningAimReference> _learningAimReferenceRepository;

        public QualificationService(IMapper mapper,
            IRepository<Qualification> qualificationRepository,
            IRepository<QualificationRoutePathMapping> qualificationRoutePathMappingRepository,
            IRepository<LearningAimReference> learningAimReferenceRepository)
        {
            _mapper = mapper;
            _qualificationRepository = qualificationRepository;
            _qualificationRoutePathMappingRepository = qualificationRoutePathMappingRepository;
            _learningAimReferenceRepository = learningAimReferenceRepository;
        }

        public async Task<int> CreateQualificationAsync(MissingQualificationViewModel viewModel)
        {
            var qualification = _mapper.Map<Qualification>(viewModel);

            var qualificationRoutePathMappings = viewModel
                .Routes?
                .Where(r => r.IsSelected)
                .Select(route => new QualificationRoutePathMapping
                {
                    RouteId = route.Id,
                    Source = viewModel.Source,
                    Qualification = qualification
                }).ToList();

            if (qualificationRoutePathMappings?.Count > 0)
            {
                await _qualificationRoutePathMappingRepository.CreateMany(qualificationRoutePathMappings);
            }

            return qualification.Id;
        }

        public async Task<QualificationSearchResultViewModel> GetQualificationAsync(int id)
        {
            var qualification = await _qualificationRepository
                .GetSingleOrDefault(p => p.Id == id,
                    q => q.QualificationRoutePathMapping);

            return _mapper.Map<Qualification, QualificationSearchResultViewModel>(qualification);
        }

        public async Task<QualificationDetailViewModel> GetQualificationAsync(string larId)
        {
            var qualification = await _qualificationRepository.GetSingleOrDefault(p => p.LarsId == larId);
            return _mapper.Map<Qualification, QualificationDetailViewModel>(qualification);
        }

        public async Task<string> GetLarTitleAsync(string larId)
        {
            var lar = await _learningAimReferenceRepository.GetSingleOrDefault(l => l.LarId == larId);
            return lar?.Title;
        }

        public async Task<QualificationSearchViewModel> SearchQualification(string searchTerm)
        {
            var qualificationSearch = searchTerm.ToQualificationSearch();
            if (string.IsNullOrEmpty(qualificationSearch))
                return new QualificationSearchViewModel
                {
                    SearchTerms = searchTerm,
                    HasTooManyResults = true
                };

            var searchCount = await _qualificationRepository.Count(q => EF.Functions.Like(q.QualificationSearch, $"%{qualificationSearch}%"));
            if (searchCount == 0)
                return new QualificationSearchViewModel
                {
                    SearchTerms = searchTerm
                };

            var searchResults = new QualificationSearchViewModel
            {
                ResultCount = searchCount,
                SearchTerms = searchTerm,
                Results = await _qualificationRepository
                    .GetMany(q => EF.Functions.Like(q.QualificationSearch, $"%{qualificationSearch}%"))
                    .OrderBy(q => q.Title)
                    .Select(q => new QualificationSearchResultViewModel
                    {
                        QualificationId = q.Id,
                        Title = q.Title,
                        ShortTitle = q.ShortTitle,
                        LarId = q.LarsId,
                        RouteIds = q.QualificationRoutePathMapping.Select(r => r.RouteId).ToList()
                    })
                    .Take(50)
                    .ToListAsync()
            };

            return searchResults;
        }

        public IEnumerable<QualificationShortTitleSearchResultViewModel> SearchShortTitle(string shortTitle)
        {
            var shortTitleSearch = shortTitle.ToQualificationSearch();

            var searchResults = _qualificationRepository
                .GetMany(q => EF.Functions.Like(q.ShortTitleSearch, $"%{shortTitleSearch}%"))
                .OrderBy(q => q.ShortTitle)
                .Select(q => new QualificationShortTitleSearchResultViewModel
                {
                    ShortTitle = q.ShortTitle
                })
                .Distinct();

            return searchResults;
        }

        public async Task UpdateQualificationAsync(SaveQualificationViewModel viewModel)
        {
            var qualification = await _qualificationRepository.GetSingleOrDefault(v => v.Id == viewModel.QualificationId);
            qualification = _mapper.Map(viewModel, qualification);
            await _qualificationRepository.Update(qualification);

            var existingMappings = _qualificationRoutePathMappingRepository
                .GetMany(r => r.Id == viewModel.QualificationId)
                .ToList();

            var comparer = new QualificationRoutePathMappingEqualityComparer();
            var newMappings = _mapper.Map<IList<QualificationRoutePathMapping>>(viewModel);
            
            var toBeAdded = newMappings.Except(existingMappings, comparer).ToList();
            var same = existingMappings.Intersect(newMappings, comparer).ToList();
            var toBeDeleted = existingMappings?.Except(same).ToList();

            QualificationRoutePathMapping Find(QualificationRoutePathMapping qrpm) => 
                existingMappings?.First(r => r.Id == qrpm.Id);

            var deleteMappings = toBeDeleted?.Select(Find).ToList();
            await _qualificationRoutePathMappingRepository.DeleteMany(deleteMappings);

            await _qualificationRoutePathMappingRepository.CreateMany(toBeAdded);
        }

        public async Task<bool> IsValidOfqualLarIdAsync(string larId)
        {
            var lar = await _learningAimReferenceRepository.GetSingleOrDefault(l => l.LarId == larId);
            return lar != null;
        }

        public async Task<bool> IsValidLarIdAsync(string larId)
        {
            return await Task.FromResult(larId?.Length == 8);
        }

        #region TODO DELETE AFTER SPRINT 10
        // TODO DELETE AFTER SPRINT 10
        public async Task<int> UpdateQualificationsSearchColumns()
        {
            var qualificationsFromDb = _qualificationRepository.GetMany()
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

                await _qualificationRepository.UpdateMany(qualificationsFromDb);
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
        #endregion
    }
}