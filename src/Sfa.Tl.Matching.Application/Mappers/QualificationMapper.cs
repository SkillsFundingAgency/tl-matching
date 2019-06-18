using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class QualificationMapper : Profile
    {
        public QualificationMapper()
        {
            CreateMap<QualificationDto, Qualification>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.QualificationSearch, config => config.MapFrom(s => GetSearchTerm(s.Title, s.ShortTitle)))
                .ForMember(m => m.ShortTitleSearch, config => config.MapFrom(s => GetSearchTerm(s.ShortTitle)))
                .ForMember(m => m.ProviderQualification, config => config.Ignore())
                .ForMember(m => m.QualificationRoutePathMapping, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                ;

            CreateMap<AddQualificationViewModel, Qualification>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.LarsId, config => config.MapFrom(s => s.LarId))
                .ForMember(m => m.Title, config => config.Ignore())
                .ForMember(m => m.ShortTitle, config => config.Ignore())
                .ForMember(m => m.QualificationSearch, config => config.Ignore())
                .ForMember(m => m.ShortTitleSearch, config => config.Ignore())
                .ForMember(m => m.ProviderQualification, config => config.Ignore())
                .ForMember(m => m.QualificationRoutePathMapping, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<AddQualificationViewModel, Qualification>>())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                ;

            CreateMap<MissingQualificationViewModel, Qualification>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.LarsId, config => config.MapFrom(s => s.LarId))
                .ForMember(m => m.ShortTitle, config => config.MapFrom(s => s.ShortTitle.ToLower()))
                .ForMember(m => m.QualificationSearch, config => config.MapFrom(s => GetSearchTerm(s.Title, s.ShortTitle)))
                .ForMember(m => m.ShortTitleSearch, config => config.MapFrom(s => GetSearchTerm(s.ShortTitle)))
                .ForMember(m => m.ProviderQualification, config => config.Ignore())
                .ForMember(m => m.QualificationRoutePathMapping, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<MissingQualificationViewModel, Qualification>>())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                ;

            CreateMap<SaveQualificationViewModel, Qualification>()
                .ForMember(m => m.Id, config => config.MapFrom(s => s.QualificationId))
                .ForMember(m => m.ShortTitle, config => config.MapFrom(s => s.ShortTitle.ToLower()))
                .ForMember(m => m.QualificationSearch, config => config.MapFrom(s => GetSearchTerm(s.Title, s.ShortTitle)))
                .ForMember(m => m.ShortTitleSearch, config => config.MapFrom(s => GetSearchTerm(s.ShortTitle)))
                .ForMember(m => m.ModifiedBy, config => config.MapFrom<LoggedInUserNameResolver<SaveQualificationViewModel, Qualification>>())
                .ForMember(m => m.ModifiedOn, config => config.MapFrom<UtcNowResolver<SaveQualificationViewModel, Qualification>>())
                .ForAllOtherMembers(o => o.Ignore())
                ;

            CreateMap<SaveQualificationViewModel, IList<QualificationRoutePathMapping>>()
                .ConstructUsing((m, context) =>
                {
                    var userNameResolver = context.Mapper.ServiceCtor(typeof(LoggedInUserNameResolver<SaveQualificationViewModel, Qualification>))
                        as LoggedInUserNameResolver<SaveQualificationViewModel, Qualification>;

                    return m.Routes == null ? 
                        new List<QualificationRoutePathMapping>() :
                        m.Routes.Where(r => r.IsSelected)
                        .Select(route => new QualificationRoutePathMapping
                        {
                            QualificationId = m.QualificationId,
                            RouteId = route.Id,
                            Source = m.Source,
                            CreatedBy = userNameResolver?.Resolve(m, null, "CreatedBy", context)
                        })
                        .ToList();
                })
                ;

            CreateMap<Qualification, QualificationSearchResultViewModel>()
                .ForMember(m => m.QualificationId, config => config.MapFrom(s => s.Id))
                .ForMember(m => m.LarId, config => config.MapFrom(s => s.LarsId))
                .ForMember(m => m.Title, config => config.MapFrom(s => s.Title))
                .ForMember(m => m.ShortTitle, config => config.MapFrom(s => s.ShortTitle))
                .ForMember(m => m.RouteIds, config => config.Ignore())
                .ForMember(m => m.Routes, config => config.Ignore())
                .AfterMap((m, o) =>
                {
                    o.RouteIds = m.QualificationRoutePathMapping.Select(r => r.RouteId).ToList();
                })
                ;
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