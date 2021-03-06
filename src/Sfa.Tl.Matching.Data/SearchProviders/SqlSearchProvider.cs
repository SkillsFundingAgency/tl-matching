﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Data.SearchProviders
{
    public class SqlSearchProvider : ISearchProvider
    {
        public const double MilesToMeters = 1609.34d;

        private readonly ILogger<SqlSearchProvider> _logger;
        private readonly MatchingDbContext _matchingDbContext;

        public SqlSearchProvider(ILogger<SqlSearchProvider> logger, MatchingDbContext matchingDbContext)
        {
            _logger = logger;
            _matchingDbContext = matchingDbContext;
        }

        public async Task<IList<OpportunityProximitySearchResultViewModelItem>> SearchOpportunitiesByPostcodeProximityAsync(OpportunityProximitySearchParametersDto dto)
        {
            _logger.LogInformation($"Searching for opportunities within radius {dto.SearchRadius} of postcode '{dto.Postcode}' with route {dto.SelectedRouteId}");

            var employerLocation = GetSearchStartPoint(dto.Latitude, dto.Longitude);

            var searchRadiusInMeters = dto.SearchRadius * MilesToMeters;

            var result = await (from provider in _matchingDbContext.Provider
                                join providerVenue in _matchingDbContext.ProviderVenue on provider.Id equals providerVenue.ProviderId
                                join providerQualification in _matchingDbContext.ProviderQualification on providerVenue.Id equals providerQualification.ProviderVenueId
                                join qualificationRouteMapping in _matchingDbContext.QualificationRouteMapping on providerQualification.QualificationId equals qualificationRouteMapping.QualificationId
                                join qualification in _matchingDbContext.Qualification on qualificationRouteMapping.QualificationId equals qualification.Id
                                orderby providerVenue.Location.Distance(employerLocation)
                                where qualificationRouteMapping.RouteId == dto.SelectedRouteId
                                      && providerVenue.Location.Distance(employerLocation) <= searchRadiusInMeters
                                      && provider.IsCdfProvider
                                      && provider.IsEnabledForReferral
                                      && providerVenue.IsEnabledForReferral
                                      && !providerVenue.IsRemoved
                                select new
                                {
                                    ProviderVenueId = providerVenue.Id,
                                    ProviderName = provider.Name,
                                    ProviderDisplayName = provider.DisplayName,
                                    ProviderVenueName = providerVenue.Name,
                                    Distance = providerVenue.Location.Distance(employerLocation) / MilesToMeters,
                                    providerVenue.Postcode,
                                    providerVenue.Town,
                                    providerVenue.Latitude,
                                    providerVenue.Longitude,
                                    provider.IsTLevelProvider,
                                    QualificationShortTitle = qualification.ShortTitle
                                }).Distinct().ToListAsync();

            return result.GroupBy(grp => new
            {
                grp.ProviderVenueId,
                grp.ProviderName,
                grp.ProviderDisplayName,
                grp.ProviderVenueName,
                grp.Distance,
                grp.Postcode,
                grp.Town,
                grp.Latitude,
                grp.Longitude,
                grp.IsTLevelProvider
            })
                .Select(g => new OpportunityProximitySearchResultViewModelItem
                {
                    ProviderVenueTown = g.Key.Town,
                    ProviderVenuePostcode = g.Key.Postcode,
                    ProviderVenueId = g.Key.ProviderVenueId,
                    ProviderName = g.Key.ProviderName,
                    ProviderDisplayName = g.Key.ProviderDisplayName,
                    ProviderVenueName = g.Key.ProviderVenueName,
                    Distance = g.Key.Distance,
                    IsTLevelProvider = g.Key.IsTLevelProvider,
                    Latitude = g.Key.Latitude ?? 0,
                    Longitude = g.Key.Longitude ?? 0,
                    QualificationShortTitles = g.Select(q => q.QualificationShortTitle)
                }).OrderBy(r => r.Distance).ToList();
        }

        public async Task<IList<OpportunityProximitySearchResultByRouteViewModelItem>> SearchOpportunitiesForOtherRoutesByPostcodeProximityAsync(OpportunityProximitySearchParametersDto dto)
        {
            _logger.LogInformation($"Searching for opportunities within radius {dto.SearchRadius} of postcode '{dto.Postcode}' with route other than {dto.SelectedRouteId}");

            var employerLocation = GetSearchStartPoint(dto.Latitude, dto.Longitude);

            var searchRadiusInMeters = dto.SearchRadius * MilesToMeters;

            var result = await (from provider in _matchingDbContext.Provider
                                join providerVenue in _matchingDbContext.ProviderVenue on provider.Id equals providerVenue.ProviderId
                                join providerQualification in _matchingDbContext.ProviderQualification on providerVenue.Id equals providerQualification.ProviderVenueId
                                join qualificationRouteMapping in _matchingDbContext.QualificationRouteMapping on providerQualification.QualificationId equals qualificationRouteMapping.QualificationId
                                join route in _matchingDbContext.Route on qualificationRouteMapping.RouteId equals route.Id
                                orderby route.Name
                                where qualificationRouteMapping.RouteId != dto.SelectedRouteId
                                      && providerVenue.Location.Distance(employerLocation) <= searchRadiusInMeters
                                      && provider.IsCdfProvider
                                      && provider.IsEnabledForReferral
                                      && providerVenue.IsEnabledForReferral
                                      && !providerVenue.IsRemoved
                                select new
                                {
                                    ProviderVenueId = providerVenue.Id,
                                    RouteName = route.Name
                                }).Distinct().ToListAsync();

            return result
                .GroupBy(r => r.RouteName)
                .Select(rg => new OpportunityProximitySearchResultByRouteViewModelItem
                {
                    RouteName = rg.Key.ToLower(),
                    NumberOfResults = rg.Count()
                }).OrderBy(r => r.RouteName).ToList();
        }

        public async Task<IList<ProviderProximitySearchResultViewModelItem>> SearchProvidersByPostcodeProximityAsync(ProviderProximitySearchParametersDto dto)
        {
            _logger.LogInformation($"Searching for providers within radius {dto.SearchRadius} of postcode '{dto.Postcode}'");

            var employerLocation = GetSearchStartPoint(dto.Latitude, dto.Longitude);

            var searchRadiusInMeters = dto.SearchRadius * MilesToMeters;

            var resultTemp = await (from provider in _matchingDbContext.Provider
                                    join providerVenue in _matchingDbContext.ProviderVenue on provider.Id equals providerVenue.ProviderId
                                    join providerQualification in _matchingDbContext.ProviderQualification on providerVenue.Id equals providerQualification.ProviderVenueId
                                    join routePathMapping in _matchingDbContext.QualificationRouteMapping on providerQualification.QualificationId equals routePathMapping.QualificationId
                                    join qualification in _matchingDbContext.Qualification on routePathMapping.QualificationId equals qualification.Id
                                    join route in _matchingDbContext.Route on routePathMapping.RouteId equals route.Id
                                    orderby providerVenue.Location.Distance(employerLocation)
                                    where providerVenue.Location.Distance(employerLocation) <= searchRadiusInMeters
                                                      && provider.IsCdfProvider
                                                      && provider.IsEnabledForReferral
                                                      && providerVenue.IsEnabledForReferral
                                                      && !providerVenue.IsRemoved
                                    select new
                                    {
                                        ProviderVenueId = providerVenue.Id,
                                        ProviderName = provider.Name,
                                        ProviderDisplayName = provider.DisplayName,
                                        ProviderVenueName = providerVenue.Name,
                                        Distance = providerVenue.Location.Distance(employerLocation) / MilesToMeters,
                                        ProviderVenuePostcode = providerVenue.Postcode,
                                        ProviderVenueTown = providerVenue.Town,
                                        Latitude = providerVenue.Latitude ?? 0,
                                        Longitude = providerVenue.Longitude ?? 0,
                                        provider.IsTLevelProvider,
                                        RouteName = route.Name,
                                        RouteId = route.Id,
                                        QualificationShortTitle = qualification.ShortTitle
                                    }).Distinct().ToListAsync();

            var result = resultTemp.GroupBy(g => new
            {
                g.Distance,
                g.IsTLevelProvider,
                g.Latitude,
                g.Longitude,
                g.ProviderDisplayName,
                g.ProviderName,
                g.ProviderVenueId,
                g.ProviderVenueName,
                g.ProviderVenuePostcode,
                g.ProviderVenueTown
            })
            .Select(grp => new ProviderProximitySearchResultViewModelItem
            {
                Latitude = grp.Key.Latitude,
                Distance = grp.Key.Distance,
                ProviderVenueTown = grp.Key.ProviderVenueTown,
                ProviderName = grp.Key.ProviderName,
                ProviderVenuePostcode = grp.Key.ProviderVenuePostcode,
                ProviderVenueId = grp.Key.ProviderVenueId,
                ProviderDisplayName = grp.Key.ProviderDisplayName,
                ProviderVenueName = grp.Key.ProviderVenueName,
                IsTLevelProvider = grp.Key.IsTLevelProvider,
                Longitude = grp.Key.Longitude,
                Routes = grp.GroupBy(rt => new { rt.RouteName, rt.RouteId })
                        .Select(rt => new RouteAndQualificationsViewModel
                        {
                            RouteName = rt.Key.RouteName,
                            RouteId = rt.Key.RouteId,
                            QualificationShortTitles = rt.Select(q =>
                                q.QualificationShortTitle)
                                .OrderBy(q => q)
                        }).OrderBy(rt => rt.RouteName)
            }).OrderBy(r => r.Distance)
            .ThenBy(r =>
            {
                if (r.ProviderVenueName == r.ProviderVenuePostcode)
                    return r.ProviderDisplayName;
                else
                    return r.ProviderVenueName;
            })
            .ToList();

            return result;
        }

        public async Task<ProviderProximityReportDto> SearchProvidersByPostcodeProximityForReportAsync(ProviderProximitySearchParametersDto searchParameters)
        {
            _logger.LogInformation($"Searching for providers within radius {searchParameters.SearchRadius} of postcode '{searchParameters.Postcode}'");

            var employerLocation = GetSearchStartPoint(searchParameters.Latitude, searchParameters.Longitude);

            var searchRadiusInMeters = searchParameters.SearchRadius * MilesToMeters;

            var resultTemp = await (from provider in _matchingDbContext.Provider
                                    join providerVenue in _matchingDbContext.ProviderVenue on provider.Id equals providerVenue.ProviderId
                                    join providerQualification in _matchingDbContext.ProviderQualification on providerVenue.Id equals providerQualification.ProviderVenueId
                                    join routePathMapping in _matchingDbContext.QualificationRouteMapping on providerQualification.QualificationId equals routePathMapping.QualificationId
                                    join qualification in _matchingDbContext.Qualification on routePathMapping.QualificationId equals qualification.Id
                                    join route in _matchingDbContext.Route on routePathMapping.RouteId equals route.Id
                                    orderby providerVenue.Location.Distance(employerLocation)
                                    where providerVenue.Location.Distance(employerLocation) <= searchRadiusInMeters
                                                      && provider.IsCdfProvider
                                                      && provider.IsEnabledForReferral
                                                      && providerVenue.IsEnabledForReferral
                                                      && !providerVenue.IsRemoved
                                    select new
                                    {
                                        ProviderVenueId = providerVenue.Id,
                                        ProviderName = provider.Name,
                                        ProviderDisplayName = provider.DisplayName,
                                        provider.PrimaryContact,
                                        provider.PrimaryContactEmail,
                                        provider.PrimaryContactPhone,
                                        provider.SecondaryContact,
                                        provider.SecondaryContactEmail,
                                        provider.SecondaryContactPhone,
                                        ProviderVenueName = providerVenue.Name,
                                        Distance = providerVenue.Location.Distance(employerLocation) / MilesToMeters,
                                        ProviderVenuePostcode = providerVenue.Postcode,
                                        ProviderVenueTown = providerVenue.Town,
                                        Latitude = providerVenue.Latitude ?? 0,
                                        Longitude = providerVenue.Longitude ?? 0,
                                        provider.IsTLevelProvider,
                                        RouteName = route.Name,
                                        RouteId = route.Id,
                                        QualificationShortTitle = qualification.ShortTitle
                                    }).Distinct().ToListAsync();

            var result = resultTemp.GroupBy(g => new
            {
                g.Distance,
                g.IsTLevelProvider,
                g.Latitude,
                g.Longitude,
                g.ProviderDisplayName,
                g.PrimaryContact,
                g.PrimaryContactEmail,
                g.PrimaryContactPhone,
                g.SecondaryContact,
                g.SecondaryContactEmail,
                g.SecondaryContactPhone,
                g.ProviderVenueId,
                g.ProviderVenueName,
                g.ProviderVenuePostcode,
                g.ProviderVenueTown
            })
            .Select(grp => new ProviderProximityReportItemDto
            {
                Distance = grp.Key.Distance,
                ProviderVenueTown = grp.Key.ProviderVenueTown,
                ProviderVenuePostcode = grp.Key.ProviderVenuePostcode,
                ProviderDisplayName = grp.Key.ProviderDisplayName,
                ProviderVenueName = grp.Key.ProviderVenueName,
                PrimaryContact = grp.Key.PrimaryContact,
                PrimaryContactEmail = grp.Key.PrimaryContactEmail,
                PrimaryContactPhone = grp.Key.PrimaryContactPhone,
                SecondaryContact = grp.Key.SecondaryContact,
                SecondaryContactEmail = grp.Key.SecondaryContactEmail,
                SecondaryContactPhone = grp.Key.SecondaryContactPhone,
                Routes = grp.GroupBy(rt => new { rt.RouteName, rt.RouteId })
                        .Select(rt => new RouteAndQualificationsDto
                        {
                            RouteName = rt.Key.RouteName,
                            RouteId = rt.Key.RouteId,
                            QualificationShortTitles = rt.Select(q =>
                                q.QualificationShortTitle)
                                .OrderBy(q => q)
                        }).OrderBy(rt => rt.RouteName)
            }).OrderBy(r => r.Distance)
            .ThenBy(r =>
            {
                if (r.ProviderVenueName == r.ProviderVenuePostcode)
                    return r.ProviderDisplayName;
                else
                    return r.ProviderVenueName;
            })
            .ToList();

            var dto = new ProviderProximityReportDto
            {
                Providers = result,
                Postcode = searchParameters.Postcode,
                //TODO: Get selected route names
                //SkillAreas = searchParameters.SelectedRoutes
            };

            return dto;
        }

        private static Point GetSearchStartPoint(string latitude, string longitude)
        {
            if (string.IsNullOrWhiteSpace(latitude) || string.IsNullOrWhiteSpace(longitude))
                throw new InvalidOperationException("Latitude and Longitude can not be null or blank");

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            return geometryFactory.CreatePoint(new Coordinate(double.Parse(longitude), double.Parse(latitude)));
        }
    }
}
