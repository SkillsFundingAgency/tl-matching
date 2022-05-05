using System.Collections.Generic;
using System.Linq;
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
                .ForMember(m => m.QualificationSearch, config => config.MapFrom(s => new[]
                {
                    s.Title,
                    s.ShortTitle.ToLower()
                }.GetSearchTerm()))
                .ForMember(m => m.ShortTitleSearch, config => config.MapFrom(s => new[]
                {
                    s.ShortTitle.ToLower()
                }.GetSearchTerm()))
                .ForMember(m => m.ProviderQualification, config => config.Ignore())
                .ForMember(m => m.QualificationRouteMapping, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                .ForMember(m => m.IsDeleted, config => config.Ignore())
                ;

            CreateMap<AddQualificationViewModel, Qualification>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.LarId, config => config.MapFrom(s => s.LarId))
                .ForMember(m => m.Title, config => config.Ignore())
                .ForMember(m => m.ShortTitle, config => config.Ignore())
                .ForMember(m => m.QualificationSearch, config => config.Ignore())
                .ForMember(m => m.ShortTitleSearch, config => config.Ignore())
                .ForMember(m => m.ProviderQualification, config => config.Ignore())
                .ForMember(m => m.QualificationRouteMapping, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<AddQualificationViewModel, Qualification>>())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                .ForMember(m => m.IsDeleted, config => config.Ignore())
                ;

            CreateMap<MissingQualificationViewModel, Qualification>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.LarId, config => config.MapFrom(s => s.LarId))
                .ForMember(m => m.ShortTitle, config => config.MapFrom(s => s.ShortTitle.ToLower()))
                .ForMember(m => m.QualificationSearch, config => config.MapFrom(s => new[]
                {
                    s.Title,
                    s.ShortTitle.ToLower()
                }.GetSearchTerm()))
                .ForMember(m => m.ShortTitleSearch, config => config.MapFrom(s => new[]
                {
                    s.ShortTitle.ToLower()
                }.GetSearchTerm()))
                .ForMember(m => m.ProviderQualification, config => config.Ignore())
                .ForMember(m => m.QualificationRouteMapping, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<MissingQualificationViewModel, Qualification>>())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                .ForMember(m => m.IsDeleted, config => config.Ignore())
                ;

            CreateMap<SaveQualificationViewModel, Qualification>()
                .ForMember(m => m.Id, config => config.MapFrom(s => s.QualificationId))
                .ForMember(m => m.ShortTitle, config => config.MapFrom(s => s.ShortTitle.ToLower()))
                .ForMember(m => m.QualificationSearch, config => config.MapFrom(s => new[]
                {
                    s.Title, 
                    s.ShortTitle.ToLower()
                }.GetSearchTerm()))
                .ForMember(m => m.ShortTitleSearch, config => config.MapFrom(s => new[]
                {
                    s.ShortTitle.ToLower()
                }.GetSearchTerm()))
                .ForMember(m => m.ModifiedBy, config => config.MapFrom<LoggedInUserNameResolver<SaveQualificationViewModel, Qualification>>())
                .ForMember(m => m.ModifiedOn, config => config.MapFrom<UtcNowResolver<SaveQualificationViewModel, Qualification>>())
                .ForMember(m => m.IsDeleted, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForPath(m => m.ProviderQualification, config => config.Ignore())
                .ForPath(m => m.QualificationRouteMapping, config => config.Ignore())
            ;

            CreateMap<SaveQualificationViewModel, IList<QualificationRouteMapping>>()
                .ConvertUsing((m, _, context) =>
                {
                    var userNameResolver = context.Options.ServiceCtor(typeof(LoggedInUserNameResolver<SaveQualificationViewModel, Qualification>))
                        as LoggedInUserNameResolver<SaveQualificationViewModel, Qualification>;

                    return m.Routes == null ?
                        new List<QualificationRouteMapping>() :
                        m.Routes
                            .Where(r => r.IsSelected)
                            .Select(route => new QualificationRouteMapping
                            {
                                QualificationId = m.QualificationId,
                                RouteId = route.Id,
                                Source = m.Source,
                                CreatedBy = userNameResolver?.Resolve(m, null, "CreatedBy", context)
                            })
                            .ToList();
                });

            CreateMap<Qualification, QualificationDetailViewModel>();
        }
    }
}